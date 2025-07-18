﻿using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.Enums;

namespace HCL.CommentServer.API.Domain.Entities
{
    public class Comment
    {
        public Guid? Id { get; set; }
        public Guid AccountId { get; set; }
        public string ArticleId { get; set; }
        public string Content { get; set; }
        public CommentMark Mark { get; set; }
        public DateTime CreatedDate { get; set; }

        public Comment()
        {
        }

        public Comment(CommentDTO commentDTO, Guid accountId, string articleId)
        {
            this.AccountId = accountId;
            Content = commentDTO.Content;
            Mark = commentDTO.Mark ?? 0;
            CreatedDate = DateTime.Now;
            ArticleId = articleId;
        }

        public Comment(Guid? id)
        {
            Id = id;
        }
    }
}