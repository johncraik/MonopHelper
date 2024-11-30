using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Migrations
{
    /// <inheritdoc />
    public partial class TenantedCardModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                columns: new[] { "Id", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Cards");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cards",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "Id");
        }
    }
}
