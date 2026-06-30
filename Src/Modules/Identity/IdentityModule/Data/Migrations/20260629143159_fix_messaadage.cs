using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IdentityModule.Data.Migrations
{
    /// <inheritdoc />
    public partial class fix_messaadage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "InboxState",
                schema: "identity");

            migrationBuilder.DropTable(
                name: "OutboxState",
                schema: "identity");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    CorrelationId = table.Column<string>(type: "text", nullable: false),
                    ProcessedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    LastError = table.Column<string>(type: "text", nullable: true),
                    IsDead = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "identity");

            migrationBuilder.CreateTable(
                name: "InboxState",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxState",
                schema: "identity",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    BusName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxState", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "identity",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalSchema: "identity",
                        principalTable: "InboxState",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxState_OutboxId",
                        column: x => x.OutboxId,
                        principalSchema: "identity",
                        principalTable: "OutboxState",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                schema: "identity",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                schema: "identity",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                schema: "identity",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                schema: "identity",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                schema: "identity",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_BusName_Created",
                schema: "identity",
                table: "OutboxState",
                columns: new[] { "BusName", "Created" });

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_Created",
                schema: "identity",
                table: "OutboxState",
                column: "Created");
        }
    }
}
