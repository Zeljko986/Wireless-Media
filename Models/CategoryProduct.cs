using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Models
{
    public class CategoryProduct
    {
        [Key]
        public int CategoryProductId { get; set; } = 0;

        [DataType(DataType.Text)]
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = "The data value is required")]
        public string CategoryName { get; set; }
    }
}
