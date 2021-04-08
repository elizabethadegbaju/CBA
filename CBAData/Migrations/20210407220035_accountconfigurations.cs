using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class accountconfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavingsInterestRate = table.Column<float>(type: "real", nullable: false),
                    LoanInterestRate = table.Column<float>(type: "real", nullable: false),
                    SavingsMinBalance = table.Column<float>(type: "real", nullable: false),
                    CurrentMinBalance = table.Column<float>(type: "real", nullable: false),
                    SavingsMaxDailyWithdrawal = table.Column<float>(type: "real", nullable: false),
                    CurrentMaxDailyWithdrawal = table.Column<float>(type: "real", nullable: false),
                    FinancialDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountConfigurations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountConfigurations");
        }
    }
}
