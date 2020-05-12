using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class AddDateTimesToExposureKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ExposureKeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Expiration",
                table: "ExposureKeys",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ExposureKeys");

            migrationBuilder.DropColumn(
                name: "Expiration",
                table: "ExposureKeys");
        }
    }
}
