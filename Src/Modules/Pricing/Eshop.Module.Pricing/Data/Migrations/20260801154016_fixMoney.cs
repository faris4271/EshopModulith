using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Pricing.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pricing");

            migrationBuilder.CreateTable(
                name: "CartRules",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsCouponRequired = table.Column<bool>(type: "boolean", nullable: false),
                    RuleToApply = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true),
                    DiscountStep = table.Column<int>(type: "integer", nullable: true),
                    UsageLimitPerCoupon = table.Column<int>(type: "integer", nullable: true),
                    UsageLimitPerCustomer = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogRules",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RuleToApply = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRates",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    ToCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Rate = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedById = table.Column<string>(type: "text", nullable: true),
                    LatestUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LatestUpdatedById = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPrices",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    BaseCurrency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    EffectiveFromUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveToUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleCategories",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleCategories_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "pricing",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleCustomerGroups",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleCustomerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleCustomerGroups_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "pricing",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleProducts",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleProducts_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "pricing",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "pricing",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogRuleCustomerGroups",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogRuleCustomerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogRuleCustomerGroups_CatalogRules_CatalogRuleId",
                        column: x => x.CatalogRuleId,
                        principalSchema: "pricing",
                        principalTable: "CatalogRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTierPrices",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductPriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinQuantity = table.Column<int>(type: "integer", nullable: false),
                    DiscountedPrice = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTierPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTierPrices_ProductPrices_ProductPriceId",
                        column: x => x.ProductPriceId,
                        principalSchema: "pricing",
                        principalTable: "ProductPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleUsages",
                schema: "pricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleUsages_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "pricing",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartRuleUsages_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalSchema: "pricing",
                        principalTable: "Coupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleCategories_CartRuleId_CategoryId",
                schema: "pricing",
                table: "CartRuleCategories",
                columns: new[] { "CartRuleId", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleCustomerGroups_CartRuleId_CustomerGroupId",
                schema: "pricing",
                table: "CartRuleCustomerGroups",
                columns: new[] { "CartRuleId", "CustomerGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleProducts_CartRuleId_ProductId",
                schema: "pricing",
                table: "CartRuleProducts",
                columns: new[] { "CartRuleId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleUsages_CartRuleId",
                schema: "pricing",
                table: "CartRuleUsages",
                column: "CartRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleUsages_CouponId",
                schema: "pricing",
                table: "CartRuleUsages",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogRuleCustomerGroups_CatalogRuleId_CustomerGroupId",
                schema: "pricing",
                table: "CatalogRuleCustomerGroups",
                columns: new[] { "CatalogRuleId", "CustomerGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CartRuleId_Code",
                schema: "pricing",
                table: "Coupons",
                columns: new[] { "CartRuleId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId_EffectiveFromUtc_IsActive",
                schema: "pricing",
                table: "ProductPrices",
                columns: new[] { "ProductId", "EffectiveFromUtc", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTierPrices_ProductPriceId_MinQuantity",
                schema: "pricing",
                table: "ProductTierPrices",
                columns: new[] { "ProductPriceId", "MinQuantity" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartRuleCategories",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CartRuleCustomerGroups",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CartRuleProducts",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CartRuleUsages",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CatalogRuleCustomerGroups",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "ExchangeRates",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "ProductTierPrices",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "Coupons",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CatalogRules",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "ProductPrices",
                schema: "pricing");

            migrationBuilder.DropTable(
                name: "CartRules",
                schema: "pricing");
        }
    }
}
