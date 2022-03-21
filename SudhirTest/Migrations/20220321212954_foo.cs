using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SudhirTest.Migrations
{
    public partial class foo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "aspnetroles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedname = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetroles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetusers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedusername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalizedemail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    emailconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    passwordhash = table.Column<string>(type: "text", nullable: true),
                    securitystamp = table.Column<string>(type: "text", nullable: true),
                    concurrencystamp = table.Column<string>(type: "text", nullable: true),
                    phonenumber = table.Column<string>(type: "text", nullable: true),
                    phonenumberconfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    twofactorenabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockoutend = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockoutenabled = table.Column<bool>(type: "boolean", nullable: false),
                    accessfailedcount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetusers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "exchangesegment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeid = table.Column<int>(type: "integer", nullable: false),
                    exchangename = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exchangesegment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hdfc",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hdfc", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hdfcbank",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hdfcbank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hindunilvr",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hindunilvr", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "icicibank",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_icicibank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "infy",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_infy", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "kotakbank",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kotakbank", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lt",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lt", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reliance",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reliance", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sbin",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sbin", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tcs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    lasttradedtime = table.Column<long>(type: "bigint", nullable: false),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    lasttradedqunatity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tcs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "aspnetroleclaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetroleclaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_aspnetroleclaims_aspnetroles_roleid",
                        column: x => x.roleid,
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserclaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<string>(type: "text", nullable: false),
                    claimtype = table.Column<string>(type: "text", nullable: true),
                    claimvalue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserclaims", x => x.id);
                    table.ForeignKey(
                        name: "FK_aspnetuserclaims_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserlogins",
                columns: table => new
                {
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    providerkey = table.Column<string>(type: "text", nullable: false),
                    providerdisplayname = table.Column<string>(type: "text", nullable: true),
                    userid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserlogins", x => new { x.loginprovider, x.providerkey });
                    table.ForeignKey(
                        name: "FK_aspnetuserlogins_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetuserroles",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    roleid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetuserroles", x => new { x.userid, x.roleid });
                    table.ForeignKey(
                        name: "FK_aspnetuserroles_aspnetroles_roleid",
                        column: x => x.roleid,
                        principalTable: "aspnetroles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_aspnetuserroles_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "aspnetusertokens",
                columns: table => new
                {
                    userid = table.Column<string>(type: "text", nullable: false),
                    loginprovider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aspnetusertokens", x => new { x.userid, x.loginprovider, x.name });
                    table.ForeignKey(
                        name: "FK_aspnetusertokens_aspnetusers_userid",
                        column: x => x.userid,
                        principalTable: "aspnetusers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "instrument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exchangeinstrumentid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    series = table.Column<string>(type: "text", nullable: true),
                    instrumentid = table.Column<long>(type: "bigint", nullable: false),
                    exchangesegmentid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrument", x => x.id);
                    table.ForeignKey(
                        name: "FK_instrument_exchangesegment_exchangesegmentid",
                        column: x => x.exchangesegmentid,
                        principalTable: "exchangesegment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "instrumentdata",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lasttradedprice = table.Column<double>(type: "double precision", nullable: false),
                    time = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    instrumentid = table.Column<int>(type: "integer", nullable: true),
                    exchangesegmentid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instrumentdata", x => x.id);
                    table.ForeignKey(
                        name: "FK_instrumentdata_exchangesegment_exchangesegmentid",
                        column: x => x.exchangesegmentid,
                        principalTable: "exchangesegment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_instrumentdata_instrument_instrumentid",
                        column: x => x.instrumentid,
                        principalTable: "instrument",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_aspnetroleclaims_roleid",
                table: "aspnetroleclaims",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "aspnetroles",
                column: "normalizedname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserclaims_userid",
                table: "aspnetuserclaims",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserlogins_userid",
                table: "aspnetuserlogins",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_aspnetuserroles_roleid",
                table: "aspnetuserroles",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "aspnetusers",
                column: "normalizedemail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "aspnetusers",
                column: "normalizedusername",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_instrument_exchangesegmentid",
                table: "instrument",
                column: "exchangesegmentid");

            migrationBuilder.CreateIndex(
                name: "IX_instrumentdata_exchangesegmentid",
                table: "instrumentdata",
                column: "exchangesegmentid");

            migrationBuilder.CreateIndex(
                name: "IX_instrumentdata_instrumentid",
                table: "instrumentdata",
                column: "instrumentid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aspnetroleclaims");

            migrationBuilder.DropTable(
                name: "aspnetuserclaims");

            migrationBuilder.DropTable(
                name: "aspnetuserlogins");

            migrationBuilder.DropTable(
                name: "aspnetuserroles");

            migrationBuilder.DropTable(
                name: "aspnetusertokens");

            migrationBuilder.DropTable(
                name: "hdfc");

            migrationBuilder.DropTable(
                name: "hdfcbank");

            migrationBuilder.DropTable(
                name: "hindunilvr");

            migrationBuilder.DropTable(
                name: "icicibank");

            migrationBuilder.DropTable(
                name: "infy");

            migrationBuilder.DropTable(
                name: "instrumentdata");

            migrationBuilder.DropTable(
                name: "kotakbank");

            migrationBuilder.DropTable(
                name: "lt");

            migrationBuilder.DropTable(
                name: "reliance");

            migrationBuilder.DropTable(
                name: "sbin");

            migrationBuilder.DropTable(
                name: "tcs");

            migrationBuilder.DropTable(
                name: "aspnetroles");

            migrationBuilder.DropTable(
                name: "aspnetusers");

            migrationBuilder.DropTable(
                name: "instrument");

            migrationBuilder.DropTable(
                name: "exchangesegment");
        }
    }
}
