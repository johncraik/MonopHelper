using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class GameTblUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rules",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardId",
                table: "Games",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Boards_BoardId",
                table: "Games",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Boards_BoardId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_BoardId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Rules",
                table: "Games");
        }
    }
}
