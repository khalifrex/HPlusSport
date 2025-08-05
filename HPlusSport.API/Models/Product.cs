using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HPlusSport.API.Models;

public class Product
{
    public int Id { get; set; }
    [Required]
    public string ProductName { get; set; } = default!;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Qty { get; set; }
}