using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class коментарии_к_заявке : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.RenameColumn(
                name: "Commentid",
                table: "OrderComments",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_OrderComments_Commentid",
                table: "OrderComments",
                newName: "IX_OrderComments_Userid");

            migrationBuilder.AddColumn<DateTime>(
                name: "Posted",
                table: "OrderComments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "OrderComments",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Users_Userid",
                table: "OrderComments",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Users_Userid",
                table: "OrderComments");

            migrationBuilder.DropColumn(
                name: "Posted",
                table: "OrderComments");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "OrderComments");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "OrderComments",
                newName: "Commentid");

            migrationBuilder.RenameIndex(
                name: "IX_OrderComments_Userid",
                table: "OrderComments",
                newName: "IX_OrderComments_Commentid");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Posted = table.Column<DateTime>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Userid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_Userid",
                table: "Comments",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments",
                column: "Commentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
