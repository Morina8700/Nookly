using BookMyHome.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMyHome.Persistence.Data
{
    public class BookMyHomeDbContext : DbContext
    {
        public BookMyHomeDbContext(
        DbContextOptions<BookMyHomeDbContext> options)
        : base(options)
        {
        }

        public DbSet<Booking> Bookings => Set<Booking>();

        public DbSet<Accommodation> Accommodations
            => Set<Accommodation>();
        public DbSet<Host> Hosts => Set<Host>();

        public DbSet<AccommodationImage> AccommodationImages
    => Set<AccommodationImage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .Property(b => b.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<User>()
                 .HasKey(u => u.UserId);

            modelBuilder.Entity<Accommodation>()
                .HasKey(a => a.AccommodationId);

            modelBuilder.Entity<Accommodation>()
                .Property(a => a.PricePerNight)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Accommodation>()
                .Property(a => a.FloorAreaSquareMeters)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.BookingId);

            modelBuilder.Entity<Accommodation>()
                .HasOne(a => a.Host)
                .WithMany(h => h.Accommodations)
                .HasForeignKey(a => a.HostId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Accommodation)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AccommodationId);

            modelBuilder.Entity<AccommodationImage>()
                .HasOne(i => i.Accommodation)
                .WithMany(a => a.Images)
                .HasForeignKey(i => i.AccommodationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Faste IDs til seed data
            var host1Id =
                Guid.Parse("33333333-3333-3333-3333-333333333333");

            var accommodation1Id =
                Guid.Parse("11111111-1111-1111-1111-111111111111");

            var accommodation2Id =
                Guid.Parse("22222222-2222-2222-2222-222222222222");

            var accommodation3Id =
                Guid.Parse("A09AF322-D431-4BE3-B01A-D7A444355088");

            var booking1Id =
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            var booking2Id =
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            var image1Id = Guid.Parse("10000000-0000-0000-0000-000000000001");
            var image2Id = Guid.Parse("10000000-0000-0000-0000-000000000002");
            var image3Id = Guid.Parse("10000000-0000-0000-0000-000000000003");
            var image4Id = Guid.Parse("10000000-0000-0000-0000-000000000004");
            var image5Id = Guid.Parse("10000000-0000-0000-0000-000000000005");
            var image6Id = Guid.Parse("10000000-0000-0000-0000-000000000006");
            var image7Id = Guid.Parse("10000000-0000-0000-0000-000000000007");
            var image8Id = Guid.Parse("10000000-0000-0000-0000-000000000008");


            // RELATION: Host -> Accommodations
            modelBuilder.Entity<Accommodation>()
                .HasOne(a => a.Host)
                .WithMany(h => h.Accommodations)
                .HasForeignKey(a => a.HostId);


            // RELATION: Accommodation -> Bookings
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Accommodation)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AccommodationId);


            // HOST
            modelBuilder.Entity<Host>().HasData(
                new
                {
                    UserId = host1Id,
                    Name = "Test Host",
                    Email = "host@bookmyhome.dk"
                }
            );


            // ACCOMMODATIONS
            modelBuilder.Entity<Accommodation>().HasData(
                new
                {
                    AccommodationId = accommodation1Id,
                    Name = "Beach House",
                    Address = "Beach Road 1",
                    Description = "Hyggeligt sommerhus tæt på stranden med stor terrasse, lys stue og moderne køkken.",
                    PricePerNight = 1200m,
                    HostId = host1Id,
                    FloorAreaSquareMeters = 92m,
                    Bedrooms = 3,
                    Bathrooms = 1,
                    MaxGuests = 6,
                    HasAirConditioning = false,
                    HasHeatedFloors = true,
                    HasWifi = true,
                    HasKitchen = true,
                    HasParking = true,
                    HasWasher = true,
                    HasTv = true,
                    HasBalcony = true,
                    HasPool = false,
                    PetsAllowed = true,
                    SmokingAllowed = false
                },

                new
                {
                    AccommodationId = accommodation2Id,
                    Name = "City Apartment",
                    Address = "Main Street 10",
                    Description = "",
                    PricePerNight = 850m,
                    HostId = host1Id,
                    FloorAreaSquareMeters = 58m,
                    Bedrooms = 2,
                    Bathrooms = 1,
                    MaxGuests = 4,
                    HasAirConditioning = true,
                    HasHeatedFloors = false,
                    HasWifi = true,
                    HasKitchen = true,
                    HasParking = false,
                    HasWasher = true,
                    HasTv = true,
                    HasBalcony = false,
                    HasPool = false,
                    PetsAllowed = false,
                    SmokingAllowed = false
                }
            );


            // BOOKINGS
            modelBuilder.Entity<Booking>().HasData(
                new
                {
                    BookingId = booking1Id,
                    StartDate = new DateOnly(2027, 6, 1),
                    EndDate = new DateOnly(2027, 6, 5),
                    AccommodationId = accommodation1Id
                },

                new
                {
                    BookingId = booking2Id,
                    StartDate = new DateOnly(2027, 7, 10),
                    EndDate = new DateOnly(2027, 7, 15),
                    AccommodationId = accommodation1Id
                }
            );

            modelBuilder.Entity<AccommodationImage>().HasData(
    new
    {
        AccommodationImageId = image1Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-facade.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image2Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-facade-terresse.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image3Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-inde-alrum.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image4Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-inde-stue.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image5Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-inde-stue-kurvesofa.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image6Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-inde-stue-sofa.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image7Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-inde-stue-udgang-terrasse.jpg",
        AccommodationId = accommodation3Id
    },
    new
    {
        AccommodationImageId = image8Id,
        ImageUrl = "/Images/SommerHus1/Nova-idyll-plan.png",
        AccommodationId = accommodation3Id
    }
);

        }

    }
}
