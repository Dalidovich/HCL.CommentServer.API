using DotNet.Testcontainers.Containers;
using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Controllers;
using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Test.IntegrationTest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HCL.CommentServer.API.Test.Controllers
{
    public class CommentControllerIntegrationTest
    {
        [Fact]
        public async Task DeleteComment_WhenExistCommentIsMine_ReturnNoContent()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            List<Comment> comments = new List<Comment>
            {
                new Comment()
                {
                    Id=commentId,
                    AccountId=accountId,
                    Content="First",
                    ArticleId="1",
                    CreatedDate=DateTime.Now,
                    Mark=CommentMark.Normal
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
            var controller = new CommentController(commServ, StandartMockBuilder.mockLoggerController);

            //Act
            var noContentResult = await controller.DeleteComment(accountId, commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WhenExistCommentIsNotMine_ReturnForbid()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            List<Comment> comments = new List<Comment>
            {
                new Comment()
                {
                    Id=commentId,
                    AccountId=Guid.NewGuid(),
                    Content="First",
                    ArticleId="1",
                    CreatedDate=DateTime.Now,
                    Mark=CommentMark.Normal
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
            var controller = new CommentController(commServ, StandartMockBuilder.mockLoggerController);

            //Act
            var forbidResult = await controller.DeleteComment(Guid.NewGuid(), commentId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WhenNotExistCommentIsNotMine_ReturnNotFound()
        {
            //Arrange
            List<Comment> comments = new List<Comment>
            {
                new Comment()
                {
                    Id=Guid.NewGuid(),
                    AccountId=Guid.NewGuid(),
                    Content="First",
                    ArticleId="1",
                    CreatedDate=DateTime.Now,
                    Mark=CommentMark.Normal
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
            var controller = new CommentController(commServ, StandartMockBuilder.mockLoggerController);

            //Act
            var notFoundResult = await controller.DeleteComment(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WhenExistComment_ReturnNoContent()
        {
            //Arrange
            var commentId = Guid.NewGuid();
            List<Comment> comments = new List<Comment>
            {
                new Comment()
                {
                    Id=commentId,
                    AccountId=Guid.NewGuid(),
                    Content="First",
                    ArticleId="1",
                    CreatedDate=DateTime.Now,
                    Mark=CommentMark.Normal
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
            var controller = new CommentController(commServ, StandartMockBuilder.mockLoggerController);

            //Act
            var noContentResult = await controller.DeleteComment(commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();

            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task DeleteComment_WhenNotExistComment_ReturnNotFound()
        {
            //Arrange
            List<Comment> comments = new List<Comment>
            {
                new Comment()
                {
                    Id=Guid.NewGuid(),
                    AccountId=Guid.NewGuid(),
                    Content="First",
                    ArticleId="1",
                    CreatedDate=DateTime.Now,
                    Mark=CommentMark.Normal
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
            var controller = new CommentController(commServ, StandartMockBuilder.mockLoggerController);

            //Act
            var notFoundResult = await controller.DeleteComment(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();

            await pgContainer.DisposeAsync();
        }
    }
}
