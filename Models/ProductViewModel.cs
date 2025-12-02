using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Моля, въведете име на продукта")]
        public string ?Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.1, 1000, ErrorMessage = "Цената трябва да е между 0.1 и 1000")]
        public decimal Price { get; set; }

        public string? UrlImg { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Категория")]
        public int CategoryId { get; set; }

        public CategoryViewModel ?Category { get; set; }
    }
}
