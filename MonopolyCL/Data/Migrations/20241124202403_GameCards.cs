using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Migrations
{
    /// <inheritdoc />
    public partial class GameCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardDecks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDecks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: true),
                    CardTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DeckId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_CardDecks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "CardDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_CardTypes_CardTypeId",
                        column: x => x.CardTypeId,
                        principalTable: "CardTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardTypeId",
                table: "Cards",
                column: "CardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_DeckId",
                table: "Cards",
                column: "DeckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "CardDecks");

            migrationBuilder.DropTable(
                name: "CardTypes");
        }
    }
}
