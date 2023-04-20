using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCL.CommentServer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService relationshipService)
        {
            _commentService = relationshipService;
        }

        [Authorize]
        [HttpPost("v1/Comment")]
        public async Task<IActionResult> CreateRelationship([FromQuery] CommentDTO commentDTO)
        {
            var resourse = await _commentService.CreateComment(new Comment(commentDTO));
            if (resourse.Data != null)
            {

                return Created("", new { commentId = resourse.Data.Id });
            }

            return NotFound();
        }

        [Authorize]
        [HttpDelete("v1/OwnComment")]
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

                return NoContent();
            }

            return Forbid();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("v1/Comment")]
        public async Task<IActionResult> DeleteComment([FromQuery] Guid id)
        {
            var relation = await _commentService.GetCommentOData().Data
                ?.Where(x => x.Id == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            if (relation == null)
            {

                return NotFound();
            }
            var resourse = await _commentService.DeleteComment(id);

            return NoContent();
        }
    }
}