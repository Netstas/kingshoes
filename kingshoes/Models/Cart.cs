using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public string CartItem { get; set; }
        public string Size { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}