using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class AddQuarantineAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuarantineAddress_City",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuarantineAddress_CountryCode",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuarantineAddress_Latitude",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QuarantineAddress_Longitude",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuarantineAddress_StreetName",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuarantineAddress_StreetNumber",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuarantineAddress_ZipCode",
                table: "Profiles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorderCrossedAt",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuarantineAddress_City",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_CountryCode",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_Latitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_Longitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_StreetName",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_StreetNumber",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "QuarantineAddress_ZipCode",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "BorderCrossedAt",
                table: "Profiles");
        }
    }
}
