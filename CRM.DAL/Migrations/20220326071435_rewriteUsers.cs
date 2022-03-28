using Microsoft.EntityFrameworkCore.Migrations;

namespace CRM.DAL.Migrations
{
    public partial class rewriteUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Patronymic",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "VerifyCodes",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "VerifyCodes",
                newName: "Phone");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Patronymic",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
