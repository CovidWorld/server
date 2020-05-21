using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class AddSeverityToPresenceCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "PresenceChecks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Severity",
                table: "PresenceChecks");
        }
    }
}
