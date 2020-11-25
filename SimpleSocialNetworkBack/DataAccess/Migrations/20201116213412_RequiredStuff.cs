using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class RequiredStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Messages_Users_PosterLogin",
                "Messages");

            migrationBuilder.AlterColumn<string>(
                "PosterLogin",
                "Messages",
                "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                "FK_Messages_Users_PosterLogin",
                "Messages",
                "PosterLogin",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Messages_Users_PosterLogin",
                "Messages");

            migrationBuilder.AlterColumn<string>(
                "PosterLogin",
                "Messages",
                "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                "FK_Messages_Users_PosterLogin",
                "Messages",
                "PosterLogin",
                "Users",
                principalColumn: "Login",
                onDelete: ReferentialAction.Restrict);
        }
    }
}