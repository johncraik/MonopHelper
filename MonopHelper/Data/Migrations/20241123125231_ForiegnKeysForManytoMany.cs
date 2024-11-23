using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class ForiegnKeysForManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PlayerToProperties_PropertyId",
                table: "PlayerToProperties",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToLoans_LoanId",
                table: "PlayerToLoans",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerToLoans_Loans_LoanId",
                table: "PlayerToLoans",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerToLoans_Players_PlayerId",
                table: "PlayerToLoans",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerToProperties_Players_PlayerId",
                table: "PlayerToProperties",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerToProperties_Properties_PropertyId",
                table: "PlayerToProperties",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerToLoans_Loans_LoanId",
                table: "PlayerToLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerToLoans_Players_PlayerId",
                table: "PlayerToLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerToProperties_Players_PlayerId",
                table: "PlayerToProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerToProperties_Properties_PropertyId",
                table: "PlayerToProperties");

            migrationBuilder.DropIndex(
                name: "IX_PlayerToProperties_PropertyId",
                table: "PlayerToProperties");

            migrationBuilder.DropIndex(
                name: "IX_PlayerToLoans_LoanId",
                table: "PlayerToLoans");
        }
    }
}
