using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SiteCook;

public partial class CookingBookContext : DbContext
{
    public CookingBookContext()
    {
    }

    public CookingBookContext(DbContextOptions<CookingBookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Moderator> Moderators { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-VC8VVR7\\MSSQLSERVER01;Database=CookingBook;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.IdAdministrator).HasName("PK__Administ__27833EB9F96F14F8");

            entity.ToTable("Administrator");

            entity.Property(e => e.Mail).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PK__Category__CBD7470691635E21");

            entity.ToTable("Category");

            entity.Property(e => e.ImageCategory).HasColumnType("image");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.IdComment).HasName("PK__Comment__57C9AD58581E2147");

            entity.ToTable("Comment");

            entity.Property(e => e.DateComement).HasColumnType("datetime");

            entity.HasOne(d => d.IdRecipeNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdRecipe)
                .HasConstraintName("FK__Comment__IdRecip__48CFD27E");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Comments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__IdUser__47DBAE45");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.IdMeal).HasName("PK__Meal__4D7C3B3AEB916D3C");

            entity.ToTable("Meal");

            entity.Property(e => e.ImageMeal).HasColumnType("image");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Meals)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK__Meal__IdCategory__398D8EEE");
        });

        modelBuilder.Entity<Moderator>(entity =>
        {
            entity.HasKey(e => e.IdModerator).HasName("PK__Moderato__39AD9582440317B4");

            entity.ToTable("Moderator");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Mail).HasMaxLength(50);
            entity.Property(e => e.NikName).HasMaxLength(50);

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Moderators)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK__Moderator__IdCat__3E52440B");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.IdRecipe).HasName("PK__Recipe__2FEC16D4CB9C2900");

            entity.ToTable("Recipe");

            entity.Property(e => e.ImageRecipe).HasColumnType("image");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Recipe__IdCatego__440B1D61");

            entity.HasOne(d => d.IdMealNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdMeal)
                .HasConstraintName("FK__Recipe__IdMeal__44FF419A");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Recipe__IdUser__4316F928");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__Users__B7C92638D900C802");

            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Mail).HasMaxLength(50);
            entity.Property(e => e.NikName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
