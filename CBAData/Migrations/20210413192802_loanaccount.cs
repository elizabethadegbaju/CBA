using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class loanaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AmountPaid",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CompoundInterest",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerAccountId",
                table: "GLAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMonths",
                table: "GLAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "InterestRate",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LoanAccount_AccountNumber",
                table: "GLAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Principal",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RepaymentAmountPerTime",
                table: "GLAccounts",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepaymentFrequencyMonths",
                table: "GLAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "GLAccounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts",
                column: "CustomerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_GLAccounts_CustomerAccountId",
                table: "GLAccounts",
                column: "CustomerAccountId",
                principalTable: "GLAccounts",
                principalColumn: "GLAccountId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_GLAccounts_CustomerAccountId",
                table: "GLAccounts");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_CustomerAccountId",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "CompoundInterest",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "CustomerAccountId",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "DurationMonths",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "LoanAccount_AccountNumber",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "Principal",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "RepaymentAmountPerTime",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "RepaymentFrequencyMonths",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "GLAccounts");
        }
    }
}
