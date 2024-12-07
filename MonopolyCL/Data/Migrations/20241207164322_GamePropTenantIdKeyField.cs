using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonopolyCL.Data.Migrations
{
    /// <inheritdoc />
    public partial class GamePropTenantIdKeyField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Properties_PropertyName_TenantId",
                table: "GameProps");

            migrationBuilder.DropIndex(
                name: "IX_GameProps_PropertyName_TenantId",
                table: "GameProps");

            migrationBuilder.AddColumn<int>(
                name: "PropertyTenantId",
                table: "GameProps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_PropertyName_PropertyTenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "PropertyTenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Properties_PropertyName_PropertyTenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "PropertyTenantId" },
                principalTable: "Properties",
                principalColumns: new[] { "Name", "TenantId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameProps_Properties_PropertyName_PropertyTenantId",
                table: "GameProps");

            migrationBuilder.DropIndex(
                name: "IX_GameProps_PropertyName_PropertyTenantId",
                table: "GameProps");

            migrationBuilder.DropColumn(
                name: "PropertyTenantId",
                table: "GameProps");

            migrationBuilder.CreateIndex(
                name: "IX_GameProps_PropertyName_TenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "TenantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GameProps_Properties_PropertyName_TenantId",
                table: "GameProps",
                columns: new[] { "PropertyName", "TenantId" },
                principalTable: "Properties",
                principalColumns: new[] { "Name", "TenantId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
