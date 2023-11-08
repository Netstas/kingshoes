using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Bạn chưa nhập danh mục")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}