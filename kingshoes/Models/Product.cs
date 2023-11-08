using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string Code { get; set; }
        [Display(Name = "Giá tiền")]
        public decimal Price { get; set; }
        [Display(Name = "Kích thước")]
        public string Size { get; set; }
        [Display(Name = "Danh sách ảnh")]
        public string ListImage { get; set; }
        [Display(Name = "Số lượng")]
        public int Number { get; set; }
        [Display(Name = "Mô tả sản phẩm")]
        public string Describe { get; set; }
        [Display(Name = "Danh mục sản phẩm")]
        public string Category { get; set; }

        [Display(Name = "Giới tính")]
        public string Gender { get; set; }

        public int CategoryId { get; set; }
        public Category ProductCategory { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public Product()
        {
        }

    }
}