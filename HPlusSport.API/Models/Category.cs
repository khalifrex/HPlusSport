using System.ComponentModel.DataAnnotations;

namespace HPlusSport.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
