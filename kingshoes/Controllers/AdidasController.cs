using kingshoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kingshoes.Controllers
{
    public class AdidasController : Controller
    {
        private KingshoesContext db = new KingshoesContext();

        // GET: Adidas
        public ActionResult Index()
        {
            var dblist = db.Products.ToList();

            ViewBag.Products = dblist;
            return View();
        }
    }
}