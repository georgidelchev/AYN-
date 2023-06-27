using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AYN.Data.Migrations
{
    public partial class MakeAddressIdRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ads_Addresses_AddressId",
                table: "Ads");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "Ads",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ads_Addresses_AddressId",
                table: "Ads",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
