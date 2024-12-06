using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class PropertyModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameProps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BuiltLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    IsOwned = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCompleteSet = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMortgaged = table.Column<bool>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: true),
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameProps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    BoardIndex = table.Column<byte>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Cost = table.Column<int>(type: "INTEGER", nullable: false),
                    Set = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => new { x.Name, x.TenantId });
                });

            migrationBuilder.CreateTable(
                name: "BoardsToProperties",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    BoardId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardsToProperties", x => new { x.BoardId, x.PropertyName, x.TenantId });
                    table.ForeignKey(
                        name: "FK_BoardsToProperties_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardsToProperties_Properties_PropertyName_TenantId",
                        columns: x => new { x.PropertyName, x.TenantId },
                        principalTable: "Properties",
                        principalColumns: new[] { "Name", "TenantId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamesToProperties",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyName = table.Column<string>(type: "TEXT", nullable: false),
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
                name: "IX_BoardsToProperties_PropertyName_TenantId",
                table: "BoardsToProperties",
                columns: new[] { "PropertyName", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_GamesToProperties_PropertyName_TenantId",
                table: "GamesToProperties",
                columns: new[] { "PropertyName", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardsToProperties");

            migrationBuilder.DropTable(
                name: "GamesToProperties");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "GameProps");

            migrationBuilder.DropTable(
                name: "Properties");
        }
    }
}
