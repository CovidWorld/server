using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class ChangeAreaExistToList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AreaExit_Accuracy",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AreaExit_Latitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AreaExit_Longitude",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AreaExit_RecordDateUtc",
                table: "Profiles");

            migrationBuilder.CreateTable(
                name: "AreaExits",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false),
                    Severity = table.Column<int>(nullable: false),
                    RecordDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaExits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaExits_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaExits_ProfileId",
                table: "AreaExits",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaExits");

            migrationBuilder.AddColumn<double>(
                name: "AreaExit_Accuracy",
                table: "Profiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AreaExit_Latitude",
                table: "Profiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AreaExit_Longitude",
                table: "Profiles",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AreaExit_RecordDateUtc",
                table: "Profiles",
                type: "datetime2",
                nullable: true);
        }
    }
}
