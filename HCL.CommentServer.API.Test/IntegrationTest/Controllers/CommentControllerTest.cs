using DotNet.Testcontainers.Containers;
using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Controllers;
using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Test.IntegrationTest;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Linq;
using Xunit;

namespace HCL.CommentServer.API.Test.Controllers
{
    public class CommentControllerIntegrationTest : IAsyncLifetime
    {
        private IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
        private CommentController commentController;
        private CommentRepository commentRepository;
        private WebApplicationFactory<Program> webHost;

        public async Task InitializeAsync()
        {
            await pgContainer.StartAsync();
            webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword,
                "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);
            
            var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();
            commentRepository = new CommentRepository(commentAppDBContext);
            var commService = new CommentService(commentRepository, StandartMockBuilder.mockLoggerCommentService);
            commentController = new CommentController(commService, StandartMockBuilder.mockLoggerController);
        }

        public async Task DisposeAsync()
        {
            await pgContainer.DisposeAsync();
        }

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
            
            await CustomTestHostBuilder.AddCommentInDBNotTracked(webHost, comments);

            //Act
            var noContentResult = await commentController.DeleteComment(accountId, commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
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

            await CustomTestHostBuilder.AddCommentInDBNotTracked(webHost, comments);

            //Act
            var forbidResult = await commentController.DeleteComment(Guid.NewGuid(), commentId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
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

            await CustomTestHostBuilder.AddCommentInDBNotTracked(webHost, comments);

            //Act
            var notFoundResult = await commentController.DeleteComment(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
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

            await CustomTestHostBuilder.AddCommentInDBNotTracked(webHost, comments);

            //Act
            var noContentResult = await commentController.DeleteComment(commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
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

            comments.ForEach(async x => await commentRepository.AddAsync(x));
            await commentRepository.SaveAsync();

            //Act
            var notFoundResult = await commentController.DeleteComment(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
        }
    }
}
