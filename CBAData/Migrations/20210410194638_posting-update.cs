using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class postingupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posting_AspNetUsers_CBAUserId",
                table: "Posting");

            migrationBuilder.DropForeignKey(
                name: "FK_Posting_GLAccounts_GLAccountId",
                table: "Posting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posting",
                table: "Posting");

            migrationBuilder.RenameTable(
                name: "Posting",
                newName: "Postings");

            migrationBuilder.RenameIndex(
                name: "IX_Posting_GLAccountId",
                table: "Postings",
                newName: "IX_Postings_GLAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Posting_CBAUserId",
                table: "Postings",
                newName: "IX_Postings_CBAUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Postings",
                table: "Postings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Postings_AspNetUsers_CBAUserId",
                table: "Postings",
                column: "CBAUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Postings_GLAccounts_GLAccountId",
                table: "Postings",
                column: "GLAccountId",
                principalTable: "GLAccounts",
                principalColumn: "GLAccountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Postings_AspNetUsers_CBAUserId",
                table: "Postings");

            migrationBuilder.DropForeignKey(
                name: "FK_Postings_GLAccounts_GLAccountId",
                table: "Postings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Postings",
                table: "Postings");

            migrationBuilder.RenameTable(
                name: "Postings",
                newName: "Posting");

            migrationBuilder.RenameIndex(
                name: "IX_Postings_GLAccountId",
                table: "Posting",
                newName: "IX_Posting_GLAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Postings_CBAUserId",
                table: "Posting",
                newName: "IX_Posting_CBAUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posting",
                table: "Posting",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posting_AspNetUsers_CBAUserId",
                table: "Posting",
                column: "CBAUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posting_GLAccounts_GLAccountId",
                table: "Posting",
                column: "GLAccountId",
                principalTable: "GLAccounts",
                principalColumn: "GLAccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
