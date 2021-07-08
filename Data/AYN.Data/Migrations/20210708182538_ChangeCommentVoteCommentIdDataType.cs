using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class ChangeCommentVoteCommentIdDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_Comments_CommentId1",
                table: "CommentVotes");

            migrationBuilder.DropIndex(
                name: "IX_CommentVotes_CommentId1",
                table: "CommentVotes");

            migrationBuilder.DropColumn(
                name: "CommentId1",
                table: "CommentVotes");

            migrationBuilder.AlterColumn<string>(
                name: "CommentId",
                table: "CommentVotes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_CommentId",
                table: "CommentVotes",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_Comments_CommentId",
                table: "CommentVotes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_Comments_CommentId",
                table: "CommentVotes");

            migrationBuilder.DropIndex(
                name: "IX_CommentVotes_CommentId",
                table: "CommentVotes");

            migrationBuilder.AlterColumn<int>(
                name: "CommentId",
                table: "CommentVotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CommentId1",
                table: "CommentVotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_CommentId1",
                table: "CommentVotes",
                column: "CommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_Comments_CommentId1",
                table: "CommentVotes",
                column: "CommentId1",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
