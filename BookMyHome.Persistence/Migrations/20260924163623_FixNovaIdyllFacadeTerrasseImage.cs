using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyHome.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixNovaIdyllFacadeTerrasseImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "ImageUrl",
                value: "/Images/SommerHus1/Nova-idyll-facade-terresse.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AccommodationImages",
                keyColumn: "AccommodationImageId",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"),
                column: "ImageUrl",
                value: "/Images/SommerHus1/Nova-idyll-facade-terrasse.jpg");
        }
    }
}
