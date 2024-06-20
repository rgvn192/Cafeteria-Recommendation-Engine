using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedMenuItemTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItemCategorys",
                columns: table => new
                {
                    MenuItemCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategorys", x => x.MenuItemCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(60)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comments = table.Column<string>(type: "varchar(70)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MenuItemCategoryId = table.Column<int>(type: "int", nullable: false),
                    UserLikeability = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItemCategorys_MenuItemCategoryId",
                        column: x => x.MenuItemCategoryId,
                        principalTable: "MenuItemCategorys",
                        principalColumn: "MenuItemCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MenuItemCategorys",
                columns: new[] { "MenuItemCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Snack" },
                    { 2, "Main Course" },
                    { 3, "Breads" },
                    { 4, "Beverages" },
                    { 5, "Side Dish" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "Comments", "MenuItemCategoryId", "Name", "Price", "UserLikeability" },
                values: new object[,]
                {
                    { 1, null, 1, "Poha", 40.00m, 0m },
                    { 2, null, 1, "Upma", 40.00m, 0m },
                    { 3, null, 1, "Fried Idli", 40.00m, 0m },
                    { 4, null, 2, "Moong Daal", 40.00m, 0m },
                    { 5, null, 2, "Paneer lababdar", 40.00m, 0m },
                    { 6, null, 2, "Vegetable Biryani", 100.00m, 0m },
                    { 7, null, 2, "Palak Paneer", 90.00m, 0m },
                    { 8, null, 2, "Mix Veg", 90.00m, 0m },
                    { 9, null, 3, "Naan", 15.00m, 0m },
                    { 10, null, 3, "Roti", 10.00m, 0m },
                    { 12, null, 3, "Paratha", 20.00m, 0m },
                    { 13, null, 4, "Masala Chai", 15.00m, 0m },
                    { 14, null, 4, "Mango Lassi", 25.00m, 0m },
                    { 15, null, 5, "Cucumber Raita", 20.00m, 0m },
                    { 16, null, 5, "Mixed Vegetable Salad", 30.00m, 0m },
                    { 17, null, 5, "Steamed Basmati Rice", 25.00m, 0m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCategorys_Name",
                table: "MenuItemCategorys",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuItemCategoryId",
                table: "MenuItems",
                column: "MenuItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_Name",
                table: "MenuItems",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemCategorys");
        }
    }
}
