using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class QuizHistoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerUsername",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "dbo",
                table: "Quiz",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "QuizHistory",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizHistory_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalSchema: "dbo",
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizHistory_QuizId",
                schema: "dbo",
                table: "QuizHistory",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizHistory",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUsername",
                schema: "dbo",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
