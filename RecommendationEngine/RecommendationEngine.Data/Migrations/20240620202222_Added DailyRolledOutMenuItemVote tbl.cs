using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedDailyRolledOutMenuItemVotetbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMenuRecommendations");

            migrationBuilder.CreateTable(
                name: "DailyRolledOutMenuItems",
                columns: table => new
                {
                    DailyRolledOutMenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsShortListed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRolledOutMenuItems", x => x.DailyRolledOutMenuItemId);
                    table.ForeignKey(
                        name: "FK_DailyRolledOutMenuItems_MealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTypes",
                        principalColumn: "MealTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyRolledOutMenuItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DailyRolledOutMenuItemVotes",
                columns: table => new
                {
                    DailyRolledOutMenuItemVoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DailyRolledOutMenuItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyRolledOutMenuItemVotes", x => x.DailyRolledOutMenuItemVoteId);
                    table.ForeignKey(
                        name: "FK_DailyRolledOutMenuItemVotes_DailyRolledOutMenuItems_DailyRolledOutMenuItemId",
                        column: x => x.DailyRolledOutMenuItemId,
                        principalTable: "DailyRolledOutMenuItems",
                        principalColumn: "DailyRolledOutMenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyRolledOutMenuItemVotes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyRolledOutMenuItems_MealTypeId",
                table: "DailyRolledOutMenuItems",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRolledOutMenuItems_MenuItemId",
                table: "DailyRolledOutMenuItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRolledOutMenuItemVotes_DailyRolledOutMenuItemId",
                table: "DailyRolledOutMenuItemVotes",
                column: "DailyRolledOutMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyRolledOutMenuItemVotes_UserId",
                table: "DailyRolledOutMenuItemVotes",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyRolledOutMenuItemVotes");

            migrationBuilder.DropTable(
                name: "DailyRolledOutMenuItems");

            migrationBuilder.CreateTable(
                name: "DailyMenuRecommendations",
                columns: table => new
                {
                    DailyMenuRecommendationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    IsShortListed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()"),
                    Votes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMenuRecommendations", x => x.DailyMenuRecommendationId);
                    table.ForeignKey(
                        name: "FK_DailyMenuRecommendations_MealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTypes",
                        principalColumn: "MealTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMenuRecommendations_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuRecommendations_MealTypeId",
                table: "DailyMenuRecommendations",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuRecommendations_MenuItemId",
                table: "DailyMenuRecommendations",
                column: "MenuItemId");
        }
    }
}
