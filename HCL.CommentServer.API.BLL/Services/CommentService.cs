using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.DTO.Builders;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using HCL.CommentServer.API.Domain.InnerResponse;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HCL.CommentServer.API.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentService> _logger;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<BaseResponse<Comment>> CreateComment(Comment comment)
        {
            var createdComment = await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveAsync();

            var log = new LogDTOBuidlder("CreateComment(comment)")
                .BuildMessage("create comment")
                .BuildSuccessState(true)
                .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return new StandartResponse<Comment>()
            {
                Data = createdComment,
                StatusCode = StatusCode.CommentCreate
            };
        }

        public async Task<BaseResponse<bool>> DeleteComment(Guid id)
        {
            var deletedComment = _commentRepository.Delete(new Comment(id));
            await _commentRepository.SaveAsync();

            var log = new LogDTOBuidlder("DeleteComment(id)")
                .BuildMessage("delete comment")
                .BuildSuccessState(true)
                .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return new StandartResponse<bool>()
            {
                Data = deletedComment,
                StatusCode = StatusCode.CommentDelete
            };
        }

        public BaseResponse<IQueryable<Comment>> GetCommentOData()
        {
            var log = new LogDTOBuidlder("GetCommentOData()");
            var contents = _commentRepository.GetAsync();
            if (contents.Count() == 0)
            {
                log.BuildMessage("no comments");
                _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

                return new StandartResponse<IQueryable<Comment>>()
                {
                    Message = "entity not found"
                };
            }

            log.BuildMessage("get comments");
            _logger.LogInformation(JsonSerializer.Serialize(log.Build()));

            return new StandartResponse<IQueryable<Comment>>()
            {
                Data = contents,
                StatusCode = StatusCode.CommentRead
            };
        }
    }
}