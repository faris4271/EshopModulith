using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eshop.Module.Shipping.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shipping");

            migrationBuilder.CreateTable(
                name: "ShippingProviders",
                schema: "shipping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    ConfigureUrl = table.Column<string>(type: "text", nullable: true),
                    ToAllShippingEnabledCountries = table.Column<bool>(type: "boolean", nullable: false),
                    OnlyCountryIdsString = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ToAllShippingEnabledStatesOrProvinces = table.Column<bool>(type: "boolean", nullable: false),
                    OnlyStateOrProvinceIdsString = table.Column<string>(type: "text", nullable: true),
                    AdditionalSettings = table.Column<string>(type: "text", nullable: true),
                    ShippingPriceServiceTypeName = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingProviders", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "shipping",
                table: "ShippingProviders",
                columns: new[] { "Id", "AdditionalSettings", "ConfigureUrl", "IsEnabled", "Name", "OnlyCountryIdsString", "OnlyStateOrProvinceIdsString", "ShippingPriceServiceTypeName", "ToAllShippingEnabledCountries", "ToAllShippingEnabledStatesOrProvinces" },
                values: new object[,]
                {
                    { new Guid("0bd42d3a-5d1d-4e45-9546-b3a2e6ddc4fc"), null, "shipping-free-config", true, "FreeShipping", "SimplCommerce.Module.ShippingTableRate.Services.TableRateShippingServiceProvider,SimplCommerce.Module.ShippingFree", null, null, true, true },
                    { new Guid("de885d99-5de2-4b03-9bbd-9a0a210b85bc"), null, "shipping-table-rate-config", true, "TableRate", "SimplCommerce.Module.ShippingTableRate.Services.TableRateShippingServiceProvider,SimplCommerce.Module.ShippingTableRate", null, null, true, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShippingProviders",
                schema: "shipping");
        }
    }
}
