using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.InnerResponse;

namespace HCL.CommentServer.API.BLL.Interfaces
{
    public interface ICommentService
    {
        public Task<BaseResponse<Comment>> CreateComment(CommentDTO commentDTO);
        public Task<BaseResponse<bool>> DeleteComment(Guid id);
        public BaseResponse<IQueryable<Comment>> GetCommentOData();
    }
}