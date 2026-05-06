using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class firstone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.CreateTable(
                name: "EntityType",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsMenuable = table.Column<bool>(type: "boolean", nullable: false),
                    AreaName = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RoutingController = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    RoutingAction = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModifiedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "media",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileSize = table.Column<int>(type: "integer", nullable: false),
                    FileName = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Caption = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    MediaType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModifiedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "entities",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Slug = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityTypeId = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModifiedBy = table.Column<string>(type: "text", nullable: false),
                    LasteModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_entities_EntityType_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalSchema: "Core",
                        principalTable: "EntityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_entities_EntityTypeId",
                schema: "Core",
                table: "entities",
                column: "EntityTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entities",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "media",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "EntityType",
                schema: "Core");
        }
    }
}
