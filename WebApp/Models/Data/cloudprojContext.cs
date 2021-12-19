using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CloudWebService.Models
{
    public partial class cloudprojContext : DbContext
    {
        public cloudprojContext()
        {
        }

        public cloudprojContext(DbContextOptions<cloudprojContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Device> Devices { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<GroupDeviceRelationship> GroupDeviceRelationships { get; set; } = null!;
        public virtual DbSet<Measurement> Measurements { get; set; } = null!;
        public virtual DbSet<SpotPrice> SpotPrices { get; set; } = null!;

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("device");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adress)
                    .HasMaxLength(50)
                    .HasColumnName("adress");

                entity.Property(e => e.Alias)
                    .HasMaxLength(50)
                    .HasColumnName("alias");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<GroupDeviceRelationship>(entity =>
            {
                entity.ToTable("group_device_relationship");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.GroupId).HasColumnName("group_id");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.GroupDeviceRelationships)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK__group_dev__devic__7E37BEF6");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupDeviceRelationships)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__group_dev__group__7D439ABD");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("measurements");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DeviceId).HasColumnName("device_id");

                entity.Property(e => e.Measurement1).HasColumnName("measurement");

                entity.Property(e => e.PriceId).HasColumnName("price_id");

                entity.Property(e => e.TimeStamp).HasColumnName("time_stamp");

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.DeviceId)
                    .HasConstraintName("FK__measureme__devic__02084FDA");

                entity.HasOne(d => d.Price)
                    .WithMany(p => p.Measurements)
                    .HasForeignKey(d => d.PriceId)
                    .HasConstraintName("FK__measureme__price__01142BA1");
            });

            modelBuilder.Entity<SpotPrice>(entity =>
            {
                entity.ToTable("spot_price");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.PriceArea)
                    .HasMaxLength(50)
                    .HasColumnName("price_area");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("datetime")
                    .HasColumnName("time_stamp");

                entity.Property(e => e.TimeStampDay)
                    .HasColumnType("date")
                    .HasColumnName("time_stamp_day");

                entity.Property(e => e.TimeStampHour).HasColumnName("time_stamp_hour");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
