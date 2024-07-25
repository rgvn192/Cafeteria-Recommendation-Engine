using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationEngine.Data.Migrations
{
    public partial class UpdatedDiscardedMenuItemIdRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DicardedMenuItemId",
                table: "DiscardedMenuItems",
                newName: "DiscardedMenuItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DiscardedMenuItemId",
                table: "DiscardedMenuItems",
                newName: "DicardedMenuItemId");
        }
    }
}
