using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivitySession_User_UserId1",
                table: "ActivitySession");

            migrationBuilder.DropIndex(
                name: "IX_ActivitySession_UserId1",
                table: "ActivitySession");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ActivitySession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "ActivitySession",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActivitySession_UserId1",
                table: "ActivitySession",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivitySession_User_UserId1",
                table: "ActivitySession",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
