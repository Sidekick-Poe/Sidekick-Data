using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_ItemClasses_ItemClassId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_BaseItems_BaseItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_NinjaExchangeItems_NinjaExchangeItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_TradeItems_TradeItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_UniqueItems_UniqueItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeItems_TradeStaticItems_StaticItemId",
                table: "TradeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_ItemClasses_ItemClassId",
                table: "BaseItems",
                column: "ItemClassId",
                principalTable: "ItemClasses",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_BaseItems_BaseItemId",
                table: "ItemDefinitions",
                column: "BaseItemId",
                principalTable: "BaseItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_NinjaExchangeItems_NinjaExchangeItemId",
                table: "ItemDefinitions",
                column: "NinjaExchangeItemId",
                principalTable: "NinjaExchangeItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_TradeItems_TradeItemId",
                table: "ItemDefinitions",
                column: "TradeItemId",
                principalTable: "TradeItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_UniqueItems_UniqueItemId",
                table: "ItemDefinitions",
                column: "UniqueItemId",
                principalTable: "UniqueItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TradeItems_TradeStaticItems_StaticItemId",
                table: "TradeItems",
                column: "StaticItemId",
                principalTable: "TradeStaticItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems",
                column: "ItemClassId",
                principalTable: "ItemClasses",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItems_ItemClasses_ItemClassId",
                table: "BaseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_BaseItems_BaseItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_NinjaExchangeItems_NinjaExchangeItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_TradeItems_TradeItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemDefinitions_UniqueItems_UniqueItemId",
                table: "ItemDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_TradeItems_TradeStaticItems_StaticItemId",
                table: "TradeItems");

            migrationBuilder.DropForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItems_ItemClasses_ItemClassId",
                table: "BaseItems",
                column: "ItemClassId",
                principalTable: "ItemClasses",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_BaseItems_BaseItemId",
                table: "ItemDefinitions",
                column: "BaseItemId",
                principalTable: "BaseItems",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_NinjaExchangeItems_NinjaExchangeItemId",
                table: "ItemDefinitions",
                column: "NinjaExchangeItemId",
                principalTable: "NinjaExchangeItems",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_TradeItems_TradeItemId",
                table: "ItemDefinitions",
                column: "TradeItemId",
                principalTable: "TradeItems",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemDefinitions_UniqueItems_UniqueItemId",
                table: "ItemDefinitions",
                column: "UniqueItemId",
                principalTable: "UniqueItems",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_TradeItems_TradeStaticItems_StaticItemId",
                table: "TradeItems",
                column: "StaticItemId",
                principalTable: "TradeStaticItems",
                principalColumn: "SidekickId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems",
                column: "ItemClassId",
                principalTable: "ItemClasses",
                principalColumn: "SidekickId");
        }
    }
}
