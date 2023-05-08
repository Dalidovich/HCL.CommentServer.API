using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Controllers;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace HCL.CommentServer.API.Test.Controllers
{
    public class CommentControllerTest
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
                    Content="First"
                }
            };

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var controller = new CommentController(commServ);

            //Act
            var noContentResult = await controller.DeleteComment(accountId, commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            comments.Should().BeEmpty();
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
                    Content="First"
                }
            };

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var controller = new CommentController(commServ);

            //Act
            var forbidResult = await controller.DeleteComment(Guid.NewGuid(), commentId) as ForbidResult;

            //Assert
            forbidResult.Should().NotBeNull();
            comments.Should().NotBeEmpty();
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
                    Content="First"
                }
            };

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var controller = new CommentController(commServ);

            //Act
            var notFoundResult = await controller.DeleteComment(Guid.NewGuid(), Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            comments.Should().NotBeEmpty();
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
                    Content="First"
                }
            };

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var controller = new CommentController(commServ);

            //Act
            var noContentResult = await controller.DeleteComment(commentId) as NoContentResult;

            //Assert
            noContentResult.Should().NotBeNull();
            comments.Should().BeEmpty();
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
                    Content="First"
                }
            };

            var mockCommRep = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(mockCommRep.Object);
            var controller = new CommentController(commServ);

            //Act
            var notFoundResult = await controller.DeleteComment(Guid.NewGuid()) as NotFoundResult;

            //Assert
            notFoundResult.Should().NotBeNull();
            comments.Should().NotBeEmpty();
        }
    }
}
