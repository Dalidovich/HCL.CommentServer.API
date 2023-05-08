using FluentAssertions;
using HCL.CommentServer.API.BLL.Services;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HCL.CommentServer.API.Test.Services
{
    public class CommentServiceTest
    {
        [Fact]
        public async Task CreateComment_WithRightData_ReturnNewComment()
        {
            //Arrange
            List<Comment> comments = new List<Comment>();

            var commRepMock=StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ=new CommentService(commRepMock.Object);
            var newComment = new Comment()
            {
                Content = "1",
                Mark=CommentMark.Good,
                AccountId=Guid.NewGuid()
            };

            //Act
            var addedComment= await commServ.CreateComment(newComment);

            //Assert
            addedComment.Should().NotBeNull();
            addedComment.Data.Should().NotBeNull();
            addedComment.StatusCode.Should().Be(StatusCode.CommentCreate);
            comments.Should().ContainSingle();
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
            var commRepMock = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(commRepMock.Object);

            //Act
            var deleteConfirm = await commServ.DeleteComment(commentId);

            //Assert
            deleteConfirm.Should().NotBeNull();
            deleteConfirm.StatusCode.Should().Be(StatusCode.CommentDelete);
            deleteConfirm.Data.Should().BeTrue();
            comments.Should().BeEmpty();
        }

        [Fact]
        public async Task DeleteComment_WithNotExistComment_ReturnBooleanTrue()
        {
            //Arrange
            List<Comment> comments = new List<Comment>();
            var commRepMock = StandartMockBuilder.CreateCommentRepositoryMock(comments);

            var commServ = new CommentService(commRepMock.Object);

            //Act
            var deleteConfirm = await commServ.DeleteComment(Guid.NewGuid());

            //Assert
            deleteConfirm.Should().NotBeNull();
            deleteConfirm.Data.Should().BeFalse();
        }
    }
}
