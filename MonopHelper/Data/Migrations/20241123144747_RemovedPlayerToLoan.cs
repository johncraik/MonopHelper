using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopHelper.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPlayerToLoan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerToLoans");

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Loans",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_PlayerId",
                table: "Loans",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Players_PlayerId",
                table: "Loans",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Players_PlayerId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_PlayerId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Loans");

            migrationBuilder.CreateTable(
                name: "PlayerToLoans",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoanId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerToLoans", x => new { x.PlayerId, x.LoanId });
                    table.ForeignKey(
                        name: "FK_PlayerToLoans_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerToLoans_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerToLoans_LoanId",
                table: "PlayerToLoans",
                column: "LoanId");
        }
    }
}
