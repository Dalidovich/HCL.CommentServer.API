using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;

namespace HCL.CommentServer.API.Test.IntegrationTest
{
    public class CustomTestHostBuilder
    {
        public static WebApplicationFactory<Program> Build(string dbUser, string dbPassword, string dbServer, ushort dbPort, string dbName)
        {
            var npgsqlConnectionString = $"User Id={dbUser}; Password={dbPassword}; Server={dbServer}; " +
                $"Port={dbPort}; Database={dbName}; IntegratedSecurity=true; Pooling=true";

            return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                             d.ServiceType == typeof(DbContextOptions<CommentAppDBContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<CommentAppDBContext>(options =>
                    {
                        options.UseNpgsql(npgsqlConnectionString);
                    });

                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<CommentAppDBContext>();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                });
            });
        }

        public static WebApplicationFactory<Program> Build(string dbUser, string dbPassword, string dbServer, ushort dbPort, string dbName, List<Comment> comments)
        {
            var npgsqlConnectionString = $"User Id={dbUser}; Password={dbPassword}; Server={dbServer}; " +
                $"Port={dbPort}; Database={dbName}; IntegratedSecurity=true; Pooling=true";

            return new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(async services =>
                {
                    var dbContextDescriptor = services.SingleOrDefault(d =>
                             d.ServiceType == typeof(DbContextOptions<CommentAppDBContext>));

                    services.Remove(dbContextDescriptor);

                    services.AddDbContext<CommentAppDBContext>(options =>
                    {
                        options.UseNpgsql(npgsqlConnectionString);
                    });

                    var serviceProvider = services.BuildServiceProvider();
                    using var scope = serviceProvider.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<CommentAppDBContext>();
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    await context.Comments.AddRangeAsync(comments);

                    await context.SaveChangesAsync();
                });
            });
        }
    }
}
