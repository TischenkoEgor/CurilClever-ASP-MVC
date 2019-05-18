using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class newsrollback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "News");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "News",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        Created = table.Column<DateTime>(nullable: false),
            //        ObjectUrl = table.Column<string>(nullable: true),
            //        TextFull = table.Column<string>(nullable: true),
            //        TextShort = table.Column<string>(nullable: true),
            //        Userid = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_News", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_News_Users_Userid",
            //            column: x => x.Userid,
            //            principalTable: "Users",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_News_Userid",
            //    table: "News",
            //    column: "Userid");
        }
    }
}
