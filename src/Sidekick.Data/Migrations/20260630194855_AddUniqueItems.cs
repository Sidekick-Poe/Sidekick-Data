using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems");

            migrationBuilder.RenameColumn(
                name: "ItemClassId",
                table: "UniqueItems",
                newName: "BaseItemId");

            migrationBuilder.RenameIndex(
                name: "IX_UniqueItems_ItemClassId",
                table: "UniqueItems",
                newName: "IX_UniqueItems_BaseItemId");

            migrationBuilder.AddColumn<string>(
                name: "ItemVisualIdentityId",
                table: "BaseItems",
                type: "TEXT",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueItems_BaseItems_BaseItemId",
                table: "UniqueItems",
                column: "BaseItemId",
                principalTable: "BaseItems",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UniqueItems_BaseItems_BaseItemId",
                table: "UniqueItems");

            migrationBuilder.DropColumn(
                name: "ItemVisualIdentityId",
                table: "BaseItems");

            migrationBuilder.RenameColumn(
                name: "BaseItemId",
                table: "UniqueItems",
                newName: "ItemClassId");

            migrationBuilder.RenameIndex(
                name: "IX_UniqueItems_BaseItemId",
                table: "UniqueItems",
                newName: "IX_UniqueItems_ItemClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_UniqueItems_ItemClasses_ItemClassId",
                table: "UniqueItems",
                column: "ItemClassId",
                principalTable: "ItemClasses",
                principalColumn: "SidekickId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
