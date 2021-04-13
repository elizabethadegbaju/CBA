using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class internalaccountglcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_GLCategories_GLCategoryId",
                table: "GLAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "GLCategoryId",
                table: "GLAccounts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_GLCategories_GLCategoryId",
                table: "GLAccounts",
                column: "GLCategoryId",
                principalTable: "GLCategories",
                principalColumn: "GLCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_GLCategories_GLCategoryId",
                table: "GLAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "GLCategoryId",
                table: "GLAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_GLCategories_GLCategoryId",
                table: "GLAccounts",
                column: "GLCategoryId",
                principalTable: "GLCategories",
                principalColumn: "GLCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
