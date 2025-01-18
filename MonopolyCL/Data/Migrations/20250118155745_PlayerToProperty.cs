using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlayerToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersToCards_GameCards_CardId",
                table: "PlayersToCards");

            migrationBuilder.DropTable(
                name: "GameCards");

            migrationBuilder.DropTable(
                name: "GameCardTypes");

            migrationBuilder.CreateTable(
                name: "PlayersToProperties",
                columns: table => new
                {
                    GamePlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    GamePropertyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersToProperties", x => new { x.GamePlayerId, x.GamePropertyId });
                    table.ForeignKey(
                        name: "FK_PlayersToProperties_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayersToProperties_GameProps_GamePropertyId",
                        column: x => x.GamePropertyId,
                        principalTable: "GameProps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayersToProperties_GamePropertyId",
                table: "PlayersToProperties",
                column: "GamePropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersToCards_Cards_CardId",
                table: "PlayersToCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayersToCards_Cards_CardId",
                table: "PlayersToCards");

            migrationBuilder.DropTable(
                name: "PlayersToProperties");

            migrationBuilder.CreateTable(
                name: "GameCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Index = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameCardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentIndex = table.Column<uint>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCardTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameCardTypes_CardTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameCards_CardId",
                table: "GameCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameCardTypes_TypeId",
                table: "GameCardTypes",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayersToCards_GameCards_CardId",
                table: "PlayersToCards",
                column: "CardId",
                principalTable: "GameCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
