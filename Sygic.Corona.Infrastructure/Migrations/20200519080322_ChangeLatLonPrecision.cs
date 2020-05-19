using Microsoft.EntityFrameworkCore.Migrations;

namespace Sygic.Corona.Infrastructure.Migrations
{
    public partial class ChangeLatLonPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineAddress_Longitude",
                table: "Profiles",
                type: "decimal(11, 8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineAddress_Latitude",
                table: "Profiles",
                type: "decimal(11, 8)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineAddress_Longitude",
                table: "Profiles",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "QuarantineAddress_Latitude",
                table: "Profiles",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11, 8)",
                oldNullable: true);
        }
    }
}
