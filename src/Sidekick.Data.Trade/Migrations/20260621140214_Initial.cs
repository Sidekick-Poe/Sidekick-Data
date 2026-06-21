using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Trade.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Realm = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "StatCategories",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatCategories", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "StaticItemCategories",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItemCategories", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    IsUnique = table.Column<bool>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => new { x.Game, x.Language, x.Id, x.Discriminator });
                    table.ForeignKey(
                        name: "FK_Items_ItemCategories_Game_Language_CategoryId",
                        columns: x => new { x.Game, x.Language, x.CategoryId },
                        principalTable: "ItemCategories",
                        principalColumns: new[] { "Game", "Language", "Id" });
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    OptionText = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => new { x.Game, x.Language, x.Id, x.OptionId });
                    table.ForeignKey(
                        name: "FK_Stats_StatCategories_Game_Language_CategoryId",
                        columns: x => new { x.Game, x.Language, x.CategoryId },
                        principalTable: "StatCategories",
                        principalColumns: new[] { "Game", "Language", "Id" });
                });

            migrationBuilder.CreateTable(
                name: "StaticItems",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Image = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItems", x => new { x.Game, x.Language, x.Id });
                    table.ForeignKey(
                        name: "FK_StaticItems_StaticItemCategories_Game_Language_CategoryId",
                        columns: x => new { x.Game, x.Language, x.CategoryId },
                        principalTable: "StaticItemCategories",
                        principalColumns: new[] { "Game", "Language", "Id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Game_Language_CategoryId",
                table: "Items",
                columns: new[] { "Game", "Language", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_StaticItems_Game_Language_CategoryId",
                table: "StaticItems",
                columns: new[] { "Game", "Language", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Stats_Game_Language_CategoryId",
                table: "Stats",
                columns: new[] { "Game", "Language", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "StaticItems");

            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "StaticItemCategories");

            migrationBuilder.DropTable(
                name: "StatCategories");
        }
    }
}
