using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityModule.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAddress_DefaultBillingAddressId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAddress_DefaultShippingAddressId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_AspNetUsers_UserId",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_City",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_Phone",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_State",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Address_ZipCode",
                schema: "identity",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                schema: "identity",
                newName: "UserAddresses",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_UserId",
                schema: "identity",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddresses",
                schema: "identity",
                table: "UserAddresses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddresses_AddressId",
                schema: "identity",
                table: "UserAddresses",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAddresses_DefaultBillingAddressId",
                schema: "identity",
                table: "AspNetUsers",
                column: "DefaultBillingAddressId",
                principalSchema: "identity",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAddresses_DefaultShippingAddressId",
                schema: "identity",
                table: "AspNetUsers",
                column: "DefaultShippingAddressId",
                principalSchema: "identity",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                schema: "identity",
                table: "UserAddresses",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAddresses_DefaultBillingAddressId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAddresses_DefaultShippingAddressId",
                schema: "identity",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_AspNetUsers_UserId",
                schema: "identity",
                table: "UserAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddresses",
                schema: "identity",
                table: "UserAddresses");

            migrationBuilder.DropIndex(
                name: "IX_UserAddresses_AddressId",
                schema: "identity",
                table: "UserAddresses");

            migrationBuilder.RenameTable(
                name: "UserAddresses",
                schema: "identity",
                newName: "UserAddress",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                schema: "identity",
                table: "UserAddress",
                newName: "IX_UserAddress_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Phone",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_State",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address_ZipCode",
                schema: "identity",
                table: "UserAddress",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                schema: "identity",
                table: "UserAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAddress_DefaultBillingAddressId",
                schema: "identity",
                table: "AspNetUsers",
                column: "DefaultBillingAddressId",
                principalSchema: "identity",
                principalTable: "UserAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAddress_DefaultShippingAddressId",
                schema: "identity",
                table: "AspNetUsers",
                column: "DefaultShippingAddressId",
                principalSchema: "identity",
                principalTable: "UserAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_AspNetUsers_UserId",
                schema: "identity",
                table: "UserAddress",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
