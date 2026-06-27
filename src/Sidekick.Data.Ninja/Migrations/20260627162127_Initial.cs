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
                name: "ExchangeItems",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    DetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeItems", x => x.UniqueId);
                });

            migrationBuilder.CreateTable(
                name: "StashItems",
                columns: table => new
                {
                    DetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
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
                    table.PrimaryKey("PK_StashItems", x => x.DetailsId);
                });

            migrationBuilder.CreateTable(
                name: "StashMutatedStats",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StashItemDetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StashMutatedStats", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_StashMutatedStats_StashItems_StashItemDetailsId",
                        column: x => x.StashItemDetailsId,
                        principalTable: "StashItems",
                        principalColumn: "DetailsId");
                });

            migrationBuilder.CreateTable(
                name: "StashTradeStats",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StashItemDetailsId = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Mod = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    Value = table.Column<int>(type: "INTEGER", nullable: true),
                    Option = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StashTradeStats", x => x.UniqueId);
                    table.ForeignKey(
                        name: "FK_StashTradeStats_StashItems_StashItemDetailsId",
                        column: x => x.StashItemDetailsId,
                        principalTable: "StashItems",
                        principalColumn: "DetailsId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeItems_Game_Type",
                table: "ExchangeItems",
                columns: new[] { "Game", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_StashMutatedStats_StashItemDetailsId",
                table: "StashMutatedStats",
                column: "StashItemDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_StashTradeStats_StashItemDetailsId",
                table: "StashTradeStats",
                column: "StashItemDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeItems");

            migrationBuilder.DropTable(
                name: "StashMutatedStats");

            migrationBuilder.DropTable(
                name: "StashTradeStats");

            migrationBuilder.DropTable(
                name: "StashItems");
        }
    }
}
