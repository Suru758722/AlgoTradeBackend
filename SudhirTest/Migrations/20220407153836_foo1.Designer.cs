﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SudhirTest.Data;

namespace SudhirTest.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220407153836_foo1")]
    partial class foo1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrencystamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalizedname");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("aspnetroles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claimtype");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claimvalue");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("roleid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("aspnetroleclaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("accessfailedcount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrencystamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("emailconfirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockoutenabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockoutend");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalizedemail");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalizedusername");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("passwordhash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phonenumber");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phonenumberconfirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("securitystamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("twofactorenabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("aspnetusers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claimtype");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claimvalue");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("userid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("aspnetuserclaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("loginprovider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("providerkey");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("providerdisplayname");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("userid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("aspnetuserlogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("userid");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("roleid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("aspnetuserroles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("userid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("loginprovider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("aspnetusertokens");
                });

            modelBuilder.Entity("SudhirTest.Entity.ExchangeSegment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeid");

                    b.Property<string>("ExchangeName")
                        .HasColumnType("text")
                        .HasColumnName("exchangename");

                    b.HasKey("Id");

                    b.ToTable("exchangesegment");
                });

            modelBuilder.Entity("SudhirTest.Entity.Hdfc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("hdfc");
                });

            modelBuilder.Entity("SudhirTest.Entity.HdfcBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("hdfcbank");
                });

            modelBuilder.Entity("SudhirTest.Entity.HindUnilvr", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("hindunilvr");
                });

            modelBuilder.Entity("SudhirTest.Entity.IciciBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("icicibank");
                });

            modelBuilder.Entity("SudhirTest.Entity.Infy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("infy");
                });

            modelBuilder.Entity("SudhirTest.Entity.Instrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<int?>("ExchangeSegmentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangesegmentid");

                    b.Property<long>("InstrumentID")
                        .HasColumnType("bigint")
                        .HasColumnName("instrumentid");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Series")
                        .HasColumnType("text")
                        .HasColumnName("series");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSegmentId");

                    b.ToTable("instrument");
                });

            modelBuilder.Entity("SudhirTest.Entity.InstrumentData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ExchangeSegmentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangesegmentid");

                    b.Property<int?>("InstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("instrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("time");

                    b.HasKey("Id");

                    b.HasIndex("ExchangeSegmentId");

                    b.HasIndex("InstrumentId");

                    b.ToTable("instrumentdata");
                });

            modelBuilder.Entity("SudhirTest.Entity.KotakBank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("kotakbank");
                });

            modelBuilder.Entity("SudhirTest.Entity.LT", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("lt");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionIVCall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("IV")
                        .HasColumnType("double precision")
                        .HasColumnName("iv");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.HasKey("Id");

                    b.ToTable("optionivcall");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionIVPut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("IV")
                        .HasColumnType("double precision")
                        .HasColumnName("iv");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.HasKey("Id");

                    b.ToTable("optionivput");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionInstrument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Discription")
                        .HasColumnType("text")
                        .HasColumnName("discription");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("displayname");

                    b.Property<long>("ExchangeInstrumentId")
                        .HasColumnType("bigint")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<string>("Expiry")
                        .HasColumnType("text")
                        .HasColumnName("expiry");

                    b.Property<string>("OptionType")
                        .HasColumnType("text")
                        .HasColumnName("optiontype");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<string>("Symbol")
                        .HasColumnType("text")
                        .HasColumnName("symbol");

                    b.HasKey("Id");

                    b.ToTable("optioninstrument");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionLtpCall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("Ltp")
                        .HasColumnType("double precision")
                        .HasColumnName("ltp");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionltpcall");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionLtpPut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("Ltp")
                        .HasColumnType("double precision")
                        .HasColumnName("ltp");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionltpput");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionLtqCall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("Ltq")
                        .HasColumnType("double precision")
                        .HasColumnName("ltq");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionltqcall");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionLtqPut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("Ltq")
                        .HasColumnType("double precision")
                        .HasColumnName("ltq");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionltqput");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionOICall", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("OI")
                        .HasColumnType("double precision")
                        .HasColumnName("oi");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionoicall");
                });

            modelBuilder.Entity("SudhirTest.Entity.OptionOIPut", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentId")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("OI")
                        .HasColumnType("double precision")
                        .HasColumnName("oi");

                    b.Property<int>("StrikePrice")
                        .HasColumnType("integer")
                        .HasColumnName("strikeprice");

                    b.Property<long>("Time")
                        .HasColumnType("bigint")
                        .HasColumnName("time");

                    b.Property<string>("TimeString")
                        .HasColumnType("text")
                        .HasColumnName("timestring");

                    b.HasKey("Id");

                    b.ToTable("optionoiput");
                });

            modelBuilder.Entity("SudhirTest.Entity.Reliance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("reliance");
                });

            modelBuilder.Entity("SudhirTest.Entity.Sbin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("sbin");
                });

            modelBuilder.Entity("SudhirTest.Entity.Tcs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("ExchangeInstrumentID")
                        .HasColumnType("integer")
                        .HasColumnName("exchangeinstrumentid");

                    b.Property<double>("LastTradedPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("lasttradedprice");

                    b.Property<long>("LastTradedQunatity")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedqunatity");

                    b.Property<long>("LastTradedTime")
                        .HasColumnType("bigint")
                        .HasColumnName("lasttradedtime");

                    b.HasKey("Id");

                    b.ToTable("tcs");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SudhirTest.Entity.Instrument", b =>
                {
                    b.HasOne("SudhirTest.Entity.ExchangeSegment", "ExchangeSegment")
                        .WithMany()
                        .HasForeignKey("ExchangeSegmentId");

                    b.Navigation("ExchangeSegment");
                });

            modelBuilder.Entity("SudhirTest.Entity.InstrumentData", b =>
                {
                    b.HasOne("SudhirTest.Entity.ExchangeSegment", "ExchangeSegment")
                        .WithMany()
                        .HasForeignKey("ExchangeSegmentId");

                    b.HasOne("SudhirTest.Entity.Instrument", "Instrument")
                        .WithMany()
                        .HasForeignKey("InstrumentId");

                    b.Navigation("ExchangeSegment");

                    b.Navigation("Instrument");
                });
#pragma warning restore 612, 618
        }
    }
}
