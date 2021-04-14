using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class loanaccountaccrual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AccrualBalance",
                table: "GLAccounts",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccrualBalance",
                table: "GLAccounts");
        }
    }
}
