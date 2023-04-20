using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Domain.InnerResponse;

namespace HCL.CommentServer.API.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<BaseResponse<Comment>> CreateComment(Comment comment)
        {
            var createdRelationship = await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            return new StandartResponse<Comment>()
            {
                Data = createdRelationship,
                StatusCode = StatusCode.CommentCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteComment(Guid id)
        {
            var createdRelationship = _commentRepository.Delete(new Comment(id));
            await _commentRepository.SaveAsync();

            return new StandartResponse<bool>()
            {
                Data = createdRelationship,
                StatusCode = StatusCode.CommentDelete
            };
        }

        public BaseResponse<IQueryable<Comment>> GetCommentOData()
        {
            var contents = _commentRepository.GetAsync();
            if (contents.Count() == 0)
            {

                return new StandartResponse<IQueryable<Comment>>()
                {
                    Message = "entity not found"
                };
            }

            return new StandartResponse<IQueryable<Comment>>()
            {
                Data = contents,
                StatusCode = StatusCode.CommentRead
            };
        }
    }
}