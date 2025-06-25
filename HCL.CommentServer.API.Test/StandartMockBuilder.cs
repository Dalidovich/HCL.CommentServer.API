using HCL.CommentServer.API.BLL.Hubs.Interfaces;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Controllers;
using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System.Security.Claims;

namespace HCL.CommentServer.API.Test
{
    public class StandartMockBuilder
    {
        private static Comment _addComment(Comment comment, List<Comment> comments)
        {
            var comm = new Comment()
            {
                AccountId = comment.AccountId,
                Content = comment.Content,
                CreatedDate = DateTime.Now,
                Mark = comment.Mark,
                Id = Guid.NewGuid(),
                ArticleId = Guid.NewGuid().ToString(),
            };
            comments.Add(comm);

            return comm;
        }

        public static readonly ILogger<CommentController> mockLoggerController = new Mock<ILogger<CommentController>>().Object;
        public static readonly ILogger<CommentService> mockLoggerCommentService = new Mock<ILogger<CommentService>>().Object;

        public static Mock<ICommentRepository> CreateCommentRepositoryMock(List<Comment> comments)
        {
            var commRep = new Mock<ICommentRepository>();
            var collectionQuerybleMock = comments.BuildMock();
            commRep.Setup(r => r
                .AddAsync(It.IsAny<Comment>()))
                .ReturnsAsync((Comment comment) => _addComment(comment, comments));

            commRep.Setup(r => r.SaveAsync());

            commRep.Setup(r => r.GetAsync())
                .Returns(collectionQuerybleMock);

            commRep.Setup(r => r.Delete(It.IsAny<Comment>()))
                .Returns((Comment comment) =>
                {

                    return comments.RemoveAll(x => x.Id == comment.Id) == 1;
                });

            commRep.Setup(r => r.Update(It.IsAny<Comment>()))
                .Returns((Comment comment) =>
                {
                    var updated = comments.Where(x => x.Id == comment.Id).SingleOrDefault();

                    if (updated != null)
                    {
                        comments.Remove(updated);
                        comments.Add(comment);

                        return comment;
                    }

                    return null;
                });

            return commRep;
        }

        public static Mock<IEnumerable<ClaimsIdentity>> CreateClaimsIdentityListMock(Guid id)
        {
            List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(new List<Claim>{ new Claim("Id",id.ToString())})
            };

            var mock = new Mock<IEnumerable<ClaimsIdentity>>();
            mock.Setup(x => x.GetEnumerator())
                .Returns(claimsIdentities.GetEnumerator());

            return mock;
        }

        public static Mock<IHubCallerClients<ICommentHub>> CreateHubClientsHubMock()
        {
            var all = new Mock<IHubCallerClients<ICommentHub>>();
            all.Setup(x => x.OthersInGroup(It.IsAny<string>())
                .SendCommentInGroupAsync(It.IsAny<CommentDTO>(), It.IsAny<string>()));

            return all;
        }

        public static Mock<HubCallerContext> CreteContextMock(string name, string connId, IEnumerable<ClaimsIdentity> claims)
        {
            var mockContext = new Mock<HubCallerContext>();
            mockContext.Setup(u => u.User.Identity.Name)
                .Returns(name);

            mockContext.Setup(u => u.ConnectionId)
                .Returns(connId);

            mockContext.Setup(u => u.User.Identities)
                .Returns(claims);

            return mockContext;
        }
    }
}
