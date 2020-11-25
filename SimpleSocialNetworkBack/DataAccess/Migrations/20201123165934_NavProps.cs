using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class NavProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Tags_TagId",
                table: "TagBans");

            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Users_ModeratorId",
                table: "TagBans");

            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Users_UserId",
                table: "TagBans");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "TagBans",
                newName: "UserLogin");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "TagBans",
                newName: "TagName");

            migrationBuilder.RenameColumn(
                name: "ModeratorId",
                table: "TagBans",
                newName: "ModeratorLogin");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_UserId",
                table: "TagBans",
                newName: "IX_TagBans_UserLogin");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_TagId",
                table: "TagBans",
                newName: "IX_TagBans_TagName");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_ModeratorId",
                table: "TagBans",
                newName: "IX_TagBans_ModeratorLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Tags_TagName",
                table: "TagBans",
                column: "TagName",
                principalTable: "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Users_ModeratorLogin",
                table: "TagBans",
                column: "ModeratorLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Users_UserLogin",
                table: "TagBans",
                column: "UserLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Tags_TagName",
                table: "TagBans");

            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Users_ModeratorLogin",
                table: "TagBans");

            migrationBuilder.DropForeignKey(
                name: "FK_TagBans_Users_UserLogin",
                table: "TagBans");

            migrationBuilder.RenameColumn(
                name: "UserLogin",
                table: "TagBans",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TagName",
                table: "TagBans",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "ModeratorLogin",
                table: "TagBans",
                newName: "ModeratorId");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_UserLogin",
                table: "TagBans",
                newName: "IX_TagBans_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_TagName",
                table: "TagBans",
                newName: "IX_TagBans_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagBans_ModeratorLogin",
                table: "TagBans",
                newName: "IX_TagBans_ModeratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Tags_TagId",
                table: "TagBans",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Users_ModeratorId",
                table: "TagBans",
                column: "ModeratorId",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagBans_Users_UserId",
                table: "TagBans",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
