using System;
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
                name: "FilterCategories",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategories",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Realm = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => new { x.Game, x.Language, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "StatCategories",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StaticItemCategories",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Label = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItemCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "Filters",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Hidden = table.Column<bool>(type: "INTEGER", nullable: true),
                    FullSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    HalfSpan = table.Column<bool>(type: "INTEGER", nullable: true),
                    MinMax = table.Column<bool>(type: "INTEGER", nullable: true),
                    Sockets = table.Column<bool>(type: "INTEGER", nullable: true),
                    Tip = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filters", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_Filters_FilterCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "FilterCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    IsUnique = table.Column<bool>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_Items_ItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ItemCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OptionText = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_Stats_StatCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "StatCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaticItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Image = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticItems", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_StaticItems_StaticItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "StaticItemCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilterOptions",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    FilterId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilterOptions", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_FilterOptions_Filters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "Filters",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilterCategories_Game_Language_Id",
                table: "FilterCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_FilterOptions_FilterId",
                table: "FilterOptions",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_CategoryId",
                table: "Filters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Filters_Game_Language_Id",
                table: "Filters",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCategories_Game_Language_Id",
                table: "ItemCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Items_CategoryId",
                table: "Items",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Game_Language_CategoryId",
                table: "Items",
                columns: new[] { "Game", "Language", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_StatCategories_Game_Language_Id",
                table: "StatCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_StaticItemCategories_Game_Language_Id",
                table: "StaticItemCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_StaticItems_CategoryId",
                table: "StaticItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticItems_Game_Language_Id",
                table: "StaticItems",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_Stats_CategoryId",
                table: "Stats",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Stats_Game_Language_Id",
                table: "Stats",
                columns: new[] { "Game", "Language", "Id" });
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
                name: "Stats");

            migrationBuilder.DropTable(
                name: "Filters");

            migrationBuilder.DropTable(
                name: "ItemCategories");

            migrationBuilder.DropTable(
                name: "StaticItemCategories");

            migrationBuilder.DropTable(
                name: "StatCategories");

            migrationBuilder.DropTable(
                name: "FilterCategories");
        }
    }
}
