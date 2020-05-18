using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class ProfileQuarantine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInQuarantine",
                table: "Profiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInQuarantine",
                table: "Profiles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
