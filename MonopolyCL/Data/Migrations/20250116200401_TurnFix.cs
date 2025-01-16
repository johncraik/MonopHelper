using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class TurnFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Order",
                table: "TurnOrders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "TurnOrders");
        }
    }
}
