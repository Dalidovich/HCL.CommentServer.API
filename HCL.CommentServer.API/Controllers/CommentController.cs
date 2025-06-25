using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.Domain.DTO.Builders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HCL.CommentServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        } 

        [Authorize]
        [HttpDelete("v1/comment/account")]
        public async Task<IActionResult> DeleteComment([FromQuery] Guid ownId, [FromQuery] Guid id)
        {
            var comment = await _commentService.GetCommentOData().Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (comment == null)
            {

                return NotFound();
            }
            else if (comment.AccountId == ownId)
            {
                var resourse = await _commentService.DeleteComment(id);
                var log = new LogDTOBuidlder("DeleteComment(ownId,id)")
                    .BuildMessage("authenticated account delete own comment")
                    .BuildSuccessState(resourse.Data)
                    .BuildStatusCode(204)
                    .Build();
                _logger.LogInformation(JsonSerializer.Serialize(log));

                return NoContent();
            }

            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("v1/comment/admin")]
        public async Task<IActionResult> DeleteComment([FromQuery] Guid id)
        {
            var comment = await _commentService.GetCommentOData().Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (comment == null)
            {

                return NotFound();
            }
            var resourse = await _commentService.DeleteComment(id);
            var log = new LogDTOBuidlder("DeleteComment(id)")
                .BuildMessage("admin account delete comment")
                .BuildSuccessState(resourse.Data)
                .BuildStatusCode(204)
                .Build();
            _logger.LogInformation(JsonSerializer.Serialize(log));

            return NoContent();
        }
    }
}