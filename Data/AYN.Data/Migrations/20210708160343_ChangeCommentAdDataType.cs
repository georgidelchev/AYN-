using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class ChangeCommentAdDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ads_AdId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AdId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "AdId1",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "AdId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AdId",
                table: "Comments",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ads_AdId",
                table: "Comments",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Ads_AdId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_AdId",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "AdId",
                table: "Comments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AdId1",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AdId1",
                table: "Comments",
                column: "AdId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Ads_AdId1",
                table: "Comments",
                column: "AdId1",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
