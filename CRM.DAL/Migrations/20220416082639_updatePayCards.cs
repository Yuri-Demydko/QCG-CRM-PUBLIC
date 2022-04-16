using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.DAL.Migrations
{
    public partial class updatePayCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidTill",
                table: "PayCards",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "character varying(6)",
                oldMaxLength: 6,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValidTill",
                table: "PayCards",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
