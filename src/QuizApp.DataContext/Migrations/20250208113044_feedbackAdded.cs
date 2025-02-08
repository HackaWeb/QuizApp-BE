using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class feedbackAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "dbo",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUsername",
                schema: "dbo",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PassCount",
                schema: "dbo",
                table: "Quiz",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                schema: "dbo",
                table: "Quiz",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "dbo",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Feedback",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedback",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "OwnerUsername",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "PassCount",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Rate",
                schema: "dbo",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "dbo",
                table: "Quiz");
        }
    }
}
