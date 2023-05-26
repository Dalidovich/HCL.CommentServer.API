using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Test.IntegrationTest;
using Xunit;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using System.Security.Principal;

namespace HCL.CommentServer.API.Test.Services
{
    public class CommentServiceIntegrationTest
    {
        [Fact]
        public async Task CreateComment_WithRightData_ReturnNewComment()
        {
            //Arrange
            IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
            await pgContainer.StartAsync();
            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);

            using var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();
            var commRep = new CommentRepository(commentAppDBContext);
            var commServ = new CommentService(commRep, StandartMockBuilder.mockLoggerCommentService);
            var newComment = new Comment()
            {
                Content = "1",
                Mark=CommentMark.Good,
                AccountId=Guid.NewGuid(),
                ArticleId="1",
                CreatedDate=DateTime.Now
            };

            //Act
            var addedComment= await commServ.CreateComment(newComment);

            //Assert
            addedComment.Should().NotBeNull();
            addedComment.Data.Should().NotBeNull();
            addedComment.StatusCode.Should().Be(StatusCode.CommentCreate);

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WithExistComment_ReturnBooleanTrue()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            List<Comment> comments = new List<Comment>()
            {
                new Comment()
                {
                    Id=commentId,
                    CreatedDate= DateTime.Now,
                    ArticleId="a",
                    Content = "1",
                    Mark = CommentMark.Good,
                    AccountId = Guid.NewGuid()
                }
            };
            IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
            await pgContainer.StartAsync();
            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB, comments);

            using var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();
            var commRep = new CommentRepository(commentAppDBContext);
            var commServ = new CommentService(commRep, StandartMockBuilder.mockLoggerCommentService);

            //Act
            var deleteConfirm = await commServ.DeleteComment(commentId);

            //Assert
            deleteConfirm.Should().NotBeNull();
            deleteConfirm.StatusCode.Should().Be(StatusCode.CommentDelete);
            deleteConfirm.Data.Should().BeTrue();

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WithNotExistComment_ReturnError()
        {
            //Arrange
            IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
            await pgContainer.StartAsync();
            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);

            using var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();
            var commRep = new CommentRepository(commentAppDBContext);
            var commServ = new CommentService(commRep, StandartMockBuilder.mockLoggerCommentService);

            //Act
            var result = async () =>
            {
                var deleteConfirm = await commServ.DeleteComment(Guid.NewGuid());
            };
            

            //Assert
            result.Should().ThrowAsync<KeyNotFoundException>();

            await pgContainer.DisposeAsync();
        }
    }
}
