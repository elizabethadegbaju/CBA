using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class posting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    Debit = table.Column<float>(type: "real", nullable: true),
                    Credit = table.Column<float>(type: "real", nullable: true),
                    Balance = table.Column<float>(type: "real", nullable: false),
                    PostingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CBAUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GLAccountId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posting_AspNetUsers_CBAUserId",
                        column: x => x.CBAUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posting_GLAccounts_GLAccountId",
                        column: x => x.GLAccountId,
                        principalTable: "GLAccounts",
                        principalColumn: "GLAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posting_CBAUserId",
                table: "Posting",
                column: "CBAUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posting_GLAccountId",
                table: "Posting",
                column: "GLAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posting");
        }
    }
}
