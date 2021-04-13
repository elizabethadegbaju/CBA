using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class customeraccountdateopened : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOpened",
                table: "GLAccounts",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOpened",
                table: "GLAccounts");
        }
    }
}
