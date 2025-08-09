using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardDecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiffRating = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wins = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BoardIndex = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Set = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => new { x.Name, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "TurnOrders",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSetup = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentTurn = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurnOrders", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "CardGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPlayed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardGames_CardDecks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "CardDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CardTypeId = table.Column<int>(type: "int", nullable: false),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_CardDecks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "CardDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_CardTypes_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: false),
                    BoardIndex = table.Column<byte>(type: "tinyint", nullable: false),
                    IsInJail = table.Column<bool>(type: "bit", nullable: false),
                    IsBankrupt = table.Column<bool>(type: "bit", nullable: false),
                    JailCost = table.Column<int>(type: "int", nullable: true),
                    TripleBonus = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardsToProperties",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardsToProperties", x => new { x.BoardId, x.PropertyName, x.TenantId });
                    table.ForeignKey(
                        name: "FK_BoardsToProperties_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardsToProperties_Properties_PropertyName_TenantId",
                        columns: x => new { x.PropertyName, x.TenantId },
                        principalTable: "Properties",
                        principalColumns: new[] { "Name", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rules = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastPlayed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardGameId = table.Column<int>(type: "int", nullable: false),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_CardGames_CardGameId",
                        column: x => x.CardGameId,
                        principalTable: "CardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TypesToGames",
                columns: table => new
                {
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CurrentIndex = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesToGames", x => new { x.TypeId, x.GameId });
                    table.ForeignKey(
                        name: "FK_TypesToGames_CardGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TypesToGames_CardTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardActionConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsKeep = table.Column<bool>(type: "bit", nullable: false),
                    PlayCondition = table.Column<int>(type: "int", nullable: false),
                    Actions = table.Column<int>(type: "int", nullable: false),
                    Groups = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardActionConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardActionConfigs_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardsToGames",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardsToGames", x => new { x.CardId, x.GameId });
                    table.ForeignKey(
                        name: "FK_CardsToGames_CardGames_GameId",
                        column: x => x.GameId,
                        principalTable: "CardGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardsToGames_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameProps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuiltLevel = table.Column<int>(type: "int", nullable: true),
                    IsOwned = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleteSet = table.Column<bool>(type: "bit", nullable: false),
                    IsMortgaged = table.Column<bool>(type: "bit", nullable: false),
                    PropertyName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PropertyTenantId = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameProps_GamePlayers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameProps_Properties_PropertyName_PropertyTenantId",
                        columns: x => new { x.PropertyName, x.PropertyTenantId },
                        principalTable: "Properties",
                        principalColumns: new[] { "Name", "TenantId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDiceNumbers",
                columns: table => new
                {
                    GamePlayerId = table.Column<int>(type: "int", nullable: false),
                    DiceOne = table.Column<int>(type: "int", nullable: false),
                    DiceTwo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDiceNumbers", x => x.GamePlayerId);
                    table.ForeignKey(
                        name: "FK_PlayerDiceNumbers_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerLoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    RepaidAmount = table.Column<int>(type: "int", nullable: false),
                    Repaid = table.Column<bool>(type: "bit", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerLoans_GamePlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayersToCards",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersToCards", x => new { x.PlayerId, x.CardId });
                    table.ForeignKey(
                        name: "FK_PlayersToCards_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayersToCards_GamePlayers_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayersToProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GamePlayerId = table.Column<int>(type: "int", nullable: false),
                    GamePropertyId = table.Column<int>(type: "int", nullable: false),
                    IsInFreeParking = table.Column<bool>(type: "bit", nullable: false),
                    IsReserved = table.Column<bool>(type: "bit", nullable: false),
                    ReservedAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayersToProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayersToProperties_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayersToProperties_GameProps_GamePropertyId",
                        column: x => x.GamePropertyId,
                        principalTable: "GameProps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardsToProperties_PropertyName_TenantId",
                table: "BoardsToProperties",
                columns: new[] { "PropertyName", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_CardActionConfigs_CardId",
                table: "CardActionConfigs",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardGames_DeckId",
                table: "CardGames",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeId",
                table: "Cards",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId",
                table: "Cards",
                column: "DeckId");

            migrationBuilder.CreateIndex(
                name: "IX_CardsToGames_GameId",
                table: "CardsToGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerId",
                table: "GamePlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_OwnerId",
                table: "GameProps",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_PropertyName_PropertyTenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "PropertyTenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Games_BoardId",
                table: "Games",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CardGameId",
                table: "Games",
                column: "CardGameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerLoans_PlayerId",
                table: "PlayerLoans",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersToCards_CardId",
                table: "PlayersToCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersToProperties_GamePlayerId",
                table: "PlayersToProperties",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayersToProperties_GamePropertyId",
                table: "PlayersToProperties",
                column: "GamePropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_TypesToGames_GameId",
                table: "TypesToGames",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardsToProperties");

            migrationBuilder.DropTable(
                name: "CardActionConfigs");

            migrationBuilder.DropTable(
                name: "CardsToGames");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PlayerDiceNumbers");

            migrationBuilder.DropTable(
                name: "PlayerLoans");

            migrationBuilder.DropTable(
                name: "PlayersToCards");

            migrationBuilder.DropTable(
                name: "PlayersToProperties");

            migrationBuilder.DropTable(
                name: "TurnOrders");

            migrationBuilder.DropTable(
                name: "TypesToGames");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "GameProps");

            migrationBuilder.DropTable(
                name: "CardGames");

            migrationBuilder.DropTable(
                name: "CardTypes");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "CardDecks");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
