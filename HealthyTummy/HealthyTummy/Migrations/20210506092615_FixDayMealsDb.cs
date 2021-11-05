using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyTummy.Migrations
{
    public partial class FixDayMealsDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayMeals_Meals_DayId",
                table: "DayMeals");

            migrationBuilder.CreateIndex(
                name: "IX_DayMeals_MealId",
                table: "DayMeals",
                column: "MealId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayMeals_Meals_MealId",
                table: "DayMeals",
                column: "MealId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayMeals_Meals_MealId",
                table: "DayMeals");

            migrationBuilder.DropIndex(
                name: "IX_DayMeals_MealId",
                table: "DayMeals");

            migrationBuilder.AddForeignKey(
                name: "FK_DayMeals_Meals_DayId",
                table: "DayMeals",
                column: "DayId",
                principalTable: "Meals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
