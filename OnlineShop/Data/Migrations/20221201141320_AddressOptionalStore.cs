using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.Migrations
{
    public partial class AddressOptionalStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropIndex(
                name: "IX_AddressInfo_UserId",
                table: "AddressInfo");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "AddressInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                unique: true,
                filter: "[StoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_UserId",
                table: "AddressInfo",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropIndex(
                name: "IX_AddressInfo_UserId",
                table: "AddressInfo");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "AddressInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_UserId",
                table: "AddressInfo",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
