using Microsoft.EntityFrameworkCore.Migrations;

namespace SudhirTest.Migrations
{
    public partial class foo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionoiput",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionoicall",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionltqput",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionltqcall",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionltpput",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "timestring",
                table: "optionltpcall",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionoiput");

            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionoicall");

            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionltqput");

            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionltqcall");

            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionltpput");

            migrationBuilder.DropColumn(
                name: "timestring",
                table: "optionltpcall");
        }
    }
}
