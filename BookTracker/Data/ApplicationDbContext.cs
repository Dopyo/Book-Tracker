using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<BookAuthor> BookAuthors { get; set; } = null!;
    public DbSet<Patron> Patrons { get; set; } = null!;
    public DbSet<BorrowingRecord> BorrowingRecords { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Genre Configuration
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(g => g.GenreID);
            entity.Property(g => g.GenreName).IsRequired().HasMaxLength(100);
            entity.HasIndex(g => g.GenreName).IsUnique();
        });

        // Author Configuration
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(a => a.AuthorID);
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
        });

        // Book Configuration
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.BookID);
            entity.Property(b => b.ISBN).IsRequired().HasMaxLength(13);
            entity.HasIndex(b => b.ISBN).IsUnique();
            entity.Property(b => b.Title).IsRequired().HasMaxLength(255);
            entity.Property(b => b.DateAdded).HasDefaultValueSql("GETDATE()");
            entity.Property(b => b.TotalCopies).HasDefaultValue(1);
            entity.Property(b => b.AvailableCopies).HasDefaultValue(1);

            entity.HasOne(b => b.Genre)
                  .WithMany(g => g.Books)
                  .HasForeignKey(b => b.GenreID)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // BookAuthor
        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity.HasKey(ba => new { ba.BookID, ba.AuthorID }); // Composite Key

            entity.HasOne(ba => ba.Book)
                  .WithMany(b => b.BookAuthors)
                  .HasForeignKey(ba => ba.BookID)
                  .OnDelete(DeleteBehavior.Cascade); // If a book is deleted, remove its author links

            entity.HasOne(ba => ba.Author)
                  .WithMany(a => a.BookAuthors)
                  .HasForeignKey(ba => ba.AuthorID)
                  .OnDelete(DeleteBehavior.Cascade); // If an author is deleted, remove their book links
        });

        // Patron Configuration
        modelBuilder.Entity<Patron>(entity =>
        {
            entity.HasKey(p => p.PatronID);
            entity.Property(p => p.LibraryCardID).IsRequired().HasMaxLength(50);
            entity.HasIndex(p => p.LibraryCardID).IsUnique();
            entity.Property(p => p.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(p => p.Email).IsUnique();
            entity.Property(p => p.RegistrationDate).HasDefaultValueSql("GETDATE()");
            entity.Property(p => p.MembershipStatus).HasDefaultValue("Active").HasMaxLength(20);
        });

        // BorrowingRecord Configuration
        modelBuilder.Entity<BorrowingRecord>(entity =>
        {
            entity.HasKey(br => br.BorrowingRecordID);
            entity.Property(br => br.CheckoutDate).HasDefaultValueSql("GETDATE()");
            entity.Property(br => br.BorrowingStatus).HasDefaultValue("Borrowed").HasMaxLength(20);
            entity.Property(br => br.FineAmount).HasColumnType("decimal(10,2)").HasDefaultValue(0.00m);

            entity.HasOne(br => br.Book)
                  .WithMany(b => b.BorrowingRecords)
                  .HasForeignKey(br => br.BookID)
                  .OnDelete(DeleteBehavior.Restrict); // Important: Don't delete a book if it has borrowing history

            entity.HasOne(br => br.Patron)
                  .WithMany(p => p.BorrowingRecords)
                  .HasForeignKey(br => br.PatronID)
                  .OnDelete(DeleteBehavior.Restrict); // Important: Don't delete a patron if they have borrowing history
        });
    }
}