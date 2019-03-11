using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class исправление_навигации2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientComments_Comments_Commentid",
                table: "ClientComments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments");

            migrationBuilder.DropColumn(
                name: "Commetid",
                table: "OrderComments");

            migrationBuilder.DropColumn(
                name: "Commetid",
                table: "ClientComments");

            migrationBuilder.AlterColumn<int>(
                name: "Commentid",
                table: "OrderComments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Commentid",
                table: "ClientComments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientComments_Comments_Commentid",
                table: "ClientComments",
                column: "Commentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments",
                column: "Commentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientComments_Comments_Commentid",
                table: "ClientComments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments");

            migrationBuilder.AlterColumn<int>(
                name: "Commentid",
                table: "OrderComments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Commetid",
                table: "OrderComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Commentid",
                table: "ClientComments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Commetid",
                table: "ClientComments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientComments_Comments_Commentid",
                table: "ClientComments",
                column: "Commentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Comments_Commentid",
                table: "OrderComments",
                column: "Commentid",
                principalTable: "Comments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
