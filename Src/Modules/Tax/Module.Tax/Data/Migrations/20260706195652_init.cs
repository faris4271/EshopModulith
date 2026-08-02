using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Module.Tax.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaxClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxClass", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxRate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaxClassId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    StateOrProvinceId = table.Column<Guid>(type: "uuid", nullable: true),
                    Rate = table.Column<decimal>(type: "numeric", nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxRate_TaxClass_TaxClassId",
                        column: x => x.TaxClassId,
                        principalTable: "TaxClass",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxRate_TaxClassId",
                table: "TaxRate",
                column: "TaxClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxRate");

            migrationBuilder.DropTable(
                name: "TaxClass");
        }
    }
}
