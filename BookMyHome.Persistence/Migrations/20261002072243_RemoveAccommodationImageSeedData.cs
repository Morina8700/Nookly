using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookMyHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccommodationImageSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Do not restore invalid seed records: their parent accommodation may not exist.
        }
    }
}