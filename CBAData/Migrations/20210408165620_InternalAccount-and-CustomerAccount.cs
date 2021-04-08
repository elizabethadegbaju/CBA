using Microsoft.EntityFrameworkCore.Migrations;

namespace CBAData.Migrations
{
    public partial class InternalAccountandCustomerAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountClass",
                table: "GLAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountCode",
                table: "GLAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "GLAccounts",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GLAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GLAccounts_CustomerId",
                table: "GLAccounts",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GLAccounts_Customers_CustomerId",
                table: "GLAccounts",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GLAccounts_Customers_CustomerId",
                table: "GLAccounts");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_GLAccounts_CustomerId",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "AccountClass",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "AccountCode",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "GLAccounts");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "GLAccounts");
        }
    }
}
