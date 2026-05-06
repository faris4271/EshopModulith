using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateDDD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "media");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "Core",
                table: "media");

            migrationBuilder.DropColumn(
                name: "LasteModified",
                schema: "Core",
                table: "media");

            migrationBuilder.DropColumn(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "media");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "Core",
                table: "entities");

            migrationBuilder.DropColumn(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "entities");

            migrationBuilder.RenameColumn(
                name: "LasteModified",
                schema: "Core",
                table: "EntityType",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "LasteModified",
                schema: "Core",
                table: "entities",
                newName: "CreatedOnUtc");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Core",
                table: "EntityType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "Core",
                table: "EntityType",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "EntityType",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Core",
                table: "entities",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "Core",
                table: "entities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "entities",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Brand",
                columns: new[] { "CreatedBy", "CreatedOnUtc", "LastModifiedBy", "LastModifiedOnUtc" },
                values: new object[] { null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Category",
                columns: new[] { "CreatedBy", "CreatedOnUtc", "LastModifiedBy", "LastModifiedOnUtc" },
                values: new object[] { null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Product",
                columns: new[] { "CreatedBy", "CreatedOnUtc", "LastModifiedBy", "LastModifiedOnUtc" },
                values: new object[] { null, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Core",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "Core",
                table: "entities");

            migrationBuilder.DropColumn(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "entities");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "Core",
                table: "EntityType",
                newName: "LasteModified");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "Core",
                table: "entities",
                newName: "LasteModified");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Core",
                table: "media",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "Core",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LasteModified",
                schema: "Core",
                table: "media",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "media",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Core",
                table: "EntityType",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Core",
                table: "EntityType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "EntityType",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "Core",
                table: "entities",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "Core",
                table: "entities",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LasteModifiedBy",
                schema: "Core",
                table: "entities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Brand",
                columns: new[] { "CreatedAt", "CreatedBy", "LasteModified", "LasteModifiedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Category",
                columns: new[] { "CreatedAt", "CreatedBy", "LasteModified", "LasteModifiedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "EntityType",
                keyColumn: "Id",
                keyValue: "Product",
                columns: new[] { "CreatedAt", "CreatedBy", "LasteModified", "LasteModifiedBy" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "System" });
        }
    }
}
