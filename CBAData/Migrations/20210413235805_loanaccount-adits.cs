using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class loanaccountadits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "DurationMonths",
                table: "GLAccounts");

            migrationBuilder.AlterColumn<float>(
                name: "RepaymentFrequencyMonths",
                table: "GLAccounts",
                type: "real",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "DurationYears",
                table: "GLAccounts",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationYears",
                table: "GLAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "RepaymentFrequencyMonths",
                table: "GLAccounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMonths",
                table: "GLAccounts",
                type: "int",
                nullable: true);
        }
    }
}
