using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityModule.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "identity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<string>(type: "text", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDead = table.Column<bool>(type: "boolean", nullable: false),
                    LastError = table.Column<string>(type: "text", nullable: true),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }
    }
}
