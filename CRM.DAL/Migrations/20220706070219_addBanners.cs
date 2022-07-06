using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.DAL.Migrations
{
    public partial class addBanners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Base64Content",
                table: "Files",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SiaMonitoredBlocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Height = table.Column<long>(type: "bigint", nullable: false),
                    Hash = table.Column<string>(type: "text", nullable: true),
                    MonitoringTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiaMonitoredBlocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiaRenterAllowances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Period = table.Column<string>(type: "text", nullable: true),
                    RenewWindow = table.Column<string>(type: "text", nullable: true),
                    Funds = table.Column<string>(type: "text", nullable: true),
                    Hosts = table.Column<string>(type: "text", nullable: true),
                    RegistrationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiaRenterAllowances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiaTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SiaId = table.Column<string>(type: "text", nullable: true),
                    CoinsValue = table.Column<decimal>(type: "numeric", nullable: false),
                    InitialHeight = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Confirmations = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DestinationAddress = table.Column<string>(type: "text", nullable: true),
                    OnBalance = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiaTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiaTransactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSiaAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSiaAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSiaAddresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiaTransactions_UserId",
                table: "SiaTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSiaAddresses_UserId",
                table: "UserSiaAddresses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiaMonitoredBlocks");

            migrationBuilder.DropTable(
                name: "SiaRenterAllowances");

            migrationBuilder.DropTable(
                name: "SiaTransactions");

            migrationBuilder.DropTable(
                name: "UserSiaAddresses");

            migrationBuilder.DropColumn(
                name: "Base64Content",
                table: "Files");
        }
    }
}
