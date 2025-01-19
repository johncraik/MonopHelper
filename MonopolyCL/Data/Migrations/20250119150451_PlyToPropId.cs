using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlyToPropId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersToProperties",
                table: "PlayersToProperties");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlayersToProperties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersToProperties",
                table: "PlayersToProperties",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersToProperties_GamePlayerId",
                table: "PlayersToProperties",
                column: "GamePlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayersToProperties",
                table: "PlayersToProperties");

            migrationBuilder.DropIndex(
                name: "IX_PlayersToProperties_GamePlayerId",
                table: "PlayersToProperties");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayersToProperties");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayersToProperties",
                table: "PlayersToProperties",
                columns: new[] { "GamePlayerId", "GamePropertyId" });
        }
    }
}
