using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_config : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Categories_ProductId",
                schema: "catalog",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Products_CategoryId",
                schema: "catalog",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Categories_CategoryId",
                schema: "catalog",
                table: "ProductCategory",
                column: "CategoryId",
                principalSchema: "catalog",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Products_ProductId",
                schema: "catalog",
                table: "ProductCategory",
                column: "ProductId",
                principalSchema: "catalog",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Categories_CategoryId",
                schema: "catalog",
                table: "ProductCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductCategory_Products_ProductId",
                schema: "catalog",
                table: "ProductCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Categories_ProductId",
                schema: "catalog",
                table: "ProductCategory",
                column: "ProductId",
                principalSchema: "catalog",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCategory_Products_CategoryId",
                schema: "catalog",
                table: "ProductCategory",
                column: "CategoryId",
                principalSchema: "catalog",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
