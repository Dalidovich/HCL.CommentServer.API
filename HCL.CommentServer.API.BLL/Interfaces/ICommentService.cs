using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.InnerResponse;

namespace HCL.CommentServer.API.BLL.Interfaces
{
    public interface ICommentService
    {
        public Task<BaseResponse<Comment>> CreateComment(Comment comment);
        public Task<BaseResponse<bool>> DeleteComment(Guid id);
        public BaseResponse<IQueryable<Comment>> GetCommentOData();
    }
}