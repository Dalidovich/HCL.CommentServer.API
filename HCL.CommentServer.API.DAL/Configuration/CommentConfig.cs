using HCL.CommentServer.API.DAL.Configuration.DataType;
using HCL.CommentServer.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCL.CommentServer.API.DAL.Configuration
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public const string Table_name = "comments";

        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable(Table_name);

            builder.HasKey(e => new { e.Id });
            builder.HasIndex(e => e.AccountId);

            builder.Property(e => e.Id)
                   .HasColumnType(EntityDataTypes.Guid)
                   .HasColumnName("pk_account_id");

            builder.Property(e => e.AccountId)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("account_id");

            builder.Property(e => e.Mark)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("mark");

            builder.Property(e => e.Content)
                   .HasColumnType(EntityDataTypes.Character_varying)
                   .HasColumnName("content");

            builder.Property(e => e.CreatedDate)
                   .HasColumnName("create_date");
        }
    }
}