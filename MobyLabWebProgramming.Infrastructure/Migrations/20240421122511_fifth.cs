using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    public partial class fifth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkoutSubscription",
                table: "UserWorkoutSubscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkoutSubscription",
                table: "UserWorkoutSubscription",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserWorkoutSubscription_UserId",
                table: "UserWorkoutSubscription",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserWorkoutSubscription",
                table: "UserWorkoutSubscription");

            migrationBuilder.DropIndex(
                name: "IX_UserWorkoutSubscription_UserId",
                table: "UserWorkoutSubscription");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserWorkoutSubscription",
                table: "UserWorkoutSubscription",
                columns: new[] { "UserId", "WorkoutProgramId" });
        }
    }
}
