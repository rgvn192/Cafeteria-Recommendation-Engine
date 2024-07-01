using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedCharacteristicsandpreferencetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    CharacteristicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.CharacteristicId);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemCharacteristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    CharacteristicId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCharacteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItemCharacteristics_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristics",
                        principalColumn: "CharacteristicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemCharacteristics_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    UserPreferenceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CharacteristicId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.UserPreferenceId);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Characteristics_CharacteristicId",
                        column: x => x.CharacteristicId,
                        principalTable: "Characteristics",
                        principalColumn: "CharacteristicId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Characteristics",
                columns: new[] { "CharacteristicId", "Name" },
                values: new object[,]
                {
                    { 1, "Veg" },
                    { 2, "Non-Veg" },
                    { 3, "Sweet" },
                    { 4, "Spicy" },
                    { 5, "Salty" },
                    { 7, "Gluten-Free" },
                    { 8, "Dairy-Free" },
                    { 9, "Nut-Free" },
                    { 10, "Low-Calorie" },
                    { 11, "High-Protein" },
                    { 12, "Vegan" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "AverageRating", "Comments", "MenuItemCategoryId", "Name", "Price", "UserLikeability" },
                values: new object[,]
                {
                    { 24, 4.2m, null, 2, "Butter Chicken", 220.00m, 4.4m },
                    { 25, 4.6m, null, 1, "Omelette", 100.00m, 4.5m }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "LastSeenNotificationAt", "Name", "RoleId" },
                values: new object[,]
                {
                    { 1, "rgvn192@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rgvn", 2 },
                    { 2, "gordonramsay@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gordon Ramsay", 3 },
                    { 3, "amitjilovesbikaji@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Amitabh", 1 },
                    { 4, "mainhoondon@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shahrukh", 1 },
                    { 5, "bhai@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Salman", 1 },
                    { 6, "onlyunder25@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Leonardo", 1 }
                });

            migrationBuilder.InsertData(
                table: "MenuItemCharacteristics",
                columns: new[] { "Id", "CharacteristicId", "MenuItemId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 10, 1 },
                    { 3, 8, 1 },
                    { 4, 1, 2 },
                    { 5, 10, 2 },
                    { 6, 5, 2 },
                    { 7, 1, 3 },
                    { 8, 4, 3 },
                    { 9, 1, 4 },
                    { 10, 11, 4 },
                    { 11, 1, 5 },
                    { 12, 11, 5 },
                    { 13, 3, 5 },
                    { 14, 1, 6 },
                    { 15, 4, 6 },
                    { 16, 1, 7 },
                    { 17, 11, 7 },
                    { 18, 1, 8 },
                    { 19, 1, 18 },
                    { 20, 1, 19 },
                    { 21, 4, 19 },
                    { 22, 1, 9 },
                    { 23, 1, 10 },
                    { 24, 1, 12 },
                    { 25, 1, 20 },
                    { 26, 1, 21 },
                    { 27, 1, 13 },
                    { 28, 1, 14 },
                    { 29, 3, 14 },
                    { 30, 1, 15 },
                    { 31, 1, 16 },
                    { 32, 1, 17 },
                    { 33, 1, 22 },
                    { 34, 1, 23 },
                    { 35, 2, 24 },
                    { 36, 11, 24 },
                    { 37, 11, 25 },
                    { 38, 2, 25 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "UserPreferenceId", "CharacteristicId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, 3 },
                    { 2, 3, 3 },
                    { 3, 8, 3 },
                    { 4, 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "UserPreferenceId", "CharacteristicId", "UserId" },
                values: new object[,]
                {
                    { 5, 11, 4 },
                    { 6, 10, 4 },
                    { 7, 2, 5 },
                    { 8, 11, 5 },
                    { 9, 1, 6 },
                    { 10, 4, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCharacteristics_CharacteristicId",
                table: "MenuItemCharacteristics",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCharacteristics_MenuItemId",
                table: "MenuItemCharacteristics",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_CharacteristicId",
                table: "UserPreferences",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemCharacteristics");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);
        }
    }
}
