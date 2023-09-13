using Microsoft.EntityFrameworkCore;

namespace ArraySorts.Models;

public partial class ArraySortingContext : DbContext
{
    public ArraySortingContext()
    {
    }

    public ArraySortingContext(DbContextOptions<ArraySortingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Array> Arrays { get; set; }

    public virtual DbSet<Sorting> Sortings { get; set; }

    public virtual DbSet<SortingType> SortingTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ArraySorting;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Array>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Arrays_pk");
        });

        modelBuilder.Entity<Sorting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Sortings_pk");

            entity.Property(e => e.StartDate).HasColumnType("date");

            entity.HasOne(d => d.OriginalArray).WithMany(p => p.Sortings)
                .HasForeignKey(d => d.OriginalArrayId)
                .HasConstraintName("Sortings_Array_id_fk");

            entity.HasOne(d => d.Type).WithMany(p => p.Sortings)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("Sortings_SortingType_id_fk");
        });

        modelBuilder.Entity<SortingType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SortingTypes_pk");

            entity.Property(e => e.TypeName).HasColumnName("Type_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
