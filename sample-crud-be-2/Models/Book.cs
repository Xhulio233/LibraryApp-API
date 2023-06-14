namespace sample_crud_be_2.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public string? ImagePath { get; set; }

    }
}