namespace BookFinderApi.Models
{
    public class FavoriteBook
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string BookId { get; set; } // Could be external ID
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}
