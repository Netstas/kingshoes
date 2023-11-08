using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using kingshoes.Models;
namespace kingshoes.Controllers
{
    public class HomeController : Controller
    {
        private KingshoesContext db = new KingshoesContext();
        public ActionResult Index()
        {
            var dbs = db.Products.ToList();

            return View();
        }
    }
}