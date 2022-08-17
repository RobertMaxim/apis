using System.ComponentModel.DataAnnotations;

namespace APIsEx.DTOs
{
    public class ProductDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        
        public int AvailableUnits { get; set; }
        [StringLength(50)]
        public string? Description { get; set; }
    }
}
