using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAccommodationPropertyDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bathrooms",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Bedrooms",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "FloorAreaSquareMeters",
                table: "Accommodations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "HasAirConditioning",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasBalcony",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasHeatedFloors",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasKitchen",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasParking",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPool",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasTv",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWasher",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasWifi",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "MaxGuests",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "PetsAllowed",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SmokingAllowed",
                table: "Accommodations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Accommodations",
                keyColumn: "AccommodationId",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Bathrooms", "Bedrooms", "FloorAreaSquareMeters", "HasAirConditioning", "HasBalcony", "HasHeatedFloors", "HasKitchen", "HasParking", "HasPool", "HasTv", "HasWasher", "HasWifi", "MaxGuests", "PetsAllowed", "SmokingAllowed" },
                values: new object[] { 1, 3, 92m, false, true, true, true, true, false, true, true, true, 6, true, false });

            migrationBuilder.UpdateData(
                table: "Accommodations",
                keyColumn: "AccommodationId",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Bathrooms", "Bedrooms", "FloorAreaSquareMeters", "HasAirConditioning", "HasBalcony", "HasHeatedFloors", "HasKitchen", "HasParking", "HasPool", "HasTv", "HasWasher", "HasWifi", "MaxGuests", "PetsAllowed", "SmokingAllowed" },
                values: new object[] { 1, 2, 58m, true, false, false, true, false, false, true, true, true, 4, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bathrooms",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "Bedrooms",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "FloorAreaSquareMeters",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasAirConditioning",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasBalcony",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasHeatedFloors",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasKitchen",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasParking",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasPool",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasTv",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasWasher",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "HasWifi",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "MaxGuests",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "PetsAllowed",
                table: "Accommodations");

            migrationBuilder.DropColumn(
                name: "SmokingAllowed",
                table: "Accommodations");
        }
    }
}
