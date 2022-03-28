using Microsoft.EntityFrameworkCore.Migrations;

namespace FreePasses_API.Migrations
{
    public partial class okok13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Informacao",
                table: "Nucleos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Informacao",
                table: "Nucleos");
        }
    }
}
