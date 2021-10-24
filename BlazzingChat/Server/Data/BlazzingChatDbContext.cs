﻿using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BlazzingChat.Server.Data
{
    public partial class BlazzingChatDbContext : DbContext
    {
        public BlazzingChatDbContext() { }

        public BlazzingChatDbContext(DbContextOptions<BlazzingChatDbContext> options) : base(options) { }

        public virtual DbSet<ChatHistory> ChatHistories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:BlazzingChatDbContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<ChatHistory>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatHistory_Users");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatHistory_Users1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AboutMe).HasMaxLength(50);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.CreationDate)
                    .IsRowVersion()
                    .IsConcurrencyToken();

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ProfilePictDataUrl)
                    .HasColumnType("text")
                    .HasColumnName("profile_pict_data_url");

                entity.Property(e => e.ProfilePictureUrl)
                    .HasMaxLength(150)
                    .HasColumnName("ProfilePictureURL");

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
