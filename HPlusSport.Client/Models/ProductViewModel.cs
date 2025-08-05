using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HPlusSport.Client.Models
{
    public class ProductViewModel
    {
    public int Id { get; set; }
    [Required]
    [DisplayName("Parent Name")]
    public string ProductName { get; set; } = default!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Qty { get; set; }
    }
}
    