using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyTummy.Migrations
{
    public partial class FIXPRODUCTDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Products_ProdutcId",
                table: "MealProducts");

            migrationBuilder.RenameColumn(
                name: "ProdutcId",
                table: "MealProducts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MealProducts_ProdutcId",
                table: "MealProducts",
                newName: "IX_MealProducts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealProducts_Products_ProductId",
                table: "MealProducts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "MealProducts",
                newName: "ProdutcId");

            migrationBuilder.RenameIndex(
                name: "IX_MealProducts_ProductId",
                table: "MealProducts",
                newName: "IX_MealProducts_ProdutcId");

            migrationBuilder.AddForeignKey(
                name: "FK_MealProducts_Products_ProdutcId",
                table: "MealProducts",
                column: "ProdutcId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
