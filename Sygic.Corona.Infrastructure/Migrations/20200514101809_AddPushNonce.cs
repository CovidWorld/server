using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class AddPushNonce : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PushNonces",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ExpiredOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushNonces", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PushNonces_Id",
                table: "PushNonces",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PushNonces");
        }
    }
}
