using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.Entities;
using MockQueryable.Moq;
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
                ArticleId= Guid.NewGuid().ToString(),
            };
            comments.Add(comm);

            return comm;
        }

        public static Mock<ICommentRepository> CreateCommentRepositoryMock(List<Comment> comments)
        {
            var commRep=new Mock<ICommentRepository>();
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
    }
}
