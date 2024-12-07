using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class GameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Money = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    IsInJail = table.Column<bool>(type: "INTEGER", nullable: false),
                    JailCost = table.Column<int>(type: "INTEGER", nullable: true),
                    TripleBonus = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerName = table.Column<string>(type: "TEXT", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Players_PlayerName_TenantId",
                        columns: x => new { x.PlayerName, x.TenantId },
                        principalTable: "Players",
                        principalColumns: new[] { "Name", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPlayed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDiceNumbers",
                columns: table => new
                {
                    GamePlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    DiceOne = table.Column<int>(type: "INTEGER", nullable: false),
                    DiceTwo = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDiceNumbers", x => x.GamePlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerDiceNumbers_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerName_TenantId",
                table: "GamePlayers",
                columns: new[] { "PlayerName", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PlayerDiceNumbers");

            migrationBuilder.DropTable(
                name: "GamePlayers");
        }
    }
}
