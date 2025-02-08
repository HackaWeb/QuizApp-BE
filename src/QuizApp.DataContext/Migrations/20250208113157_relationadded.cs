using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class relationadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuizId",
                schema: "dbo",
                table: "Feedback",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "QuizId",
                schema: "dbo",
                table: "Feedback");
        }
    }
}
