using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudhirTest.Migrations
{
    public partial class foo6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "futureinstrument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<long>(type: "bigint", nullable: false),
                    symbol = table.Column<string>(type: "text", nullable: true),
                    expiry = table.Column<string>(type: "text", nullable: true),
                    discription = table.Column<string>(type: "text", nullable: true),
                    displayname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_futureinstrument", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "futureinstrument");
        }
    }
}
