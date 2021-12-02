using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DiCentralProcess.API.Models
{
    public partial class DiCentralProcessDataContext : DbContext
    {
        public DiCentralProcessDataContext()
        {
        }

        public DiCentralProcessDataContext(DbContextOptions<DiCentralProcessDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<UserClient> UserClients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DiCentralProcessData");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("htd")
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Createdon).HasColumnType("datetime");

                entity.Property(e => e.Message1)
                    .HasMaxLength(500)
                    .HasColumnName("Message");

                entity.Property(e => e.UserIdFrom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserId_From");

                entity.Property(e => e.UserIdTo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("UserId_To");
            });

            modelBuilder.Entity<UserClient>(entity =>
            {
                entity.ToTable("UserClient", "dbo");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Psid)
                    .HasMaxLength(50)
                    .HasColumnName("PSID")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
