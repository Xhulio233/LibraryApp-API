using System;
using System.Collections.Generic;

namespace sample_crud_be_2.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }
}
