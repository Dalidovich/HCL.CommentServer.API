using DotNet.Testcontainers.Containers;
using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Test.IntegrationTest;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HCL.CommentServer.API.Test.Services
{
    public class CommentServiceIntegrationTest : IAsyncLifetime
    {
        private IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
        private CommentService commentService;
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
            commentService = new CommentService(commentRepository, StandartMockBuilder.mockLoggerCommentService);
        }

        public async Task DisposeAsync()
        {
            await pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task CreateComment_WithRightData_ReturnNewComment()
        {
            //Arrange
            var newComment = new Comment()
            {
                Content = "1",
                Mark=CommentMark.Good,
                AccountId=Guid.NewGuid(),
                ArticleId="1",
                CreatedDate=DateTime.Now
            };

            //Act
            var addedComment= await commentService.CreateComment(newComment);

            //Assert
            addedComment.Should().NotBeNull();
            addedComment.Data.Should().NotBeNull();
            addedComment.StatusCode.Should().Be(StatusCode.CommentCreate);
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

            await CustomTestHostBuilder.AddCommentInDBNotTracked(webHost, comments);

            //Act
            var deleteConfirm = await commentService.DeleteComment(commentId);

            //Assert
            deleteConfirm.Should().NotBeNull();
            deleteConfirm.StatusCode.Should().Be(StatusCode.CommentDelete);
            deleteConfirm.Data.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteComment_WithNotExistComment_ReturnError()
        {
            //Arrange

            //Act
            var result = async () =>
            {
                var deleteConfirm = await commentService.DeleteComment(Guid.NewGuid());
            };
            

            //Assert
            result.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
