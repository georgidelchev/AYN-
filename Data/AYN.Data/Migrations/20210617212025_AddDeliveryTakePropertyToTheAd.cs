using Microsoft.EntityFrameworkCore.Migrations;

namespace AYN.Data.Migrations
{
    public partial class AddDeliveryTakePropertyToTheAd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryTake",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryTake",
                table: "Ads");
        }
    }
}
