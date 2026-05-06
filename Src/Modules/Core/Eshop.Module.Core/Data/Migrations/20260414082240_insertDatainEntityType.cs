using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class insertDatainEntityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Core",
                table: "EntityType",
                columns: new[] { "Id", "AreaName", "CreatedAt", "CreatedBy", "IsMenuable", "LasteModified", "LasteModifiedBy", "RoutingAction", "RoutingController" },
                values: new object[,]
                {
                    { "Brand", "Catalog", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "BrandDetail", "Brand" },
                    { "Category", "Catalog", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "CategoryDetail", "Category" },
                    { "Product", "Catalog", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", "ProductDetail", "Product" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Brand");

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Category");

            migrationBuilder.DeleteData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Product");
        }
    }
}
