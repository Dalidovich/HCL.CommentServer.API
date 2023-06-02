using DotNet.Testcontainers.Containers;
using FluentAssertions;
using HCL.CommentServer.API.BLL.Hubs;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.DAL;
using HCL.CommentServer.API.DAL.Repositories;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.DTO.SignalRDTO;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Test.IntegrationTest;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HCL.CommentServer.API.Test.Services
{
    public class SignalRIntegrationTest
    {
        [Fact]
        public async Task CreateComment_CreateWithSignalr_ReturnNewComment()
        {
            //Arrange
            IContainer pgContainer = TestContainerBuilder.CreatePostgreSQLContainer();
            await pgContainer.StartAsync();
            var webHost = CustomTestHostBuilder.Build(TestContainerBuilder.npgsqlUser, TestContainerBuilder.npgsqlPassword
                , "localhost", pgContainer.GetMappedPublicPort(5432), TestContainerBuilder.npgsqlDB);

            using var scope = webHost.Services.CreateScope();
            var commentAppDBContext = scope.ServiceProvider.GetRequiredService<CommentAppDBContext>();


            var commRepository = new CommentRepository(commentAppDBContext);

            var commService = new CommentService(commRepository, StandartMockBuilder.mockLoggerCommentService);

            string group = Guid.NewGuid().ToString();
            var id = Guid.NewGuid();

            var chatManager = new ChatManager();
            var hub = new CommentHub(chatManager, commService);

            var all = StandartMockBuilder.CreateHubClientsHubMock();
            var mockClaims = StandartMockBuilder.CreateClaimsIdentityListMock(id);
            var mockContext = StandartMockBuilder.CreteContextMock("acc1", "acc1", mockClaims.Object);

            hub.Clients = all.Object;
            hub.Context = mockContext.Object;
            hub.Groups = new Mock<IGroupManager>().Object;

            var commentDto = new CommentDTO()
            {
                Content = "1",
                Mark = CommentMark.Bad
            };

            //Act
            await hub.OnConnectedAsync();
            await hub.SetConnectionInGroup(group);
            await hub.SendCommentInGroupAsync(commentDto, group);

            //Assert
            var comment = await commService.GetCommentOData().Data
                .Where(x => 
                x.Content == commentDto.Content 
                &&
                x.Mark == commentDto.Mark)
                .SingleOrDefaultAsync();
            comment.Should().NotBeNull();
        }
    }
}
