using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookMyHome.Persistence.Migrations
{
    public partial class AddAndSeedAccommodationImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Removed seed images: their accommodation does not exist in a fresh database.
            // Keep the migration ID so existing databases retain their migration history.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}