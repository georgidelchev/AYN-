using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class RenameUserAvatarAndThumbnailFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailExtension",
                table: "AspNetUsers",
                newName: "ThumbnailImageUrl");

            migrationBuilder.RenameColumn(
                name: "AvatarExtension",
                table: "AspNetUsers",
                newName: "AvatarImageUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThumbnailImageUrl",
                table: "AspNetUsers",
                newName: "ThumbnailExtension");

            migrationBuilder.RenameColumn(
                name: "AvatarImageUrl",
                table: "AspNetUsers",
                newName: "AvatarExtension");
        }
    }
}
