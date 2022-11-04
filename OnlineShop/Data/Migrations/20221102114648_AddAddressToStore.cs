using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Data.Migrations
{
    public partial class AddAddressToStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "AddressInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddressInfo_Stores_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropIndex(
                name: "IX_AddressInfo_StoreId",
                table: "AddressInfo");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "AddressInfo");
        }
    }
}
