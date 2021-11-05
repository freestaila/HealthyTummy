using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthyTummy.Migrations
{
    public partial class EditDayDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfTheWeek",
                table: "Days");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayOfTheWeek",
                table: "Days",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
