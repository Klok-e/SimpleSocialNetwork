using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class NavProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_TagBans_Tags_TagId",
                "TagBans");

            migrationBuilder.DropForeignKey(
                "FK_TagBans_Users_ModeratorId",
                "TagBans");

            migrationBuilder.DropForeignKey(
                "FK_TagBans_Users_UserId",
                "TagBans");

            migrationBuilder.RenameColumn(
                "UserId",
                "TagBans",
                "UserLogin");

            migrationBuilder.RenameColumn(
                "TagId",
                "TagBans",
                "TagName");

            migrationBuilder.RenameColumn(
                "ModeratorId",
                "TagBans",
                "ModeratorLogin");

            migrationBuilder.RenameIndex(
                "IX_TagBans_UserId",
                table: "TagBans",
                newName: "IX_TagBans_UserLogin");

            migrationBuilder.RenameIndex(
                "IX_TagBans_TagId",
                table: "TagBans",
                newName: "IX_TagBans_TagName");

            migrationBuilder.RenameIndex(
                "IX_TagBans_ModeratorId",
                table: "TagBans",
                newName: "IX_TagBans_ModeratorLogin");

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Tags_TagName",
                "TagBans",
                "TagName",
                "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Users_ModeratorLogin",
                "TagBans",
                "ModeratorLogin",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Users_UserLogin",
                "TagBans",
                "UserLogin",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_TagBans_Tags_TagName",
                "TagBans");

            migrationBuilder.DropForeignKey(
                "FK_TagBans_Users_ModeratorLogin",
                "TagBans");

            migrationBuilder.DropForeignKey(
                "FK_TagBans_Users_UserLogin",
                "TagBans");

            migrationBuilder.RenameColumn(
                "UserLogin",
                "TagBans",
                "UserId");

            migrationBuilder.RenameColumn(
                "TagName",
                "TagBans",
                "TagId");

            migrationBuilder.RenameColumn(
                "ModeratorLogin",
                "TagBans",
                "ModeratorId");

            migrationBuilder.RenameIndex(
                "IX_TagBans_UserLogin",
                table: "TagBans",
                newName: "IX_TagBans_UserId");

            migrationBuilder.RenameIndex(
                "IX_TagBans_TagName",
                table: "TagBans",
                newName: "IX_TagBans_TagId");

            migrationBuilder.RenameIndex(
                "IX_TagBans_ModeratorLogin",
                table: "TagBans",
                newName: "IX_TagBans_ModeratorId");

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Tags_TagId",
                "TagBans",
                "TagId",
                "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Users_ModeratorId",
                "TagBans",
                "ModeratorId",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                "FK_TagBans_Users_UserId",
                "TagBans",
                "UserId",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}