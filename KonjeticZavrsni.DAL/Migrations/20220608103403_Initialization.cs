using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KonjeticZavrsni.DAL.Migrations
{
    public partial class Initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Španjolska" },
                    { 2, "Monaco" },
                    { 3, "Njemačka" }
                });

            migrationBuilder.InsertData(
                table: "RaceTracks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Catalunya" },
                    { 2, "Circuit de Monaco" },
                    { 3, "Hockenheim" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RaceTracks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RaceTracks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RaceTracks",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
