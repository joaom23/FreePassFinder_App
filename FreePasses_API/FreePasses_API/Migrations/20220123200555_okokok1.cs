using Microsoft.EntityFrameworkCore.Migrations;

namespace FreePasses_API.Migrations
{
    public partial class okokok1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userEmail",
                table: "ClientBuyFreePasses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userEmail",
                table: "ClientBuyFreePasses");
        }
    }
}
