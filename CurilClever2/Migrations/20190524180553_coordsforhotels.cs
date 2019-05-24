using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class coordsforhotels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "X",
                table: "Hotels",
                nullable: false,
                defaultValue: 55.76);

            migrationBuilder.AddColumn<double>(
                name: "Y",
                table: "Hotels",
                nullable: false,
                defaultValue: 37.64);

            migrationBuilder.AddColumn<int>(
                name: "Zoom",
                table: "Hotels",
                nullable: false,
                defaultValue: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "Zoom",
                table: "Hotels");
        }
    }
}
