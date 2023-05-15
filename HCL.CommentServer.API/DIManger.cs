using HCL.CommentServer.API.BackgroundHostedService;
using HCL.CommentServer.API.BLL.Hubs;
using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.DTO.SignalRDTO;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Serilog;
using System.Text;

namespace HCL.CommentServer.API
{
    public static class DIManger
    {
        public static void AddRepositores(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<ICommentRepository, CommentRepository>();
        }

        public static void AddServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddScoped<ICommentService, CommentService>();
        }

        public static void AddSignalRProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddSignalR();
            webApplicationBuilder.Services.AddSingleton<ChatManager>();
        }

        public static void AddElasticserchProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            ElasticsearchHelper.ConfigureLogging();
            webApplicationBuilder.Host.UseSerilog();
        }

        public static void AddODataProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Comment>("Comment");

            webApplicationBuilder.Services.AddControllers().AddOData(opt =>
            {
                opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(5000);
                opt.TimeZone = TimeZoneInfo.Utc;
            });
        }

        public static void AddAuthProperty(this WebApplicationBuilder webApplicationBuilder)
        {
            var secretKey = webApplicationBuilder.Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = webApplicationBuilder.Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = webApplicationBuilder.Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            webApplicationBuilder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuerSigningKey = true,
                };
            });
        }

        public static void AddHostedServices(this WebApplicationBuilder webApplicationBuilder)
        {
            webApplicationBuilder.Services.AddHostedService<CheckDBHostedService>();
        }

        public static void AddMiddleware(this WebApplication webApplication)
        {
            webApplication.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void AddSignalR(this WebApplication webApplication)
        {
            webApplication.MapHub<CommentHub>("/comment");
        }
    }
}