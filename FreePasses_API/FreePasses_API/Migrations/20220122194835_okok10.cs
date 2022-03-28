using Microsoft.EntityFrameworkCore.Migrations;

namespace FreePasses_API.Migrations
{
    public partial class okok10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientBuyFreePasses",
                table: "ClientBuyFreePasses");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "ClientBuyFreePasses",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "IdCompra",
                table: "ClientBuyFreePasses",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientBuyFreePasses",
                table: "ClientBuyFreePasses",
                column: "IdCompra");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ClientBuyFreePasses",
                table: "ClientBuyFreePasses");

            migrationBuilder.DropColumn(
                name: "IdCompra",
                table: "ClientBuyFreePasses");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "ClientBuyFreePasses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClientBuyFreePasses",
                table: "ClientBuyFreePasses",
                column: "userId");
        }
    }
}
