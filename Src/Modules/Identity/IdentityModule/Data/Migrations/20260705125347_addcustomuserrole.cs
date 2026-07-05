using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityModule.Data.Migrations
{
    /// <inheritdoc />
    public partial class addcustomuserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "identity",
                table: "Vendor",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "Vendor",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                schema: "identity",
                table: "UserGroups",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                schema: "identity",
                table: "AspNetUserRoles",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_AppUserId",
                schema: "identity",
                table: "UserGroups",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_AppUserId",
                schema: "identity",
                table: "UserGroups",
                column: "AppUserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_AppUserId",
                schema: "identity",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_AppUserId",
                schema: "identity",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                schema: "identity",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                schema: "identity",
                table: "AspNetUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "identity",
                table: "Vendor",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "Vendor",
                type: "character varying(450)",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
