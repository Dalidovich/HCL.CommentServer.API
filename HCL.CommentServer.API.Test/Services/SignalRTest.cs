using FluentAssertions;
using HCL.CommentServer.API.BLL.Hubs;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.DTO.SignalRDTO;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;

namespace HCL.CommentServer.API.Test.Services
{
    public class SignalRTest
    {
        [Fact]
        public async Task CreateComment_CreateWithSignalr_ReturnNewComment()
        {
            //Arrange
            List<Comment> comments = new List<Comment>();

            string group = Guid.NewGuid().ToString();
            var id = Guid.NewGuid();

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var chatManager = new ChatManager();
            var commServ = new CommentService(mockCommRep.Object, StandartMockBuilder.mockLoggerCommentService);
            var hub = new CommentHub(chatManager, commServ);

            var all = StandartMockBuilder.CreateHubClientsHubMock();
            var mockClaims = StandartMockBuilder.CreateClaimsIdentityListMock(id);
            var mockContext = StandartMockBuilder.CreteContextMock("acc1", "acc1", mockClaims.Object);

            hub.Clients = all.Object;
            hub.Context = mockContext.Object;
            hub.Groups = new Mock<IGroupManager>().Object;

            var commentDto = new CommentDTO()
            {
                Content = "1",
                Mark = Domain.Enums.CommentMark.Normal
            };

            //Act
            await hub.OnConnectedAsync();
            await hub.SetConnectionInGroup(group);
            await hub.SendCommentInGroupAsync(commentDto, group);

            //Assert
            comments.Should().NotBeEmpty();
            comments.Should().ContainSingle();
            comments.Where(x => x.Content == "1").SingleOrDefault().Mark.Should().Be(commentDto.Mark);
        }
    }
}
