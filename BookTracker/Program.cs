using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Logging Middleware 
app.Use(async (context, next) =>
{
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Started.");
    await next(context);
    Console.WriteLine($"[{context.Request.Method} {context.Request.Path} {DateTime.UtcNow}] Finished.");
});

// POST /books
app.MapPost("/books", async (CreateBookDto bookDto, ApplicationDbContext db) =>
{
    if (await db.Books.AnyAsync(b => b.ISBN == bookDto.ISBN))
    {
        return Results.Conflict(new { message = $"Book with ISBN {bookDto.ISBN} already exists." });
    }

    var book = new Book
    {
        ISBN = bookDto.ISBN,
        Title = bookDto.Title,
        GenreID = bookDto.GenreId, // Assumes GenreId is valid or null
        PublicationYear = bookDto.PublicationYear,
        Publisher = bookDto.Publisher,
        NumberOfPages = bookDto.NumberOfPages,
        TotalCopies = bookDto.TotalCopies,
        AvailableCopies = bookDto.TotalCopies, // Initially all copies are available
        Edition = bookDto.Edition,
        CoverImageURL = bookDto.CoverImageURL,
        Description = bookDto.Description,
        DateAdded = DateTime.UtcNow
    };

    if (bookDto.AuthorIds != null && bookDto.AuthorIds.Any())
    {
        foreach (var authorId in bookDto.AuthorIds.Distinct()) // Ensure no duplicate author links
        {
            var author = await db.Authors.FindAsync(authorId);
            if (author != null)
            {
                book.BookAuthors.Add(new BookAuthor { Author = author });
            }
        }
    }

    db.Books.Add(book);
    await db.SaveChangesAsync();

    var createdBookAuthors = book.BookAuthors.Select(ba => ba.Author.FullName).ToList();
    var genreName = book.GenreID.HasValue ? (await db.Genres.FindAsync(book.GenreID.Value))?.GenreName : null;

    var responseDto = new BookDetailDto(
        book.BookID, book.ISBN, book.Title, genreName,
        createdBookAuthors, book.PublicationYear, book.Publisher, book.NumberOfPages,
        book.TotalCopies, book.AvailableCopies, book.Edition, book.CoverImageURL,
        book.Description, book.DateAdded
    );

    return Results.Created($"/books/{book.BookID}", responseDto);
})
.AddEndpointFilter(async (context, next) => // Validation Filter
{
    var bookDto = context.GetArgument<CreateBookDto>(0);
    var db = context.GetArgument<ApplicationDbContext>(1);
    var errors = new Dictionary<string, string[]>();

    if (string.IsNullOrWhiteSpace(bookDto.ISBN))
        errors.Add("ISBN", new[] { "ISBN is required." });
    else if (await db.Books.AnyAsync(b => b.ISBN == bookDto.ISBN)) // Check for existing ISBN
        errors.Add("ISBN", new[] { $"Book with ISBN {bookDto.ISBN} already exists." });


    if (string.IsNullOrWhiteSpace(bookDto.Title))
        errors.Add("Title", new[] { "Title is required." });

    if (bookDto.GenreId.HasValue && !await db.Genres.AnyAsync(g => g.GenreID == bookDto.GenreId.Value))
        errors.Add("GenreId", new[] { $"Genre with ID {bookDto.GenreId.Value} not found." });

    if (bookDto.AuthorIds != null && bookDto.AuthorIds.Any())
    {
        foreach (var authorId in bookDto.AuthorIds.Distinct())
        {
            if (!await db.Authors.AnyAsync(a => a.AuthorID == authorId))
            {
                errors.Add("AuthorIds", new[] { $"Author with ID {authorId} not found." });
            }
        }
    }
    if (bookDto.TotalCopies < 1)
        errors.Add("TotalCopies", new[] { "Total copies must be at least 1." });


    if (errors.Any())
    {
        return Results.ValidationProblem(errors);
    }

    return await next(context);
});

// GET /books
app.MapGet("/books", async (ApplicationDbContext db) =>
{
    var books = await db.Books
        .Include(b => b.Genre)
        .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
        .Select(b => new BookSummaryDto(
            b.BookID,
            b.Title,
            b.BookAuthors.Select(ba => ba.Author.FullName).ToList(),
            b.AvailableCopies
        ))
        .ToListAsync();
    return Results.Ok(books);
});

// GET /books/{id:int}
app.MapGet("/books/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var book = await db.Books
        .Include(b => b.Genre)
        .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
        .FirstOrDefaultAsync(b => b.BookID == id);

    if (book is null) return Results.NotFound();

    var bookDetail = new BookDetailDto(
        book.BookID, book.ISBN, book.Title, book.Genre?.GenreName,
        book.BookAuthors.Select(ba => ba.Author.FullName).ToList(),
        book.PublicationYear, book.Publisher, book.NumberOfPages,
        book.TotalCopies, book.AvailableCopies, book.Edition, book.CoverImageURL,
        book.Description, book.DateAdded
    );
    return Results.Ok(bookDetail);
});

// DELETE /books/{id:int}
app.MapDelete("/books/{id:int}", async (int id, ApplicationDbContext db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();

    if (await db.BorrowingRecords.AnyAsync(br => br.BookID == id))
    {
        return Results.Conflict(new { message = "Cannot delete book. It has borrowing records associated with it." });
    }

    db.Books.Remove(book);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();