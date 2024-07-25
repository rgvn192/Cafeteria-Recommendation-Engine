using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedAverageRatingColumnforMenuItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "MenuItems",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "MenuItems");
        }
    }
}
