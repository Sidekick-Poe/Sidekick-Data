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
                    FilterGroupId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: true),
                    FullSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    HalfSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    MinMax = table.Column<bool>(type: "INTEGER", nullable: true),
                    Sockets = table.Column<bool>(type: "INTEGER", nullable: true),
                    Tip = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => new { x.Game, x.Language, x.FilterGroupId, x.Id });
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
                name: "FilterOptions",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    FilterGroupId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    FilterId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterOptions", x => new { x.Game, x.Language, x.FilterGroupId, x.FilterId, x.Id });
                    table.ForeignKey(
                        name: "FK_FilterOptions_Filters_Game_Language_FilterGroupId_FilterId",
                        columns: x => new { x.Game, x.Language, x.FilterGroupId, x.FilterId },
                        principalTable: "Filters",
                        principalColumns: new[] { "Game", "Language", "FilterGroupId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    IsUnique = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => new { x.Game, x.Language, x.Id });
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
                    Text = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => new { x.Game, x.Language, x.Id });
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

            migrationBuilder.CreateTable(
                name: "StatOptions",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    StatId = table.Column<string>(type: "TEXT", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    TradeStatGame = table.Column<int>(type: "INTEGER", nullable: true),
                    TradeStatId = table.Column<string>(type: "TEXT", nullable: true),
                    TradeStatLanguage = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatOptions", x => new { x.Game, x.Language, x.StatId, x.Id });
                    table.ForeignKey(
                        name: "FK_StatOptions_Stats_TradeStatGame_TradeStatLanguage_TradeStatId",
                        columns: x => new { x.TradeStatGame, x.TradeStatLanguage, x.TradeStatId },
                        principalTable: "Stats",
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
                name: "IX_StatOptions_TradeStatGame_TradeStatLanguage_TradeStatId",
                table: "StatOptions",
                columns: new[] { "TradeStatGame", "TradeStatLanguage", "TradeStatId" });

            migrationBuilder.CreateIndex(
                name: "IX_Stats_Game_Language_CategoryId",
                table: "Stats",
                columns: new[] { "Game", "Language", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilterOptions");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "StaticItems");

            migrationBuilder.DropTable(
                name: "StatOptions");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "StaticItemCategories");

            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropTable(
                name: "StatCategories");
        }
    }
}
