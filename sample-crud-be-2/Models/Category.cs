using static System.Reflection.Metadata.BlobBuilder;

namespace sample_crud_be_2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Priority { get; set; }
        public string? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

    }
}
