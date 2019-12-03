using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CarPoolingApp.DataModels
{
    public partial class carpoolingContext : DbContext
    {
        public carpoolingContext()
        {
        }

        public carpoolingContext(DbContextOptions<carpoolingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Administrator> Administrator { get; set; }
        public virtual DbSet<Authdetail> Authdetail { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Preferences> Preferences { get; set; }
        public virtual DbSet<Ride> Ride { get; set; }
        public virtual DbSet<Sessiondetail> Sessiondetail { get; set; }
        public virtual DbSet<Trip> Trip { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-GE0DKLV;Initial Catalog=car-pooling;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrator>(entity =>
            {
                entity.ToTable("administrator");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthId).HasColumnName("auth_id");

                entity.HasOne(d => d.Auth)
                    .WithMany(p => p.Administrator)
                    .HasForeignKey(d => d.AuthId)
                    .HasConstraintName("FK__administr__auth___4AB81AF0");
            });

            modelBuilder.Entity<Authdetail>(entity =>
            {
                entity.ToTable("authdetail");

                entity.HasIndex(e => e.Email)
                    .HasName("UQ__authdeta__AB6E6164B81BF454")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AuthId).HasColumnName("auth_id");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsDriver)
                    .HasColumnName("isDriver")
                    .HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.HasOne(d => d.Auth)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.AuthId)
                    .HasConstraintName("FK__client__auth_id__4D94879B");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("location");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lon).HasColumnName("lon");

                entity.Property(e => e.RideId).HasColumnName("ride_id");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Location)
                    .HasForeignKey(d => d.RideId)
                    .HasConstraintName("FK__location__ride_i__4E88ABD4");
            });

            modelBuilder.Entity<Preferences>(entity =>
            {
                entity.ToTable("preferences");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Gender)
                    .HasColumnName("gender")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Notification)
                    .HasColumnName("notification")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Preferences)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__preferenc__user___4F7CD00D");
            });

            modelBuilder.Entity<Ride>(entity =>
            {
                entity.ToTable("ride");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.EndLoc)
                    .IsRequired()
                    .HasColumnName("end_loc")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Fee).HasColumnName("fee");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.StartLoc)
                    .IsRequired()
                    .HasColumnName("start_loc")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnName("time");

                entity.Property(e => e.VehicleId).HasColumnName("vehicle_id");

                entity.HasOne(d => d.Vehicle)
                    .WithMany(p => p.Ride)
                    .HasForeignKey(d => d.VehicleId)
                    .HasConstraintName("FK__ride__vehicle_id__5070F446");
            });

            modelBuilder.Entity<Sessiondetail>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("PK__sessiond__69B13FDC3D81BBC5");

                entity.ToTable("sessiondetail");

                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AuthId).HasColumnName("auth_id");

                entity.Property(e => e.EndTime).HasColumnName("endTime");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.StartTime).HasColumnName("startTime");
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.ToTable("trip");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PassengerId).HasColumnName("passenger_id");

                entity.Property(e => e.RideId).HasColumnName("ride_id");

                entity.Property(e => e.Seats).HasColumnName("seats");

                entity.Property(e => e.Time).HasColumnName("time");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("FK__trip__passenger___52593CB8");

                entity.HasOne(d => d.Ride)
                    .WithMany(p => p.Trip)
                    .HasForeignKey(d => d.RideId)
                    .HasConstraintName("FK__trip__ride_id__534D60F1");
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.ToTable("vehicle");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.Model).HasColumnName("model");

                entity.Property(e => e.Plate)
                    .IsRequired()
                    .HasColumnName("plate")
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__vehicle__user_id__5441852A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
