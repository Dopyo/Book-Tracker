public class BookAuthor
{
    public int BookID { get; set; } // Composite Key Part 1, FK
    public Book Book { get; set; } = null!;

    public int AuthorID { get; set; } // Composite Key Part 2, FK
    public Author Author { get; set; } = null!;
}