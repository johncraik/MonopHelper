using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedMoveAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoveActions",
                table: "MoveActions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MoveActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoveActions",
                table: "MoveActions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MoveActions",
                table: "MoveActions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MoveActions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MoveActions",
                table: "MoveActions",
                columns: new[] { "MoveAmount", "IsForward" });
        }
    }
}
