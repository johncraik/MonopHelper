using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Migrations
{
    /// <inheritdoc />
    public partial class CardGameDeckid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeckId",
                table: "CardGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CardGames_DeckId",
                table: "CardGames",
                column: "DeckId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardGames_CardDecks_DeckId",
                table: "CardGames",
                column: "DeckId",
                principalTable: "CardDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardGames_CardDecks_DeckId",
                table: "CardGames");

            migrationBuilder.DropIndex(
                name: "IX_CardGames_DeckId",
                table: "CardGames");

            migrationBuilder.DropColumn(
                name: "DeckId",
                table: "CardGames");
        }
    }
}
