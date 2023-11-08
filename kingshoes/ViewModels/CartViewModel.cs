using kingshoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kingshoes.ViewModels
{
    public class CartViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal TotalPrice { get; set; }
        public int CartId { get; set; }
        public string CartItem { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string ListImage { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }

    }
}