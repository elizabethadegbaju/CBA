using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class loanaccountiscustomeraccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoanAccount_AccountNumber",
                table: "GLAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LoanAccount_AccountNumber",
                table: "GLAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
