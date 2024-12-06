using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlayerTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GamesToProperties");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "GameProps");

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "GameProps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "GameProps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropertyName",
                table: "GameProps",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => new { x.Name, x.TenantId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_OwnerName_TenantId",
                table: "GameProps",
                columns: new[] { "OwnerName", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_PropertyName_TenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Players_OwnerName_TenantId",
                table: "GameProps",
                columns: new[] { "OwnerName", "TenantId" },
                principalTable: "Players",
                principalColumns: new[] { "Name", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Properties_PropertyName_TenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "TenantId" },
                principalTable: "Properties",
                principalColumns: new[] { "Name", "TenantId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Players_OwnerName_TenantId",
                table: "GameProps");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Properties_PropertyName_TenantId",
                table: "GameProps");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropIndex(
                name: "IX_GameProps_OwnerName_TenantId",
                table: "GameProps");

            migrationBuilder.DropIndex(
                name: "IX_GameProps_PropertyName_TenantId",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "PropertyName",
                table: "GameProps");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "GameProps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GamesToProperties",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: false),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamesToProperties", x => new { x.GameId, x.PropertyName, x.TenantId });
                    table.ForeignKey(
                        name: "FK_GamesToProperties_GameProps_GameId",
                        column: x => x.GameId,
                        principalTable: "GameProps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamesToProperties_Properties_PropertyName_TenantId",
                        columns: x => new { x.PropertyName, x.TenantId },
                        principalTable: "Properties",
                        principalColumns: new[] { "Name", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamesToProperties_PropertyName_TenantId",
                table: "GamesToProperties",
                columns: new[] { "PropertyName", "TenantId" });
        }
    }
}
