using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AddProductEntityToOrderedProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderedProduct_productId",
                table: "OrderedProduct",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderedProduct_Products_productId",
                table: "OrderedProduct",
                column: "productId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderedProduct_Products_productId",
                table: "OrderedProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderedProduct_productId",
                table: "OrderedProduct");
        }
    }
}
