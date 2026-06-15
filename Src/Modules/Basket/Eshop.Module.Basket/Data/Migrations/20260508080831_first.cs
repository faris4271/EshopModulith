using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Module.Basket.Data.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "basket");

            migrationBuilder.CreateTable(
                name: "CartItems",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LatestUpdatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<string>(type: "text", nullable: false),
                    VendorId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartRules",
                schema: "basket",
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
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    DiscountStep = table.Column<int>(type: "integer", nullable: true),
                    UsageLimitPerCoupon = table.Column<int>(type: "integer", nullable: true),
                    UsageLimitPerCustomer = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogRule",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    StartOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RuleToApply = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxDiscountAmount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogRule", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleCategory",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleCategory_CartRules_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "basket",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleCustomerGroup",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleCustomerGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleCustomerGroup_CartRules_CartRuleId",
                        column: x => x.CartRuleId,
                        principalSchema: "basket",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleProduct",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleProduct_CartRules_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "basket",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                schema: "basket",
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
                        principalSchema: "basket",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CatalogRuleCustomerGroup",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogRuleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogRuleCustomerGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogRuleCustomerGroup_CatalogRule_CustomerGroupId",
                        column: x => x.CustomerGroupId,
                        principalSchema: "basket",
                        principalTable: "CatalogRule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartRuleUsages",
                schema: "basket",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CartRuleId = table.Column<long>(type: "bigint", nullable: false),
                    CartRuleId1 = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartRuleUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartRuleUsages_CartRules_CartRuleId1",
                        column: x => x.CartRuleId1,
                        principalSchema: "basket",
                        principalTable: "CartRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartRuleUsages_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalSchema: "basket",
                        principalTable: "Coupons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleCategory_CategoryId",
                schema: "basket",
                table: "CartRuleCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleCustomerGroup_CartRuleId",
                schema: "basket",
                table: "CartRuleCustomerGroup",
                column: "CartRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleProduct_ProductId",
                schema: "basket",
                table: "CartRuleProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleUsages_CartRuleId1",
                schema: "basket",
                table: "CartRuleUsages",
                column: "CartRuleId1");

            migrationBuilder.CreateIndex(
                name: "IX_CartRuleUsages_CouponId",
                schema: "basket",
                table: "CartRuleUsages",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogRuleCustomerGroup_CustomerGroupId",
                schema: "basket",
                table: "CatalogRuleCustomerGroup",
                column: "CustomerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_CartRuleId",
                schema: "basket",
                table: "Coupons",
                column: "CartRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CartRuleCategory",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CartRuleCustomerGroup",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CartRuleProduct",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CartRuleUsages",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CatalogRuleCustomerGroup",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "Coupons",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CatalogRule",
                schema: "basket");

            migrationBuilder.DropTable(
                name: "CartRules",
                schema: "basket");
        }
    }
}
