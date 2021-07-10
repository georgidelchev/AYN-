using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class AddAddressIdPropertyToTheAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Ads",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ads_AddressId",
                table: "Ads",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Addresses_AddressId",
                table: "Ads",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Addresses_AddressId",
                table: "Ads");

            migrationBuilder.DropIndex(
                name: "IX_Ads_AddressId",
                table: "Ads");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Ads");
        }
    }
}
