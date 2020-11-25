using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class IdentityToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Tags",
                table => new
                {
                    Name = table.Column<string>("nvarchar(450)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Tags", x => x.Name); });

            migrationBuilder.CreateTable(
                "Users",
                table => new
                {
                    Login = table.Column<string>("nvarchar(450)", nullable: false),
                    About = table.Column<string>("nvarchar(max)", nullable: false),
                    DateBirth = table.Column<DateTime>("datetime2", nullable: true),
                    IsDeleted = table.Column<bool>("bit", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Login); });

            migrationBuilder.CreateTable(
                "OpMessages",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PosterId = table.Column<string>("nvarchar(450)", nullable: true),
                    Title = table.Column<string>("nvarchar(max)", nullable: false),
                    Content = table.Column<string>("nvarchar(max)", nullable: false),
                    Points = table.Column<int>("int", nullable: false),
                    IsDeleted = table.Column<bool>("bit", nullable: false),
                    SendDate = table.Column<DateTime>("datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpMessages", x => x.Id);
                    table.ForeignKey(
                        "FK_OpMessages_Users_PosterId",
                        x => x.PosterId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "SecurePasswords",
                table => new
                {
                    UserId = table.Column<string>("nvarchar(450)", nullable: false),
                    Salt = table.Column<string>("nvarchar(max)", nullable: false),
                    Hashed = table.Column<string>("nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurePasswords", x => x.UserId);
                    table.ForeignKey(
                        "FK_SecurePasswords_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Subscriptions",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberId = table.Column<string>("nvarchar(450)", nullable: true),
                    TargetId = table.Column<string>("nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        "FK_Subscriptions_Users_SubscriberId",
                        x => x.SubscriberId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Subscriptions_Users_TargetId",
                        x => x.TargetId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "TagBans",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagId = table.Column<string>("nvarchar(450)", nullable: true),
                    UserId = table.Column<string>("nvarchar(450)", nullable: true),
                    ModeratorId = table.Column<string>("nvarchar(450)", nullable: true),
                    ExpirationDate = table.Column<DateTime>("datetime2", nullable: false),
                    BanIssuedDate = table.Column<DateTime>("datetime2", nullable: false),
                    Cancelled = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagBans", x => x.Id);
                    table.ForeignKey(
                        "FK_TagBans_Tags_TagId",
                        x => x.TagId,
                        "Tags",
                        "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TagBans_Users_ModeratorId",
                        x => x.ModeratorId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_TagBans_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "TagModerators",
                table => new
                {
                    TagId = table.Column<string>("nvarchar(450)", nullable: false),
                    UserId = table.Column<string>("nvarchar(450)", nullable: false),
                    IsRevoked = table.Column<bool>("bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagModerators", x => new {x.TagId, x.UserId});
                    table.ForeignKey(
                        "FK_TagModerators_Tags_TagId",
                        x => x.TagId,
                        "Tags",
                        "Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_TagModerators_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Messages",
                table => new
                {
                    OpId = table.Column<int>("int", nullable: false),
                    MessageId = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>("nvarchar(max)", nullable: false),
                    Points = table.Column<int>("int", nullable: false),
                    IsDeleted = table.Column<bool>("bit", nullable: false),
                    SendDate = table.Column<DateTime>("datetime2", nullable: false),
                    PosterLogin = table.Column<string>("nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => new {x.OpId, x.MessageId});
                    table.ForeignKey(
                        "FK_Messages_OpMessages_OpId",
                        x => x.OpId,
                        "OpMessages",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Messages_Users_PosterLogin",
                        x => x.PosterLogin,
                        "Users",
                        "Login",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "OpMessageTags",
                table => new
                {
                    TagId = table.Column<string>("nvarchar(450)", nullable: false),
                    OpId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpMessageTags", x => new {x.TagId, x.OpId});
                    table.ForeignKey(
                        "FK_OpMessageTags_OpMessages_OpId",
                        x => x.OpId,
                        "OpMessages",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_OpMessageTags_Tags_TagId",
                        x => x.TagId,
                        "Tags",
                        "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Messages_PosterLogin",
                "Messages",
                "PosterLogin");

            migrationBuilder.CreateIndex(
                "IX_OpMessages_PosterId",
                "OpMessages",
                "PosterId");

            migrationBuilder.CreateIndex(
                "IX_OpMessageTags_OpId",
                "OpMessageTags",
                "OpId");

            migrationBuilder.CreateIndex(
                "IX_Subscriptions_SubscriberId",
                "Subscriptions",
                "SubscriberId");

            migrationBuilder.CreateIndex(
                "IX_Subscriptions_TargetId",
                "Subscriptions",
                "TargetId");

            migrationBuilder.CreateIndex(
                "IX_TagBans_ModeratorId",
                "TagBans",
                "ModeratorId");

            migrationBuilder.CreateIndex(
                "IX_TagBans_TagId",
                "TagBans",
                "TagId");

            migrationBuilder.CreateIndex(
                "IX_TagBans_UserId",
                "TagBans",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_TagModerators_UserId",
                "TagModerators",
                "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Messages");

            migrationBuilder.DropTable(
                "OpMessageTags");

            migrationBuilder.DropTable(
                "SecurePasswords");

            migrationBuilder.DropTable(
                "Subscriptions");

            migrationBuilder.DropTable(
                "TagBans");

            migrationBuilder.DropTable(
                "TagModerators");

            migrationBuilder.DropTable(
                "OpMessages");

            migrationBuilder.DropTable(
                "Tags");

            migrationBuilder.DropTable(
                "Users");
        }
    }
}