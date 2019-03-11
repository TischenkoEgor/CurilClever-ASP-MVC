using Microsoft.EntityFrameworkCore.Migrations;

namespace CurilClever2.Migrations
{
    public partial class исправление_навигации : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientComments_Clients_ClientId",
                table: "ClientComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Orders_OrderId",
                table: "OrderComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Hotels_Hotelid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "HoteId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Orders",
                newName: "Clientid");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ClientId",
                table: "Orders",
                newName: "IX_Orders_Clientid");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "OrderComments",
                newName: "Orderid");

            migrationBuilder.RenameColumn(
                name: "CommetId",
                table: "OrderComments",
                newName: "Commetid");

            migrationBuilder.RenameIndex(
                name: "IX_OrderComments_OrderId",
                table: "OrderComments",
                newName: "IX_OrderComments_Orderid");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Comments",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                newName: "IX_Comments_Userid");

            migrationBuilder.RenameColumn(
                name: "CommetId",
                table: "ClientComments",
                newName: "Commetid");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientComments",
                newName: "Clientid");

            migrationBuilder.RenameIndex(
                name: "IX_ClientComments_ClientId",
                table: "ClientComments",
                newName: "IX_ClientComments_Clientid");

            migrationBuilder.AlterColumn<int>(
                name: "Hotelid",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientComments_Clients_Clientid",
                table: "ClientComments",
                column: "Clientid",
                principalTable: "Clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_Userid",
                table: "Comments",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Orders_Orderid",
                table: "OrderComments",
                column: "Orderid",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_Clientid",
                table: "Orders",
                column: "Clientid",
                principalTable: "Clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Hotels_Hotelid",
                table: "Orders",
                column: "Hotelid",
                principalTable: "Hotels",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientComments_Clients_Clientid",
                table: "ClientComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_Userid",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderComments_Orders_Orderid",
                table: "OrderComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Clients_Clientid",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Hotels_Hotelid",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Clientid",
                table: "Orders",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Clientid",
                table: "Orders",
                newName: "IX_Orders_ClientId");

            migrationBuilder.RenameColumn(
                name: "Orderid",
                table: "OrderComments",
                newName: "OrderId");

            migrationBuilder.RenameColumn(
                name: "Commetid",
                table: "OrderComments",
                newName: "CommetId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderComments_Orderid",
                table: "OrderComments",
                newName: "IX_OrderComments_OrderId");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Comments",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Userid",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameColumn(
                name: "Commetid",
                table: "ClientComments",
                newName: "CommetId");

            migrationBuilder.RenameColumn(
                name: "Clientid",
                table: "ClientComments",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientComments_Clientid",
                table: "ClientComments",
                newName: "IX_ClientComments_ClientId");

            migrationBuilder.AlterColumn<int>(
                name: "Hotelid",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "HoteId",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientComments_Clients_ClientId",
                table: "ClientComments",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderComments_Orders_OrderId",
                table: "OrderComments",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Clients_ClientId",
                table: "Orders",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Hotels_Hotelid",
                table: "Orders",
                column: "Hotelid",
                principalTable: "Hotels",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
