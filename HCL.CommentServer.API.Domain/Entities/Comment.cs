using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.Enums;

namespace HCL.CommentServer.API.Domain.Entities
{
    public class Comment
    {
        public Guid? Id { get; set; }
        public Guid AccountId { get; set; }
        public string Content { get; set; }
        public CommentMark Mark { get; set; }
        public DateTime CreatedDate { get; set; }

        public Comment()
        {
        }

        public Comment(CommentDTO commentDTO)
        {
            AccountId = commentDTO.AccountId;
            Content = commentDTO.Content;
            Mark = commentDTO.Mark ?? 0;
            CreatedDate = DateTime.Now;
        }

        public Comment(Guid? id)
        {
            Id = id;
        }
    }
}