using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FreePasses_API.Migrations
{
    public partial class okok3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientBuyFreePasses",
                columns: table => new
                {
                    userId = table.Column<string>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Disco = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientBuyFreePasses", x => x.userId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientBuyFreePasses");
        }
    }
}
