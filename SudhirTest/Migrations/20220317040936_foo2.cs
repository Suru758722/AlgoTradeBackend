using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudhirTest.Migrations
{
    public partial class foo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SymbolData");

            migrationBuilder.DropTable(
                name: "Symbol");

            migrationBuilder.CreateTable(
                name: "ExchangeSegment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExchangeId = table.Column<int>(type: "integer", nullable: false),
                    ExchangeName = table.Column<string>(type: "text", nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeSegment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instrument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExchangeInstrumentID = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Series = table.Column<string>(type: "text", nullable: true),
                    InstrumentID = table.Column<int>(type: "integer", nullable: false),
                    ExchangeSegmentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instrument_ExchangeSegment_ExchangeSegmentId",
                        column: x => x.ExchangeSegmentId,
                        principalTable: "ExchangeSegment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastTradedPrice = table.Column<double>(type: "double precision", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    InstrumentId = table.Column<int>(type: "integer", nullable: true),
                    ExchangeSegmentId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstrumentData_ExchangeSegment_ExchangeSegmentId",
                        column: x => x.ExchangeSegmentId,
                        principalTable: "ExchangeSegment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstrumentData_Instrument_ExchangeSegmentId",
                        column: x => x.ExchangeSegmentId,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instrument_ExchangeSegmentId",
                table: "Instrument",
                column: "ExchangeSegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentData_ExchangeSegmentId",
                table: "InstrumentData",
                column: "ExchangeSegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstrumentData");

            migrationBuilder.DropTable(
                name: "Instrument");

            migrationBuilder.DropTable(
                name: "ExchangeSegment");

            migrationBuilder.CreateTable(
                name: "Symbol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SymbolCode = table.Column<string>(type: "text", nullable: true),
                    SymbolName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbol", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SymbolData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    SymbolId = table.Column<int>(type: "integer", nullable: true),
                    Time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymbolData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymbolData_Symbol_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbol",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SymbolData_SymbolId",
                table: "SymbolData",
                column: "SymbolId");
        }
    }
}
