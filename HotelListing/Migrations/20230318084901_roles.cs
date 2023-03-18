using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Migrations
{
    /// <inheritdoc />
    public partial class roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "20e6a20e-5702-4591-b3e6-a1b0e3bcfac7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d18fde3-f082-4ac8-878a-399c553e2bb0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8af8dece-375e-4e27-b59a-ef4c5d9e1fbc", null, "user", "USER" },
                    { "bb39f2dd-d072-4a7c-8805-2bb856b626f2", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8af8dece-375e-4e27-b59a-ef4c5d9e1fbc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb39f2dd-d072-4a7c-8805-2bb856b626f2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "20e6a20e-5702-4591-b3e6-a1b0e3bcfac7", null, "admin", "ADMIN" },
                    { "9d18fde3-f082-4ac8-878a-399c553e2bb0", null, "user", "USER" }
                });
        }
    }
}
