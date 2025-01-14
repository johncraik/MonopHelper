using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedGameDM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardGameId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CardGameId",
                table: "Games",
                column: "CardGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_CardGames_CardGameId",
                table: "Games",
                column: "CardGameId",
                principalTable: "CardGames",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_CardGames_CardGameId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_CardGameId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "CardGameId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Games");
        }
    }
}
