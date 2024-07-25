using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedMoreMenuItemSeeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.5m, 4.7m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.3m, 4.6m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.0m, 4.2m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.4m, 4.5m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.7m, 4.8m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.8m, 4.9m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 7,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.5m, 4.6m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 8,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.3m, 4.4m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 9,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.6m, 4.7m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 10,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.4m, 4.5m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 12,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.7m, 4.8m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 13,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.8m, 4.9m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 14,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.6m, 4.7m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 15,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.5m, 4.6m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 16,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.7m, 4.8m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 17,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 4.3m, 4.4m });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "AverageRating", "Comments", "MenuItemCategoryId", "Name", "Price", "UserLikeability" },
                values: new object[,]
                {
                    { 18, 2.5m, null, 2, "Aloo Gobi", 50.00m, 2.7m },
                    { 19, 2.8m, null, 2, "Kadhi Pakoda", 60.00m, 3.0m },
                    { 20, 3.0m, null, 3, "Butter Naan", 25.00m, 3.2m },
                    { 21, 3.3m, null, 3, "Plain Paratha", 15.00m, 3.5m },
                    { 22, 3.5m, null, 5, "Plain Curd", 10.00m, 3.6m },
                    { 23, 3.2m, null, 5, "Green Salad", 20.00m, 3.4m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 23);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 7,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 8,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 9,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 10,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 12,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 13,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 14,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 15,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 16,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 17,
                columns: new[] { "AverageRating", "UserLikeability" },
                values: new object[] { 0m, 0m });
        }
    }
}
