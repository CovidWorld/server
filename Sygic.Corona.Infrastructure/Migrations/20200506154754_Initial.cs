using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<string>(nullable: false),
                    PushToken = table.Column<string>(nullable: true),
                    Locale = table.Column<string>(nullable: false),
                    AreaExit_Latitude = table.Column<double>(nullable: true),
                    AreaExit_Longitude = table.Column<double>(nullable: true),
                    AreaExit_Accuracy = table.Column<double>(nullable: true),
                    AreaExit_RecordDateUtc = table.Column<DateTime>(nullable: true),
                    AuthToken = table.Column<string>(nullable: true),
                    ConfirmedInfection = table.Column<bool>(nullable: false),
                    IsInQuarantine = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    MedicalId = table.Column<string>(nullable: true),
                    CovidPass = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    QuarantineBeginning = table.Column<DateTime>(nullable: true),
                    QuarantineEnd = table.Column<DateTime>(nullable: true),
                    LastPositionReportTime = table.Column<DateTime>(nullable: true),
                    LastInactivityNotificationSendTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DeviceId = table.Column<string>(nullable: true),
                    ProfileId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false),
                    SourceDeviceId = table.Column<string>(nullable: true),
                    SeenProfileId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    Timestamp = table.Column<int>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Accuracy = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProfileId = table.Column<long>(nullable: false),
                    Latitude = table.Column<double>(nullable: true),
                    Longitude = table.Column<double>(nullable: true),
                    Accuracy = table.Column<double>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_ProfileId",
                table: "Alerts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ProfileId",
                table: "Contacts",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ProfileId",
                table: "Locations",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
