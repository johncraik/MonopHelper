using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdvActionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvanceActions",
                table: "AdvanceActions");

            migrationBuilder.AlterColumn<int>(
                name: "AdvanceIndex",
                table: "AdvanceActions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AdvanceActions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvanceActions",
                table: "AdvanceActions",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AdvanceActions",
                table: "AdvanceActions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AdvanceActions");

            migrationBuilder.AlterColumn<int>(
                name: "AdvanceIndex",
                table: "AdvanceActions",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdvanceActions",
                table: "AdvanceActions",
                column: "AdvanceIndex");
        }
    }
}
