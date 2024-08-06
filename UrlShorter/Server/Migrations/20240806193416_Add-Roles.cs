using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8192ed0c-24b7-4a19-b247-dc12e5d4adb7", "8192ed0c-24b7-4a19-b247-dc12e5d4adb7", "Admin", "ADMIN" },
                    { "e8f55c06-ff6e-4d1a-94a6-0dea9d9d8e5a", "e8f55c06-ff6e-4d1a-94a6-0dea9d9d8e5a", "Ordinary", "ORDINARY" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8192ed0c-24b7-4a19-b247-dc12e5d4adb7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e8f55c06-ff6e-4d1a-94a6-0dea9d9d8e5a");
        }
    }
}
