using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudhirTest.Migrations
{
    public partial class foo7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "optionltpcallweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    ltp = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionltpcallweekly", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optionltpputweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    ltp = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionltpputweekly", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optionltqcallweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    ltq = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionltqcallweekly", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optionltqputweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    ltq = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionltqputweekly", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optionoicallweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    oi = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionoicallweekly", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "optionoiputweekly",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<long>(type: "bigint", nullable: false),
                    oi = table.Column<double>(type: "double precision", nullable: false),
                    strikeprice = table.Column<int>(type: "integer", nullable: false),
                    timestring = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_optionoiputweekly", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "optionltpcallweekly");

            migrationBuilder.DropTable(
                name: "optionltpputweekly");

            migrationBuilder.DropTable(
                name: "optionltqcallweekly");

            migrationBuilder.DropTable(
                name: "optionltqputweekly");

            migrationBuilder.DropTable(
                name: "optionoicallweekly");

            migrationBuilder.DropTable(
                name: "optionoiputweekly");
        }
    }
}
