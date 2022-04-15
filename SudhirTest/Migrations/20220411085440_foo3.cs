using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudhirTest.Migrations
{
    public partial class foo3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "niftyfut",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false),
                    oi = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_niftyfut", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "niftyfut");
        }
    }
}
