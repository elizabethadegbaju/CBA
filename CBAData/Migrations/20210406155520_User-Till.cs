using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class UserTill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_AspNetUsers_UserId",
                table: "GLAccounts");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_UserId",
                table: "GLAccounts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "GLAccounts",
                newName: "CBAUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_CBAUserId",
                table: "GLAccounts",
                column: "CBAUserId",
                unique: true,
                filter: "[CBAUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_AspNetUsers_CBAUserId",
                table: "GLAccounts",
                column: "CBAUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_AspNetUsers_CBAUserId",
                table: "GLAccounts");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_CBAUserId",
                table: "GLAccounts");

            migrationBuilder.RenameColumn(
                name: "CBAUserId",
                table: "GLAccounts",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_UserId",
                table: "GLAccounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_AspNetUsers_UserId",
                table: "GLAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
