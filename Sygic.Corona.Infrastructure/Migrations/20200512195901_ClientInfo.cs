using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class ClientInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientInfo_Integrator",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientInfo_Name",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientInfo_OperationSystem",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientInfo_Version",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientInfo_Integrator",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ClientInfo_Name",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ClientInfo_OperationSystem",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ClientInfo_Version",
                table: "Profiles");
        }
    }
}
