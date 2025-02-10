using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class QuestionsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question");

            migrationBuilder.AlterColumn<long>(
                name: "Duration",
                schema: "dbo",
                table: "Quiz",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Question",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                schema: "dbo",
                table: "Question",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "Question",
                type: "uuid",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                schema: "dbo",
                table: "Question",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MediaUrl",
                schema: "dbo",
                table: "Question",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Options",
                schema: "dbo",
                table: "Question",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "dbo",
                table: "Question",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question",
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
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "MediaUrl",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Options",
                schema: "dbo",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "dbo",
                table: "Question");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                schema: "dbo",
                table: "Quiz",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                schema: "dbo",
                table: "Question",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<Guid>(
                name: "QuizId",
                schema: "dbo",
                table: "Question",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "dbo",
                table: "Question",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Quiz_QuizId",
                schema: "dbo",
                table: "Question",
                column: "QuizId",
                principalSchema: "dbo",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
