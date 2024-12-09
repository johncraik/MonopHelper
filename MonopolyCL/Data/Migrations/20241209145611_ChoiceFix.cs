using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChoiceFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChoiceActions_CardTypes_CardTypeId",
                table: "ChoiceActions");

            migrationBuilder.DropIndex(
                name: "IX_ChoiceActions_CardTypeId",
                table: "ChoiceActions");

            migrationBuilder.AddColumn<string>(
                name: "CardTypeName",
                table: "ChoiceActions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardTypeName",
                table: "ChoiceActions");

            migrationBuilder.CreateIndex(
                name: "IX_ChoiceActions_CardTypeId",
                table: "ChoiceActions",
                column: "CardTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoiceActions_CardTypes_CardTypeId",
                table: "ChoiceActions",
                column: "CardTypeId",
                principalTable: "CardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
