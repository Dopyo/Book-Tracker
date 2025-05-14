public class Book
{
    public int BookID { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public int? GenreID { get; set; } // Foreign Key
    public Genre? Genre { get; set; } // Navigation property

    public int? PublicationYear { get; set; }
    public string? Publisher { get; set; }
    public int? NumberOfPages { get; set; }
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;
    public string? Edition { get; set; }
    public string? CoverImageURL { get; set; }
    public string? Description { get; set; }
    public DateTime DateAdded { get; set; }
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BorrowingRecord> BorrowingRecords { get; set; } = new List<BorrowingRecord>();
}