using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class QuizChangess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOption_Question_QuestionId",
                schema: "dbo",
                table: "AnswerOption");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizHistory_Quiz_QuizId",
                schema: "dbo",
                table: "QuizHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quiz",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerOption",
                schema: "dbo",
                table: "AnswerOption");

            migrationBuilder.RenameTable(
                name: "Quiz",
                schema: "dbo",
                newName: "Quizzes",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Question",
                schema: "dbo",
                newName: "Questions",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AnswerOption",
                schema: "dbo",
                newName: "AnswerOptions",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Question_QuizId",
                schema: "dbo",
                table: "Questions",
                newName: "IX_Questions_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOption_QuestionId",
                schema: "dbo",
                table: "AnswerOptions",
                newName: "IX_AnswerOptions_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quizzes",
                schema: "dbo",
                table: "Quizzes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                schema: "dbo",
                table: "Questions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerOptions",
                schema: "dbo",
                table: "AnswerOptions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                schema: "dbo",
                table: "AnswerOptions",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Quizzes_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                schema: "dbo",
                table: "Questions",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizHistory_Quizzes_QuizId",
                schema: "dbo",
                table: "QuizHistory",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerOptions_Questions_QuestionId",
                schema: "dbo",
                table: "AnswerOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Quizzes_QuizId",
                schema: "dbo",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quizzes_QuizId",
                schema: "dbo",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizHistory_Quizzes_QuizId",
                schema: "dbo",
                table: "QuizHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Quizzes",
                schema: "dbo",
                table: "Quizzes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                schema: "dbo",
                table: "Questions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerOptions",
                schema: "dbo",
                table: "AnswerOptions");

            migrationBuilder.RenameTable(
                name: "Quizzes",
                schema: "dbo",
                newName: "Quiz",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Questions",
                schema: "dbo",
                newName: "Question",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AnswerOptions",
                schema: "dbo",
                newName: "AnswerOption",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_QuizId",
                schema: "dbo",
                table: "Question",
                newName: "IX_Question_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_AnswerOptions_QuestionId",
                schema: "dbo",
                table: "AnswerOption",
                newName: "IX_AnswerOption_QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Quiz",
                schema: "dbo",
                table: "Quiz",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                schema: "dbo",
                table: "Question",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerOption",
                schema: "dbo",
                table: "AnswerOption",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerOption_Question_QuestionId",
                schema: "dbo",
                table: "AnswerOption",
                column: "QuestionId",
                principalSchema: "dbo",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Quiz_QuizId",
                schema: "dbo",
                table: "Feedback",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuizHistory_Quiz_QuizId",
                schema: "dbo",
                table: "QuizHistory",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
