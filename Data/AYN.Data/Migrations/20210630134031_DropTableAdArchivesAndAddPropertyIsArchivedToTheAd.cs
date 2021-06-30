using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class DropTableAdArchivesAndAddPropertyIsArchivedToTheAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdArchives");

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Ads",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Ads");

            migrationBuilder.CreateTable(
                name: "AdArchives",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdId = table.Column<int>(type: "int", nullable: false),
                    AdId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdArchives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdArchives_Ads_AdId1",
                        column: x => x.AdId1,
                        principalTable: "Ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdArchives_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdArchives_AdId1",
                table: "AdArchives",
                column: "AdId1");

            migrationBuilder.CreateIndex(
                name: "IX_AdArchives_IsDeleted",
                table: "AdArchives",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_AdArchives_UserId",
                table: "AdArchives",
                column: "UserId");
        }
    }
}
