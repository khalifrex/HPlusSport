using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HPlusSport.Client.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Category Name")]
        public string CategoryName { get; set; } = default!;
        
        [DisplayName("Description")]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public int ProductCount { get; set; }
    }
}