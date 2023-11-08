using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kingshoes.Models
{
    public class Ward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DistrictId { get; set; }
        public District District { get; set; }
    }
}