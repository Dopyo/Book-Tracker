public class BorrowingRecord
{
    public int BorrowingRecordID { get; set; }

    public int BookID { get; set; } // FK
    public Book Book { get; set; } = null!;

    public int PatronID { get; set; } // FK
    public Patron Patron { get; set; } = null!;

    public DateTime CheckoutDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string BorrowingStatus { get; set; } = "Borrowed";
    public decimal? FineAmount { get; set; }
    public string? Notes { get; set; }
}