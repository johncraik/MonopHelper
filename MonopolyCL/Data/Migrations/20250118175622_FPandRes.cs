using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class FPandRes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInFreeParking",
                table: "PlayersToProperties",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "PlayersToProperties",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInFreeParking",
                table: "PlayersToProperties");

            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "PlayersToProperties");
        }
    }
}
