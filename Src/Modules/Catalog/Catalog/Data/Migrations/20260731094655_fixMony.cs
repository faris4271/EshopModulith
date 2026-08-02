using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMony : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpecialPrice",
                schema: "catalog",
                table: "Products",
                newName: "SpecialPrice_Amount");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "catalog",
                table: "Products",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "OldPrice",
                schema: "catalog",
                table: "Products",
                newName: "OldPrice_Amount");

            migrationBuilder.RenameColumn(
                name: "SpecialPrice",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "SpecialPrice_Amount");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "Price_Amount");

            migrationBuilder.RenameColumn(
                name: "OldPrice",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "OldPrice_Amount");

            migrationBuilder.AddColumn<string>(
                name: "OldPrice_Currency",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price_Currency",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialPrice_Currency",
                schema: "catalog",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldPrice_Currency",
                schema: "catalog",
                table: "ProductPriceHistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price_Currency",
                schema: "catalog",
                table: "ProductPriceHistory",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialPrice_Currency",
                schema: "catalog",
                table: "ProductPriceHistory",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice_Currency",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Price_Currency",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialPrice_Currency",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OldPrice_Currency",
                schema: "catalog",
                table: "ProductPriceHistory");

            migrationBuilder.DropColumn(
                name: "Price_Currency",
                schema: "catalog",
                table: "ProductPriceHistory");

            migrationBuilder.DropColumn(
                name: "SpecialPrice_Currency",
                schema: "catalog",
                table: "ProductPriceHistory");

            migrationBuilder.RenameColumn(
                name: "SpecialPrice_Amount",
                schema: "catalog",
                table: "Products",
                newName: "SpecialPrice");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                schema: "catalog",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "OldPrice_Amount",
                schema: "catalog",
                table: "Products",
                newName: "OldPrice");

            migrationBuilder.RenameColumn(
                name: "SpecialPrice_Amount",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "SpecialPrice");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "OldPrice_Amount",
                schema: "catalog",
                table: "ProductPriceHistory",
                newName: "OldPrice");
        }
    }
}
