using Microsoft.EntityFrameworkCore;
using System;

namespace bcfamilyalbum_db
{
    public class FamilyAlbumDbContext : DbContext
    {
        const string Schema = "bcfamilyalbum";

        string _connectionString;

        public FamilyAlbumDbContext(string dbpath)
        {
            _connectionString = $"Filename = {dbpath}";
        }
        public DbSet<DeletedFileInfo> DeletedFiles { get; set; }
        public DbSet<MovedFileInfo> MovedFiles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString, options =>
            {
                options.MigrationsAssembly(typeof(FamilyAlbumDbContext).AssemblyQualifiedName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<DeletedFileInfo>().ToTable("DeletedFiles", Schema);
            modelBuilder.Entity<DeletedFileInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RelativePath).IsUnique();
                entity.Property(e => e.RemovalTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<MovedFileInfo>().ToTable("MovedFiles", Schema);
            modelBuilder.Entity<MovedFileInfo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RelativePath).IsUnique();
                entity.HasIndex(e => e.OriginalRelativePath).IsUnique();
                entity.Property(e => e.MovingTimestamp).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
