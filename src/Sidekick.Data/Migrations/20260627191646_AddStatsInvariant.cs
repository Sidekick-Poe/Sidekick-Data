using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatsInvariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatsInvariantColdWeaponDamage",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantColdWeaponDamage", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantFireWeaponDamage",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantFireWeaponDamage", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantIgnore",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantIgnore", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantIncursionRoom",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantIncursionRoom", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantLightningWeaponDamage",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantLightningWeaponDamage", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantLogbookBoss",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantLogbookBoss", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "StatsInvariantLogbookFaction",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    StatId = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsInvariantLogbookFaction", x => x.SidekickId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatsInvariantColdWeaponDamage");

            migrationBuilder.DropTable(
                name: "StatsInvariantFireWeaponDamage");

            migrationBuilder.DropTable(
                name: "StatsInvariantIgnore");

            migrationBuilder.DropTable(
                name: "StatsInvariantIncursionRoom");

            migrationBuilder.DropTable(
                name: "StatsInvariantLightningWeaponDamage");

            migrationBuilder.DropTable(
                name: "StatsInvariantLogbookBoss");

            migrationBuilder.DropTable(
                name: "StatsInvariantLogbookFaction");
        }
    }
}
