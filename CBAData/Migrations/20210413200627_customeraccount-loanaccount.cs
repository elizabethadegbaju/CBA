using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class customeraccountloanaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts",
                column: "CustomerAccountId",
                unique: true,
                filter: "[CustomerAccountId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts",
                column: "CustomerAccountId");
        }
    }
}
