using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RequiredStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_PosterLogin",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "PosterLogin",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_PosterLogin",
                table: "Messages",
                column: "PosterLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_PosterLogin",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "PosterLogin",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_PosterLogin",
                table: "Messages",
                column: "PosterLogin",
                principalTable: "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
