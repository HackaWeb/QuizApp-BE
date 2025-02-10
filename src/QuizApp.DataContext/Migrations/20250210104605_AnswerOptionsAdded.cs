using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class AnswerOptionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Options",
                schema: "dbo",
                table: "Question");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                schema: "dbo",
                table: "Feedback",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AnswerOption",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOption_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "dbo",
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOption_QuestionId",
                schema: "dbo",
                table: "AnswerOption",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropTable(
                name: "AnswerOption",
                schema: "dbo");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                schema: "dbo",
                table: "Question",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                schema: "dbo",
                table: "Question",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                schema: "dbo",
                table: "Feedback",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
