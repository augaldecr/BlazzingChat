using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BlazzingChat.Server.Data
{
    public partial class BlazzingChatDbContext : DbContext
    {
        public BlazzingChatDbContext(DbContextOptions<BlazzingChatDbContext> options) : base(options) 
        {
        }

        public virtual DbSet<ChatHistory> ChatHistories { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Name=ConnectionStrings:BlazzingChatDbContext");
            }
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

        //    modelBuilder.Entity<ChatHistory>(entity =>
        //    {
        //        entity.ToTable("ChatHistory");

        //        entity.Property(e => e.CreatedDate).HasColumnType("date");

        //        entity.Property(e => e.Message)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.HasOne(d => d.FromUser)
        //            .WithMany(p => p.ChatHistoryFromUsers)
        //            .HasForeignKey(d => d.FromUserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_ChatHistory_Users");

        //        entity.HasOne(d => d.ToUser)
        //            .WithMany(p => p.ChatHistoryToUsers)
        //            .HasForeignKey(d => d.ToUserId)
        //            .OnDelete(DeleteBehavior.ClientSetNull)
        //            .HasConstraintName("FK_ChatHistory_Users1");
        //    });

        //    modelBuilder.Entity<Log>(entity =>
        //    {
        //        entity.Property(e => e.Id).HasColumnName("id");

        //        entity.Property(e => e.CreatedDate)
        //            .HasColumnType("text")
        //            .HasColumnName("CreatedDate");

        //        entity.Property(e => e.EventName)
        //            .HasColumnType("text")
        //            .HasColumnName("EventName");

        //        entity.Property(e => e.ExceptionMessage)
        //            .HasColumnType("text")
        //            .HasColumnName("ExceptionMessage");

        //        entity.Property(e => e.LogLevel)
        //            .HasColumnType("text")
        //            .HasColumnName("LogLevel");

        //        entity.Property(e => e.Source)
        //            .HasColumnType("text")
        //            .HasColumnName("\"Source\"");

        //        entity.Property(e => e.StackTrace)
        //            .HasColumnType("text")
        //            .HasColumnName("StackTrace");

        //        entity.Property(e => e.UserId).HasColumnName("UserId");
        //    });

        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.Property(e => e.AboutMe).HasMaxLength(50);

        //        entity.Property(e => e.Birthday).HasColumnType("datetime");

        //        entity.Property(e => e.CreationDate)
        //            .IsRowVersion()
        //            .IsConcurrencyToken();

        //        entity.Property(e => e.EmailAddress)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.Property(e => e.FirstName).HasMaxLength(50);

        //        entity.Property(e => e.LastName).HasMaxLength(50);

        //        entity.Property(e => e.Password)
        //            .IsRequired()
        //            .HasMaxLength(50);

        //        entity.Property(e => e.ProfilePictDataUrl)
        //            .HasColumnType("text")
        //            .HasColumnName("ProfilePictDataUrl");

        //        entity.Property(e => e.Role)
        //            .HasColumnType("text")
        //            .HasColumnName("Role");

        //        entity.Property(e => e.Source)
        //            .IsRequired()
        //            .HasMaxLength(50);
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
