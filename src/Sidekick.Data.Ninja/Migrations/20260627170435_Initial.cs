using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Ninja.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NinjaExchangeItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    DetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaExchangeItems", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "NinjaStashItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    BaseType = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Corrupted = table.Column<bool>(type: "INTEGER", nullable: true),
                    GemLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    GemQuality = table.Column<int>(type: "INTEGER", nullable: true),
                    Links = table.Column<int>(type: "INTEGER", nullable: true),
                    LevelRequired = table.Column<int>(type: "INTEGER", nullable: true),
                    Variant = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaStashItems", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "NinjaStashMutatedStats",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Optional = table.Column<bool>(type: "INTEGER", nullable: false),
                    StashItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaStashMutatedStats", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_NinjaStashMutatedStats_NinjaStashItems_StashItemId",
                        column: x => x.StashItemId,
                        principalTable: "NinjaStashItems",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NinjaStashTradeStats",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Mod = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Min = table.Column<int>(type: "INTEGER", nullable: true),
                    Max = table.Column<int>(type: "INTEGER", nullable: true),
                    Option = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    StashItemId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinjaStashTradeStats", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_NinjaStashTradeStats_NinjaStashItems_StashItemId",
                        column: x => x.StashItemId,
                        principalTable: "NinjaStashItems",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NinjaExchangeItems_Game_Type",
                table: "NinjaExchangeItems",
                columns: new[] { "Game", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_NinjaStashMutatedStats_StashItemId",
                table: "NinjaStashMutatedStats",
                column: "StashItemId");

            migrationBuilder.CreateIndex(
                name: "IX_NinjaStashTradeStats_StashItemId",
                table: "NinjaStashTradeStats",
                column: "StashItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NinjaExchangeItems");

            migrationBuilder.DropTable(
                name: "NinjaStashMutatedStats");

            migrationBuilder.DropTable(
                name: "NinjaStashTradeStats");

            migrationBuilder.DropTable(
                name: "NinjaStashItems");
        }
    }
}
