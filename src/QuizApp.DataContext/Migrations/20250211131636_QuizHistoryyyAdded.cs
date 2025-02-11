using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class QuizHistoryyyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Score",
                schema: "dbo",
                table: "QuizHistory",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                schema: "dbo",
                table: "QuizHistory");
        }
    }
}
