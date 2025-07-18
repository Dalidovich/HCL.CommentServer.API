using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HCL.CommentServer.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton(builder.Configuration);
            builder.AddRepositores();
            builder.AddServices();
            builder.AddAuthProperty();
            builder.AddODataProperty();
            builder.AddHostedServices();
            builder.AddSignalRProperty();
            builder.AddElasticserchProperty();

            builder.Services.AddDbContext<CommentAppDBContext>(opt => opt.UseNpgsql(
                builder.Configuration.GetConnectionString(StandartConst.NameConnection)));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.AddMiddleware();
            app.AddSignalR();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}