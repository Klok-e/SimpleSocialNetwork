using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class OpMessageNavProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpMessages_Users_PosterId",
                table: "OpMessages");

            migrationBuilder.RenameColumn(
                name: "PosterId",
                table: "OpMessages",
                newName: "PosterLogin");

            migrationBuilder.RenameIndex(
                name: "IX_OpMessages_PosterId",
                table: "OpMessages",
                newName: "IX_OpMessages_PosterLogin");

            migrationBuilder.AddForeignKey(
                name: "FK_OpMessages_Users_PosterLogin",
                table: "OpMessages",
                column: "PosterLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OpMessages_Users_PosterLogin",
                table: "OpMessages");

            migrationBuilder.RenameColumn(
                name: "PosterLogin",
                table: "OpMessages",
                newName: "PosterId");

            migrationBuilder.RenameIndex(
                name: "IX_OpMessages_PosterLogin",
                table: "OpMessages",
                newName: "IX_OpMessages_PosterId");

            migrationBuilder.AddForeignKey(
                name: "FK_OpMessages_Users_PosterId",
                table: "OpMessages",
                column: "PosterId",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
