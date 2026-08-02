using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Module.Inventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_Phone",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_State",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                schema: "inventory",
                table: "Warehouse",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressId",
                schema: "inventory",
                table: "Warehouse");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "inventory",
                table: "Warehouse",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                schema: "inventory",
                table: "Warehouse",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Phone",
                schema: "inventory",
                table: "Warehouse",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                schema: "inventory",
                table: "Warehouse",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                schema: "inventory",
                table: "Warehouse",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                schema: "inventory",
                table: "Warehouse",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                schema: "inventory",
                table: "Warehouse",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
