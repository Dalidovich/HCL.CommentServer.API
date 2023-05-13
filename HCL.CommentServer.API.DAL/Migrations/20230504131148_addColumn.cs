using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HCL.CommentServer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pk_account_id",
                table: "comments",
                newName: "pk_comment_id");

            migrationBuilder.AddColumn<string>(
                name: "article_id",
                table: "comments",
                type: "character varying",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "article_id",
                table: "comments");

            migrationBuilder.RenameColumn(
                name: "pk_comment_id",
                table: "comments",
                newName: "pk_account_id");
        }
    }
}
