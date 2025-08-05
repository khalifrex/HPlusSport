using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HPlusSport.Client.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Product Name")]
        public string ProductName { get; set; } = default!;
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public int Qty { get; set; }
        
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation property for display
        public CategoryViewModel? Category { get; set; }
    }
}