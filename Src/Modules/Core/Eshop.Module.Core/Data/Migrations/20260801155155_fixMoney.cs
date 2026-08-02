using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code3 = table.Column<string>(type: "text", nullable: false),
                    IsBillingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsShippingEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsCityEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsZipCodeEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    IsDistrictEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatesOrProvinces",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatesOrProvinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatesOrProvinces_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Core",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    StateOrProvinceId = table.Column<Guid>(type: "uuid", maxLength: 10, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    LatestUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LatestUpdatedById = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_StatesOrProvinces_StateOrProvinceId",
                        column: x => x.StateOrProvinceId,
                        principalSchema: "Core",
                        principalTable: "StatesOrProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AddressLine1 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    AddressLine2 = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", maxLength: 10, nullable: false),
                    StateOrProvinceId = table.Column<Guid>(type: "uuid", maxLength: 10, nullable: false),
                    DistrictId = table.Column<Guid>(type: "uuid", maxLength: 10, nullable: true),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    LatestUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LatestUpdatedById = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Core",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: "Core",
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Addresses_StatesOrProvinces_StateOrProvinceId",
                        column: x => x.StateOrProvinceId,
                        principalSchema: "Core",
                        principalTable: "StatesOrProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityName",
                schema: "Core",
                table: "Addresses",
                column: "CityName");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                schema: "Core",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DistrictId",
                schema: "Core",
                table: "Addresses",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StateOrProvinceId",
                schema: "Core",
                table: "Addresses",
                column: "StateOrProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ZipCode",
                schema: "Core",
                table: "Addresses",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_StateOrProvinceId",
                schema: "Core",
                table: "Districts",
                column: "StateOrProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_StatesOrProvinces_CountryId",
                schema: "Core",
                table: "StatesOrProvinces",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Districts",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "StatesOrProvinces",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Core");
        }
    }
}
