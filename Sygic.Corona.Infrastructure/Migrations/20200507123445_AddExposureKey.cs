using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class AddExposureKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExposureKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false, defaultValueSql: "NEWID()"),
                    Data = table.Column<string>(nullable: true),
                    RollingStartNumber = table.Column<int>(nullable: false),
                    RollingDuration = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExposureKeys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExposureKeys");
        }
    }
}
