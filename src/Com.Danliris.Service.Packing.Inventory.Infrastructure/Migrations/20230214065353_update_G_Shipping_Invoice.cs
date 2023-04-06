using Microsoft.EntityFrameworkCore.Migrations;

namespace Com.Danliris.Service.Packing.Inventory.Infrastructure.Migrations
{
    public partial class update_G_Shipping_Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attn",
                table: "GarmentShippingInvoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "GarmentShippingInvoices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attn",
                table: "GarmentShippingInvoices");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "GarmentShippingInvoices");
        }
    }
}
