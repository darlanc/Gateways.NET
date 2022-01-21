using Microsoft.EntityFrameworkCore.Migrations;

namespace Gateways.NET.Migrations
{
    public partial class Detachable_Peripherals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GatewayId",
                table: "Peripheral",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GatewayId",
                table: "Peripheral",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
