using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class Пол : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Clients",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "Clients",
                nullable: false,
                defaultValue: 0);
        }
    }
}
