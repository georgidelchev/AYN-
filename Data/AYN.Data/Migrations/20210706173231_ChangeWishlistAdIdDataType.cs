using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class ChangeWishlistAdIdDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Ads_AdId1",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AdId1",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "AdId1",
                table: "Wishlists");

            migrationBuilder.AlterColumn<string>(
                name: "AdId",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AdId",
                table: "Wishlists",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Ads_AdId",
                table: "Wishlists",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Ads_AdId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_AdId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "AdId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AdId1",
                table: "Wishlists",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_AdId1",
                table: "Wishlists",
                column: "AdId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Ads_AdId1",
                table: "Wishlists",
                column: "AdId1",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
