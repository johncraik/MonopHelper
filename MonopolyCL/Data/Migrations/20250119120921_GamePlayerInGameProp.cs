using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class GamePlayerInGameProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Players_OwnerId",
                table: "GameProps");

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_GamePlayers_OwnerId",
                table: "GameProps",
                column: "OwnerId",
                principalTable: "GamePlayers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_GamePlayers_OwnerId",
                table: "GameProps");

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Players_OwnerId",
                table: "GameProps",
                column: "OwnerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
