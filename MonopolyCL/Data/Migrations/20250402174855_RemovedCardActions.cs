using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCardActions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvanceActions");

            migrationBuilder.DropTable(
                name: "CardActions");

            migrationBuilder.DropTable(
                name: "ChoiceActions");

            migrationBuilder.DropTable(
                name: "KeepActions");

            migrationBuilder.DropTable(
                name: "MoveActions");

            migrationBuilder.DropTable(
                name: "PayPlayerActions");

            migrationBuilder.DropTable(
                name: "StreetRepairsActions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdvanceActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdvanceIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimGo = table.Column<bool>(type: "INTEGER", nullable: false),
                    Colour = table.Column<string>(type: "TEXT", nullable: false),
                    Set = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvanceActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardId = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<int>(type: "INTEGER", nullable: false),
                    ActionId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardActions_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChoiceActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardTypeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChoiceActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KeepActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeepActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoveActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsForward = table.Column<bool>(type: "INTEGER", nullable: false),
                    MoveAmount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayPlayerActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AmountToPlayer = table.Column<int>(type: "INTEGER", nullable: false),
                    PayToType = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPlayerActions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StreetRepairsActions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HotelCost = table.Column<int>(type: "INTEGER", nullable: false),
                    HouseCost = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreetRepairsActions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardActions_CardId",
                table: "CardActions",
                column: "CardId");
        }
    }
}
