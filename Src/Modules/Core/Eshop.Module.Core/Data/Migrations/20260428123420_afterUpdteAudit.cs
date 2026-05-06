using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class afterUpdteAudit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "EntityType",
                newName: "LatestUpdatedOn");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Core",
                table: "EntityType",
                newName: "LatestUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "Core",
                table: "EntityType",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Core",
                table: "EntityType",
                newName: "CreatedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedOnUtc",
                schema: "Core",
                table: "entities",
                newName: "LatestUpdatedOn");

            migrationBuilder.RenameColumn(
                name: "LastModifiedBy",
                schema: "Core",
                table: "entities",
                newName: "LatestUpdatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedOnUtc",
                schema: "Core",
                table: "entities",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                schema: "Core",
                table: "entities",
                newName: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LatestUpdatedOn",
                schema: "Core",
                table: "EntityType",
                newName: "LastModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "LatestUpdatedById",
                schema: "Core",
                table: "EntityType",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Core",
                table: "EntityType",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                schema: "Core",
                table: "EntityType",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "LatestUpdatedOn",
                schema: "Core",
                table: "entities",
                newName: "LastModifiedOnUtc");

            migrationBuilder.RenameColumn(
                name: "LatestUpdatedById",
                schema: "Core",
                table: "entities",
                newName: "LastModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                schema: "Core",
                table: "entities",
                newName: "CreatedOnUtc");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                schema: "Core",
                table: "entities",
                newName: "CreatedBy");
        }
    }
}
