using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HCL.CommentServer.API.Controllers
{
    public class CommentODataController : ODataController
    {
        private readonly ICommentService _commentService;

        public CommentODataController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("odata/v1/comment")]
        [EnableQuery]
        public IQueryable<Comment> GetComment()
        {

            return _commentService.GetCommentOData().Data;
        }
    }
}