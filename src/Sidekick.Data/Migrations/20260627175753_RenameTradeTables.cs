using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameTradeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilterOptions_Filters_FilterId",
                table: "FilterOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Filters_FilterCategories_CategoryId",
                table: "Filters");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_StaticItems_StaticItemCategories_CategoryId",
                table: "StaticItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Stats_StatCategories_CategoryId",
                table: "Stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stats",
                table: "Stats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaticItems",
                table: "StaticItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StaticItemCategories",
                table: "StaticItemCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StatCategories",
                table: "StatCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Filters",
                table: "Filters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilterOptions",
                table: "FilterOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilterCategories",
                table: "FilterCategories");

            migrationBuilder.RenameTable(
                name: "Stats",
                newName: "TradeStats");

            migrationBuilder.RenameTable(
                name: "StaticItems",
                newName: "TradeStaticItems");

            migrationBuilder.RenameTable(
                name: "StaticItemCategories",
                newName: "TradeStaticItemCategories");

            migrationBuilder.RenameTable(
                name: "StatCategories",
                newName: "TradeStatCategories");

            migrationBuilder.RenameTable(
                name: "Leagues",
                newName: "TradeLeagues");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "TradeItems");

            migrationBuilder.RenameTable(
                name: "ItemCategories",
                newName: "TradeItemCategories");

            migrationBuilder.RenameTable(
                name: "Filters",
                newName: "TradeFilters");

            migrationBuilder.RenameTable(
                name: "FilterOptions",
                newName: "TradeFilterOptions");

            migrationBuilder.RenameTable(
                name: "FilterCategories",
                newName: "TradeFilterCategories");

            migrationBuilder.RenameIndex(
                name: "IX_Stats_Game_Language_Id",
                table: "TradeStats",
                newName: "IX_TradeStats_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Stats_CategoryId",
                table: "TradeStats",
                newName: "IX_TradeStats_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StaticItems_Game_Language_Id",
                table: "TradeStaticItems",
                newName: "IX_TradeStaticItems_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_StaticItems_CategoryId",
                table: "TradeStaticItems",
                newName: "IX_TradeStaticItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StaticItemCategories_Game_Language_Id",
                table: "TradeStaticItemCategories",
                newName: "IX_TradeStaticItemCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_StatCategories_Game_Language_Id",
                table: "TradeStatCategories",
                newName: "IX_TradeStatCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Items_Game_Language_CategoryId",
                table: "TradeItems",
                newName: "IX_TradeItems_Game_Language_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CategoryId",
                table: "TradeItems",
                newName: "IX_TradeItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemCategories_Game_Language_Id",
                table: "TradeItemCategories",
                newName: "IX_TradeItemCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Filters_Game_Language_Id",
                table: "TradeFilters",
                newName: "IX_TradeFilters_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Filters_CategoryId",
                table: "TradeFilters",
                newName: "IX_TradeFilters_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_FilterOptions_FilterId",
                table: "TradeFilterOptions",
                newName: "IX_TradeFilterOptions_FilterId");

            migrationBuilder.RenameIndex(
                name: "IX_FilterCategories_Game_Language_Id",
                table: "TradeFilterCategories",
                newName: "IX_TradeFilterCategories_Game_Language_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeStats",
                table: "TradeStats",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeStaticItems",
                table: "TradeStaticItems",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeStaticItemCategories",
                table: "TradeStaticItemCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeStatCategories",
                table: "TradeStatCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeLeagues",
                table: "TradeLeagues",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeItems",
                table: "TradeItems",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeItemCategories",
                table: "TradeItemCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeFilters",
                table: "TradeFilters",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeFilterOptions",
                table: "TradeFilterOptions",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TradeFilterCategories",
                table: "TradeFilterCategories",
                column: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeFilterOptions_TradeFilters_FilterId",
                table: "TradeFilterOptions",
                column: "FilterId",
                principalTable: "TradeFilters",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeFilters_TradeFilterCategories_CategoryId",
                table: "TradeFilters",
                column: "CategoryId",
                principalTable: "TradeFilterCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeItems_TradeItemCategories_CategoryId",
                table: "TradeItems",
                column: "CategoryId",
                principalTable: "TradeItemCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeStaticItems_TradeStaticItemCategories_CategoryId",
                table: "TradeStaticItems",
                column: "CategoryId",
                principalTable: "TradeStaticItemCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeStats_TradeStatCategories_CategoryId",
                table: "TradeStats",
                column: "CategoryId",
                principalTable: "TradeStatCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TradeFilterOptions_TradeFilters_FilterId",
                table: "TradeFilterOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeFilters_TradeFilterCategories_CategoryId",
                table: "TradeFilters");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeItems_TradeItemCategories_CategoryId",
                table: "TradeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeStaticItems_TradeStaticItemCategories_CategoryId",
                table: "TradeStaticItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeStats_TradeStatCategories_CategoryId",
                table: "TradeStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeStats",
                table: "TradeStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeStaticItems",
                table: "TradeStaticItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeStaticItemCategories",
                table: "TradeStaticItemCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeStatCategories",
                table: "TradeStatCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeLeagues",
                table: "TradeLeagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeItems",
                table: "TradeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeItemCategories",
                table: "TradeItemCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeFilters",
                table: "TradeFilters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeFilterOptions",
                table: "TradeFilterOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TradeFilterCategories",
                table: "TradeFilterCategories");

            migrationBuilder.RenameTable(
                name: "TradeStats",
                newName: "Stats");

            migrationBuilder.RenameTable(
                name: "TradeStaticItems",
                newName: "StaticItems");

            migrationBuilder.RenameTable(
                name: "TradeStaticItemCategories",
                newName: "StaticItemCategories");

            migrationBuilder.RenameTable(
                name: "TradeStatCategories",
                newName: "StatCategories");

            migrationBuilder.RenameTable(
                name: "TradeLeagues",
                newName: "Leagues");

            migrationBuilder.RenameTable(
                name: "TradeItems",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "TradeItemCategories",
                newName: "ItemCategories");

            migrationBuilder.RenameTable(
                name: "TradeFilters",
                newName: "Filters");

            migrationBuilder.RenameTable(
                name: "TradeFilterOptions",
                newName: "FilterOptions");

            migrationBuilder.RenameTable(
                name: "TradeFilterCategories",
                newName: "FilterCategories");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStats_Game_Language_Id",
                table: "Stats",
                newName: "IX_Stats_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStats_CategoryId",
                table: "Stats",
                newName: "IX_Stats_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStaticItems_Game_Language_Id",
                table: "StaticItems",
                newName: "IX_StaticItems_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStaticItems_CategoryId",
                table: "StaticItems",
                newName: "IX_StaticItems_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStaticItemCategories_Game_Language_Id",
                table: "StaticItemCategories",
                newName: "IX_StaticItemCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeStatCategories_Game_Language_Id",
                table: "StatCategories",
                newName: "IX_StatCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeItems_Game_Language_CategoryId",
                table: "Items",
                newName: "IX_Items_Game_Language_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeItems_CategoryId",
                table: "Items",
                newName: "IX_Items_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeItemCategories_Game_Language_Id",
                table: "ItemCategories",
                newName: "IX_ItemCategories_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeFilters_Game_Language_Id",
                table: "Filters",
                newName: "IX_Filters_Game_Language_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TradeFilters_CategoryId",
                table: "Filters",
                newName: "IX_Filters_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeFilterOptions_FilterId",
                table: "FilterOptions",
                newName: "IX_FilterOptions_FilterId");

            migrationBuilder.RenameIndex(
                name: "IX_TradeFilterCategories_Game_Language_Id",
                table: "FilterCategories",
                newName: "IX_FilterCategories_Game_Language_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stats",
                table: "Stats",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaticItems",
                table: "StaticItems",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StaticItemCategories",
                table: "StaticItemCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StatCategories",
                table: "StatCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemCategories",
                table: "ItemCategories",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Filters",
                table: "Filters",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilterOptions",
                table: "FilterOptions",
                column: "SidekickId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilterCategories",
                table: "FilterCategories",
                column: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilterOptions_Filters_FilterId",
                table: "FilterOptions",
                column: "FilterId",
                principalTable: "Filters",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Filters_FilterCategories_CategoryId",
                table: "Filters",
                column: "CategoryId",
                principalTable: "FilterCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemCategories_CategoryId",
                table: "Items",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StaticItems_StaticItemCategories_CategoryId",
                table: "StaticItems",
                column: "CategoryId",
                principalTable: "StaticItemCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stats_StatCategories_CategoryId",
                table: "Stats",
                column: "CategoryId",
                principalTable: "StatCategories",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
