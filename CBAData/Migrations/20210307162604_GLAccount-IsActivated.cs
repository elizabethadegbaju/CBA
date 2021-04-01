using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class GLAccountIsActivated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActivated",
                table: "GLAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActivated",
                table: "GLAccounts");
        }
    }
}
