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
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: true),
                    FullSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    HalfSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    MinMax = table.Column<bool>(type: "INTEGER", nullable: true),
                    Sockets = table.Column<bool>(type: "INTEGER", nullable: true),
                    Tip = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    IsUnique = table.Column<bool>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Realm = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "StaticItems",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Image = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItems", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    CategoryId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => new { x.Game, x.Language, x.Id });
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
                        name: "FK_FilterOptions_Filters_Game_Language_FilterId",
                        columns: x => new { x.Game, x.Language, x.FilterId },
                        principalTable: "Filters",
                        principalColumns: new[] { "Game", "Language", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatOptions",
                columns: table => new
                {
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatOptions", x => new { x.Game, x.Language, x.StatId, x.Id });
                    table.ForeignKey(
                        name: "FK_StatOptions_Stats_Game_Language_StatId",
                        columns: x => new { x.Game, x.Language, x.StatId },
                        principalTable: "Stats",
                        principalColumns: new[] { "Game", "Language", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterOptions_Game_Language_FilterId",
                table: "FilterOptions",
                columns: new[] { "Game", "Language", "FilterId" });
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
                name: "Stats");
        }
    }
}
