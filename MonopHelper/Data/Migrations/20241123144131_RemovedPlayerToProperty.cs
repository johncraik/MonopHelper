using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPlayerToProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerToProperties");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Properties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PlayerId",
                table: "Properties",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Players_PlayerId",
                table: "Properties",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Players_PlayerId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PlayerId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Properties");

            migrationBuilder.CreateTable(
                name: "PlayerToProperties",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerToProperties", x => new { x.PlayerId, x.PropertyId });
                    table.ForeignKey(
                        name: "FK_PlayerToProperties_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerToProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToProperties_PropertyId",
                table: "PlayerToProperties",
                column: "PropertyId");
        }
    }
}
