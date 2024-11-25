using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Migrations
{
    /// <inheritdoc />
    public partial class IsDeletedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CardTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CardDecks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CardTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CardDecks");
        }
    }
}
