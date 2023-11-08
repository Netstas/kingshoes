using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kingshoes.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public string Gender { get; set; }
        public string Code { get; set; }
        public string ListImage { get; set; }
        public string CategoryName { get; set; }
    }
}