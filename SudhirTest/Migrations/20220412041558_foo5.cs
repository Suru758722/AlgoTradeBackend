using Microsoft.EntityFrameworkCore.Migrations;

namespace SudhirTest.Migrations
{
    public partial class foo5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "oi",
                table: "niftyfut",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "niftyfut",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timestring",
                table: "niftyfut");

            migrationBuilder.AlterColumn<long>(
                name: "oi",
                table: "niftyfut",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
