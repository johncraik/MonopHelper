using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class GamesMarkedAsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Games");
        }
    }
}
