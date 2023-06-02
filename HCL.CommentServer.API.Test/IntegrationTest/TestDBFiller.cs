using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;

namespace HCL.CommentServer.API.Test.IntegrationTest
{
    public class TestDBFiller
    {
        public static async Task AddCommentInDBNotTracked(WebApplicationFactory<Program> webHost, List<Comment> comments)
        {
            var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();
            await commentAppDBContext.AddRangeAsync(comments);
            await commentAppDBContext.SaveChangesAsync();
            scope.Dispose();
        }
    }
}
