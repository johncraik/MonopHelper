using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class PlayerId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_Players_PlayerName_TenantId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Players_OwnerName_TenantId",
                table: "GameProps");

            migrationBuilder.DropIndex(
                name: "IX_GameProps_OwnerName_TenantId",
                table: "GameProps");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_PlayerName_TenantId",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "OwnerName",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "PlayerName",
                table: "GamePlayers");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "GameProps",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "GamePlayers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "GamePlayers");

            migrationBuilder.AddColumn<string>(
                name: "OwnerName",
                table: "GameProps",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlayerName",
                table: "GamePlayers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_OwnerName_TenantId",
                table: "GameProps",
                columns: new[] { "OwnerName", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerName_TenantId",
                table: "GamePlayers",
                columns: new[] { "PlayerName", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_Players_PlayerName_TenantId",
                table: "GamePlayers",
                columns: new[] { "PlayerName", "TenantId" },
                principalTable: "Players",
                principalColumns: new[] { "Name", "TenantId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Players_OwnerName_TenantId",
                table: "GameProps",
                columns: new[] { "OwnerName", "TenantId" },
                principalTable: "Players",
                principalColumns: new[] { "Name", "TenantId" });
        }
    }
}
