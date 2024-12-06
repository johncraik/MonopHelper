using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class DbUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TypesToGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CardsToGames",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TypesToGames");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CardsToGames");
        }
    }
}
