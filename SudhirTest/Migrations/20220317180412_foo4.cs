using Microsoft.EntityFrameworkCore.Migrations;

namespace SudhirTest.Migrations
{
    public partial class foo4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentData_Instrument_ExchangeSegmentId",
                table: "InstrumentData");

            migrationBuilder.CreateIndex(
                name: "IX_InstrumentData_InstrumentId",
                table: "InstrumentData",
                column: "InstrumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentData_Instrument_InstrumentId",
                table: "InstrumentData",
                column: "InstrumentId",
                principalTable: "Instrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstrumentData_Instrument_InstrumentId",
                table: "InstrumentData");

            migrationBuilder.DropIndex(
                name: "IX_InstrumentData_InstrumentId",
                table: "InstrumentData");

            migrationBuilder.AddForeignKey(
                name: "FK_InstrumentData_Instrument_ExchangeSegmentId",
                table: "InstrumentData",
                column: "ExchangeSegmentId",
                principalTable: "Instrument",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
