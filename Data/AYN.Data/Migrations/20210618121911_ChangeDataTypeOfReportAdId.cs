using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class ChangeDataTypeOfReportAdId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ads_ReportedAdId1",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportedAdId1",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReportedAdId1",
                table: "Reports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportedAdId",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedAdId",
                table: "Reports",
                column: "ReportedAdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ads_ReportedAdId",
                table: "Reports",
                column: "ReportedAdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Ads_ReportedAdId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReportedAdId",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "ReportedAdId",
                table: "Reports",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ReportedAdId1",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedAdId1",
                table: "Reports",
                column: "ReportedAdId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Ads_ReportedAdId1",
                table: "Reports",
                column: "ReportedAdId1",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
