using FluentAssertions;
using HCL.CommentServer.API.BLL.Hubs;
using HCL.CommentServer.API.BLL.Hubs.Interfaces;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.DTO.SignalRDTO;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System.Security.Claims;
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

            var chatManager = new ChatManager();
            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var hub = new CommentHub(chatManager, commServ);

            var all = new Mock<IHubCallerClients<ICommentHub>>();
            all.Setup(x => x.OthersInGroup(It.IsAny<string>())
                .SendCommentInGroupAsync(It.IsAny<CommentDTO>(), It.IsAny<string>()));

            List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>{ new Claim("Id",id.ToString())})
            };
            var mockClaims1 = new Mock<IEnumerable<ClaimsIdentity>>();
            mockClaims1.Setup(x => x.GetEnumerator()).Returns(claimsIdentities.GetEnumerator());

            var mockContext = new Mock<HubCallerContext>();
            mockContext.Setup(u => u.User.Identity.Name).Returns("acc1");
            mockContext.Setup(u => u.ConnectionId).Returns("acc1");
            mockContext.Setup(u => u.User.Identities).Returns(mockClaims1.Object);

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
