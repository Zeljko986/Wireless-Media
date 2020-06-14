using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Products.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; } = 0;

        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "The data value is required")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "The data value is required")]
        public string Description { get; set; }

        public int CategoryProductId { get; set; }
        [Display(Name = "Category Product")]
        public virtual CategoryProduct CategoryProduct { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "The data value is required")]
        public string Manufacturer { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "The data value is required")]
        public string Supplier { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Price")]
        [Required(ErrorMessage = "The data value is required")]
        public string Price { get; set; }     
    }
}
