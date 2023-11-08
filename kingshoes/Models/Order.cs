using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Mã đơn hàng")]
        public string CodeProduct { get; set; }

        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Địa chỉ")]
        public string City { get; set; }

        [Display(Name = "Địa chỉ")]
        public string District { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Ward { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string NameProduct { get; set; }

        [Display(Name = "Kích thước")]
        public string Size { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Số lượng")]
        public string Quantity { get; set; }

        public Order()
        {
            //CodeProduct = GenerateRandomCode();
        }

        
    }
}