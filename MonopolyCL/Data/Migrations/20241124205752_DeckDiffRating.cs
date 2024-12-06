using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Migrations
{
    /// <inheritdoc />
    public partial class DeckDiffRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiffRating",
                table: "CardDecks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiffRating",
                table: "CardDecks");
        }
    }
}
