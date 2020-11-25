using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class OpMessageNavProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_OpMessages_Users_PosterId",
                "OpMessages");

            migrationBuilder.RenameColumn(
                "PosterId",
                "OpMessages",
                "PosterLogin");

            migrationBuilder.RenameIndex(
                "IX_OpMessages_PosterId",
                table: "OpMessages",
                newName: "IX_OpMessages_PosterLogin");

            migrationBuilder.AddForeignKey(
                "FK_OpMessages_Users_PosterLogin",
                "OpMessages",
                "PosterLogin",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_OpMessages_Users_PosterLogin",
                "OpMessages");

            migrationBuilder.RenameColumn(
                "PosterLogin",
                "OpMessages",
                "PosterId");

            migrationBuilder.RenameIndex(
                "IX_OpMessages_PosterLogin",
                table: "OpMessages",
                newName: "IX_OpMessages_PosterId");

            migrationBuilder.AddForeignKey(
                "FK_OpMessages_Users_PosterId",
                "OpMessages",
                "PosterId",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}