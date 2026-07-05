using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Module.Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "sku",
                schema: "inventory",
                table: "Stocks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sku",
                schema: "inventory",
                table: "Stocks");
        }
    }
}
