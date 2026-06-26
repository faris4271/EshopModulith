using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_Option : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductOptionValue_ProductOption_OptionId1",
                schema: "catalog",
                table: "ProductOptionValue");

            migrationBuilder.DropIndex(
                name: "IX_ProductOptionValue_OptionId1",
                schema: "catalog",
                table: "ProductOptionValue");

            migrationBuilder.DropColumn(
                name: "OptionId1",
                schema: "catalog",
                table: "ProductOptionValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OptionId1",
                schema: "catalog",
                table: "ProductOptionValue",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductOptionValue_OptionId1",
                schema: "catalog",
                table: "ProductOptionValue",
                column: "OptionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOptionValue_ProductOption_OptionId1",
                schema: "catalog",
                table: "ProductOptionValue",
                column: "OptionId1",
                principalSchema: "catalog",
                principalTable: "ProductOption",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
