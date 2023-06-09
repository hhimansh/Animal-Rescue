﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Animal_Rescue.Models
{
    public partial class Contact_ProjectContext : DbContext
    {
        public Contact_ProjectContext()
        {
        }

        public Contact_ProjectContext(DbContextOptions<Contact_ProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnimalRescue> AnimalRescue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnimalRescue>(entity =>
            {
                entity.Property(e => e.Adoption_fee).HasColumnName("Adoption fee");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Breed)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Colour)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Identification)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.PrefixedID)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasComputedColumnSql("('000'+right('0000'+CONVERT([varchar](8),[ID]),(8)))", true);

                entity.Property(e => e.Spayed)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Species)
                    .HasMaxLength(512)
                    .IsUnicode(false);

                entity.Property(e => e.Vaccine_Status)
                    .HasMaxLength(512)
                    .IsUnicode(false)
                    .HasColumnName("Vaccine Status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}