using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.Entities;
using Moq;

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
            };
            comments.Add(comm);

            return comm;
        }

        public static Mock<ICommentRepository> CreateCommentRepositoryMock(List<Comment> comments)
        {
            var commRep=new Mock<ICommentRepository>();
            commRep.Setup(r => r
                .AddAsync(It.IsAny<Comment>()))
                .ReturnsAsync((Comment comment) => _addComment(comment, comments));

            commRep.Setup(r => r.SaveAsync());

            commRep.Setup(r => r.GetAsync())
                .Returns(comments.AsQueryable());

            return commRep;
        }
    }
}
