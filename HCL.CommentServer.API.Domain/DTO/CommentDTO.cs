using HCL.CommentServer.API.Domain.Enums;

namespace HCL.CommentServer.API.Domain.DTO
{
    public class CommentDTO
    {
        public Guid AccountId { get; set; }
        public string Content { get; set; }
        public CommentMark? Mark { get; set; }
    }
}