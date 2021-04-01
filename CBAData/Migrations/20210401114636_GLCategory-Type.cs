using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class GLCategoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "GLCategories",
                newName: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "GLCategories",
                newName: "AccountType");
        }
    }
}
