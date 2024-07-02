using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class AddedDiscardedMenuItemanditsfeedbacktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DiscardedMenuItems",
                columns: table => new
                {
                    DicardedMenuItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardedMenuItems", x => x.DicardedMenuItemId);
                    table.ForeignKey(
                        name: "FK_DiscardedMenuItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscardedMenuItemFeedbacks",
                columns: table => new
                {
                    DiscardedMenuItemFeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscardedMenuItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDatetime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    ModifiedDatetime = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscardedMenuItemFeedbacks", x => x.DiscardedMenuItemFeedbackId);
                    table.ForeignKey(
                        name: "FK_DiscardedMenuItemFeedbacks_DiscardedMenuItems_DiscardedMenuItemId",
                        column: x => x.DiscardedMenuItemId,
                        principalTable: "DiscardedMenuItems",
                        principalColumn: "DicardedMenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscardedMenuItemFeedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "Name" },
                values: new object[] { 5, "DiscardMenuUpdated" });

            migrationBuilder.CreateIndex(
                name: "IX_DiscardedMenuItemFeedbacks_DiscardedMenuItemId",
                table: "DiscardedMenuItemFeedbacks",
                column: "DiscardedMenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscardedMenuItemFeedbacks_UserId",
                table: "DiscardedMenuItemFeedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscardedMenuItems_MenuItemId",
                table: "DiscardedMenuItems",
                column: "MenuItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscardedMenuItemFeedbacks");

            migrationBuilder.DropTable(
                name: "DiscardedMenuItems");

            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "MenuItems");
        }
    }
}
