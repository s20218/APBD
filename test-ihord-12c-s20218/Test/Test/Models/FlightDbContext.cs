using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{

    public class FlightDbContext : DbContext
    {
        public FlightDbContext()
        { }

        public FlightDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Plane> Planes { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Flight_Passenger> Flight_Passengers { get; set; }
        public DbSet<CityDict> CityDicts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s20218;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<Plane>(entity =>
            {
                entity.HasKey(d => d.IdPlane)
                    .HasName("Plane_pk");

                entity.ToTable("Plane");

                entity.Property(p => p.IdPlane).ValueGeneratedOnAdd();

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasData(
                        new Plane { IdPlane= 1, Name = "Jet321",  MaxSeats = 150 },
                        new Plane { IdPlane = 2, Name = "Boeng456", MaxSeats = 250 }
                    );

            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(f => f.IdFlight)
                    .HasName("Flight_pk");

                entity.HasOne(f => f.IdCityDictNavigation)
                    .WithMany(c => c.Flights)
                    .HasForeignKey(f => f.IdCItyDict)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Flight_CityDict");

                entity.HasOne(f => f.IdPlaneNavigation)
                     .WithMany(c => c.Flights)
                     .HasForeignKey(f => f.IdPlane)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                     .HasConstraintName("Flight_Plane");

                entity.ToTable("Flight");

                entity.Property(f => f.IdFlight).ValueGeneratedOnAdd();

                entity.Property(f => f.FlightDate)
                    .IsRequired();

                entity.Property(f => f.Comment)
                    .HasMaxLength(200);

                entity.HasData(
                       new Flight { IdFlight = 1, FlightDate = DateTime.Now, Comment = "default", IdPlane = 1, IdCItyDict = 2 }
                   );
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(p => p.IdPassenger)
                    .HasName("Passenger_pk");

                entity.ToTable("Passenger");

                entity.Property(p => p.IdPassenger).ValueGeneratedOnAdd();

                entity.Property(p => p.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(p => p.LastName)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(p => p.PassportNum)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasData(
                       new Passenger { IdPassenger = 1, FirstName = "Jan", LastName = "Kowalski", PassportNum = "1234567890"},
                       new Passenger { IdPassenger = 2, FirstName = "Tom", LastName = "Reddle", PassportNum = "551662883" }
                   );
            });

            modelBuilder.Entity<Flight_Passenger>(entity =>
            {
                entity.HasKey(fp => new { fp.IdFlight, fp.IdPassenger })
                        .HasName("Flight_Passenger_pk");



                entity.ToTable("Flight_Passenger");

                entity.HasOne(fp => fp.IdFlightNavigation)
            .WithMany(m => m.Flight_Passengers)
            .HasForeignKey(fp => fp.IdFlight)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("Flight_Passenger_Flight");

                entity.HasOne(fp => fp.IdPassengerNavigation)
              .WithMany(m => m.Flight_Passengers)
              .HasForeignKey(fp => fp.IdPassenger)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("Flight_Passenger_Passenger");

                entity.HasData(
                       new Flight_Passenger { IdPassenger = 1, IdFlight = 1},
                       new Flight_Passenger { IdPassenger = 2, IdFlight = 1 }
                   );
            });


            modelBuilder.Entity<CityDict>(entity =>
            {
                entity.HasKey(cd => new
                {
                    cd.IdCityDict
                })
                   .HasName("CityDict_pk");

                entity.ToTable("CityDict");

                entity.Property(cd => cd.City)
                .IsRequired()
                .HasMaxLength(30);

                entity.HasData(
               new CityDict { IdCityDict = 1, City = "Warsaw"},
               new CityDict { IdCityDict = 2, City = "Berlin" }
           );
            });
        }
    }
}
