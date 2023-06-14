using Microsoft.AspNetCore.Http;

namespace sample_crud_be_2.Models

{
    public class BookCreateDto
    {
        public int AuthorId { get; set; }
        public List<int> CategoryIds { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
    }
}
