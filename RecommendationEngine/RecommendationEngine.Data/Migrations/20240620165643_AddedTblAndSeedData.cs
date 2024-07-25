using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedTblAndSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "varchar(70)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealTypes",
                columns: table => new
                {
                    MealTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.MealTypeId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.NotificationTypeId);
                });

            migrationBuilder.CreateTable(
                name: "DailyMenuRecommendations",
                columns: table => new
                {
                    DailyMenuRecommendationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Votes = table.Column<int>(type: "int", nullable: false),
                    IsShortListed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
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

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "varchar(500)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "NotificationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MealTypes",
                columns: new[] { "MealTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Breakfast" },
                    { 2, "Lunch" },
                    { 3, "Dinner" }
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "MenuItemVoting" },
                    { 2, "FinalizeMenu" },
                    { 3, "NewMenuItemAdded" },
                    { 4, "MenuItemAvailabilityUpdated" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuRecommendations_MealTypeId",
                table: "DailyMenuRecommendations",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMenuRecommendations_MenuItemId",
                table: "DailyMenuRecommendations",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_MenuItemId",
                table: "Feedbacks",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationTypes_Name",
                table: "NotificationTypes",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMenuRecommendations");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "MealTypes");

            migrationBuilder.DropTable(
                name: "NotificationTypes");
        }
    }
}
