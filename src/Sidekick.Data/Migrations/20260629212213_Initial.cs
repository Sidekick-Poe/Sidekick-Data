using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sidekick.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemClasses",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemClasses", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "NinjaExchangeItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
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
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    Url = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TradeFilterCategories",
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
                    table.PrimaryKey("PK_TradeFilterCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "TradeItemCategories",
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
                    table.PrimaryKey("PK_TradeItemCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "TradeLeagues",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Realm = table.Column<byte>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeLeagues", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "TradeStatCategories",
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
                    table.PrimaryKey("PK_TradeStatCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "TradeStaticItemCategories",
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
                    table.PrimaryKey("PK_TradeStaticItemCategories", x => x.SidekickId);
                });

            migrationBuilder.CreateTable(
                name: "BaseItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    ArmourMin = table.Column<int>(type: "INTEGER", nullable: true),
                    ArmourMax = table.Column<int>(type: "INTEGER", nullable: true),
                    EnergyShieldMin = table.Column<int>(type: "INTEGER", nullable: true),
                    EnergyShieldMax = table.Column<int>(type: "INTEGER", nullable: true),
                    EvasionMin = table.Column<int>(type: "INTEGER", nullable: true),
                    EvasionMax = table.Column<int>(type: "INTEGER", nullable: true),
                    WardMin = table.Column<int>(type: "INTEGER", nullable: true),
                    WardMax = table.Column<int>(type: "INTEGER", nullable: true),
                    PhysicalDamageMin = table.Column<int>(type: "INTEGER", nullable: true),
                    PhysicalDamageMax = table.Column<int>(type: "INTEGER", nullable: true),
                    AttacksPerSecond = table.Column<double>(type: "REAL", nullable: true),
                    CriticalHitChance = table.Column<double>(type: "REAL", nullable: true),
                    Block = table.Column<int>(type: "INTEGER", nullable: true),
                    DropLevel = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiresDexterity = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiresIntelligence = table.Column<int>(type: "INTEGER", nullable: true),
                    RequiresStrength = table.Column<int>(type: "INTEGER", nullable: true),
                    ItemClassId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseItems", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_BaseItems_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "SidekickId");
                });

            migrationBuilder.CreateTable(
                name: "UniqueItems",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Image = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    ItemClassId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UniqueItems", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_UniqueItems_ItemClasses_ItemClassId",
                        column: x => x.ItemClassId,
                        principalTable: "ItemClasses",
                        principalColumn: "SidekickId");
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

            migrationBuilder.CreateTable(
                name: "TradeFilters",
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
                    table.PrimaryKey("PK_TradeFilters", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_TradeFilters_TradeFilterCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TradeFilterCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeStats",
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
                    table.PrimaryKey("PK_TradeStats", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_TradeStats_TradeStatCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TradeStatCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeStaticItems",
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
                    table.PrimaryKey("PK_TradeStaticItems", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_TradeStaticItems_TradeStaticItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TradeStaticItemCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeFilterOptions",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Id = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    FilterId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeFilterOptions", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_TradeFilterOptions_TradeFilters_FilterId",
                        column: x => x.FilterId,
                        principalTable: "TradeFilters",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeItems",
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
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StaticItemId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeItems", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_TradeItems_TradeItemCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "TradeItemCategories",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TradeItems_TradeStaticItems_StaticItemId",
                        column: x => x.StaticItemId,
                        principalTable: "TradeStaticItems",
                        principalColumn: "SidekickId");
                });

            migrationBuilder.CreateTable(
                name: "ItemDefinitions",
                columns: table => new
                {
                    SidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Game = table.Column<byte>(type: "INTEGER", nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    TradeItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    BaseItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UniqueItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    NinjaExchangeItemId = table.Column<Guid>(type: "TEXT", nullable: true),
                    NamePattern = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    TypePattern = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true),
                    TextPattern = table.Column<string>(type: "TEXT", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDefinitions", x => x.SidekickId);
                    table.ForeignKey(
                        name: "FK_ItemDefinitions_BaseItems_BaseItemId",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItems",
                        principalColumn: "SidekickId");
                    table.ForeignKey(
                        name: "FK_ItemDefinitions_NinjaExchangeItems_NinjaExchangeItemId",
                        column: x => x.NinjaExchangeItemId,
                        principalTable: "NinjaExchangeItems",
                        principalColumn: "SidekickId");
                    table.ForeignKey(
                        name: "FK_ItemDefinitions_TradeItems_TradeItemId",
                        column: x => x.TradeItemId,
                        principalTable: "TradeItems",
                        principalColumn: "SidekickId");
                    table.ForeignKey(
                        name: "FK_ItemDefinitions_UniqueItems_UniqueItemId",
                        column: x => x.UniqueItemId,
                        principalTable: "UniqueItems",
                        principalColumn: "SidekickId");
                });

            migrationBuilder.CreateTable(
                name: "ItemDefinitionEntityNinjaStashItem",
                columns: table => new
                {
                    ItemDefinitionsSidekickId = table.Column<Guid>(type: "TEXT", nullable: false),
                    NinjaStashItemsSidekickId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDefinitionEntityNinjaStashItem", x => new { x.ItemDefinitionsSidekickId, x.NinjaStashItemsSidekickId });
                    table.ForeignKey(
                        name: "FK_ItemDefinitionEntityNinjaStashItem_ItemDefinitions_ItemDefinitionsSidekickId",
                        column: x => x.ItemDefinitionsSidekickId,
                        principalTable: "ItemDefinitions",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemDefinitionEntityNinjaStashItem_NinjaStashItems_NinjaStashItemsSidekickId",
                        column: x => x.NinjaStashItemsSidekickId,
                        principalTable: "NinjaStashItems",
                        principalColumn: "SidekickId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_Game_Language",
                table: "BaseItems",
                columns: new[] { "Game", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_BaseItems_ItemClassId",
                table: "BaseItems",
                column: "ItemClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemClasses_Game_Language_Id",
                table: "ItemClasses",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitionEntityNinjaStashItem_NinjaStashItemsSidekickId",
                table: "ItemDefinitionEntityNinjaStashItem",
                column: "NinjaStashItemsSidekickId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_BaseItemId",
                table: "ItemDefinitions",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_Game_Language",
                table: "ItemDefinitions",
                columns: new[] { "Game", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_NinjaExchangeItemId",
                table: "ItemDefinitions",
                column: "NinjaExchangeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_TradeItemId",
                table: "ItemDefinitions",
                column: "TradeItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemDefinitions_UniqueItemId",
                table: "ItemDefinitions",
                column: "UniqueItemId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TradeFilterCategories_Game_Language_Id",
                table: "TradeFilterCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeFilterOptions_FilterId",
                table: "TradeFilterOptions",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeFilters_CategoryId",
                table: "TradeFilters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeFilters_Game_Language_Id",
                table: "TradeFilters",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeItemCategories_Game_Language_Id",
                table: "TradeItemCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeItems_CategoryId",
                table: "TradeItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeItems_Game_Language",
                table: "TradeItems",
                columns: new[] { "Game", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeItems_StaticItemId",
                table: "TradeItems",
                column: "StaticItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeLeagues_Game_Language_Id",
                table: "TradeLeagues",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeStatCategories_Game_Language_Id",
                table: "TradeStatCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeStaticItemCategories_Game_Language_Id",
                table: "TradeStaticItemCategories",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeStaticItems_CategoryId",
                table: "TradeStaticItems",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeStaticItems_Game_Language_Id",
                table: "TradeStaticItems",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_TradeStats_CategoryId",
                table: "TradeStats",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeStats_Game_Language_Id",
                table: "TradeStats",
                columns: new[] { "Game", "Language", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_UniqueItems_Game_Language",
                table: "UniqueItems",
                columns: new[] { "Game", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_UniqueItems_ItemClassId",
                table: "UniqueItems",
                column: "ItemClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemDefinitionEntityNinjaStashItem");

            migrationBuilder.DropTable(
                name: "NinjaStashMutatedStats");

            migrationBuilder.DropTable(
                name: "NinjaStashTradeStats");

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

            migrationBuilder.DropTable(
                name: "TradeFilterOptions");

            migrationBuilder.DropTable(
                name: "TradeLeagues");

            migrationBuilder.DropTable(
                name: "TradeStats");

            migrationBuilder.DropTable(
                name: "ItemDefinitions");

            migrationBuilder.DropTable(
                name: "NinjaStashItems");

            migrationBuilder.DropTable(
                name: "TradeFilters");

            migrationBuilder.DropTable(
                name: "TradeStatCategories");

            migrationBuilder.DropTable(
                name: "BaseItems");

            migrationBuilder.DropTable(
                name: "NinjaExchangeItems");

            migrationBuilder.DropTable(
                name: "TradeItems");

            migrationBuilder.DropTable(
                name: "UniqueItems");

            migrationBuilder.DropTable(
                name: "TradeFilterCategories");

            migrationBuilder.DropTable(
                name: "TradeItemCategories");

            migrationBuilder.DropTable(
                name: "TradeStaticItems");

            migrationBuilder.DropTable(
                name: "ItemClasses");

            migrationBuilder.DropTable(
                name: "TradeStaticItemCategories");
        }
    }
}
