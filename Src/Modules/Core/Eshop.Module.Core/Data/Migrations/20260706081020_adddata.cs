using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class adddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Core",
                table: "EntityType",
                columns: new[] { "Id", "AreaName", "CreatedById", "CreatedOn", "IsMenuable", "LatestUpdatedById", "LatestUpdatedOn", "RoutingAction", "RoutingController" },
                values: new object[] { "Vendor", "Catalog", null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, null, null, "VendorDetail", "Vendor" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Vendor");
        }
    }
}
