using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Api.Migrations
{
    /// <inheritdoc />
    public partial class defaultRolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a816fee-5a64-4c96-8ad9-2284a8c2c996", "5ce95aee-8c69-4d07-ac1c-0d9e5e1228ae", "User", "USER" },
                    { "f6e0af36-bec7-4331-b554-5cd97cc146aa", "8be06358-39b9-4b96-a651-1344b74c2946", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a816fee-5a64-4c96-8ad9-2284a8c2c996");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6e0af36-bec7-4331-b554-5cd97cc146aa");
        }
    }
}
