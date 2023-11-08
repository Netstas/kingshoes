using kingshoes.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
namespace kingshoes.Controllers
{
    public class NikeController : Controller
    {
        private KingshoesContext db = new KingshoesContext();
        // GET: Nike
        [Route("nike")]
        public ActionResult Index()
        {

            var dblist = db.Products.ToList();

            ViewBag.Products = dblist;
            return View();
        }

        public ActionResult Detail(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            Product products = db.Products.Find(id);
            ViewBag.products = products;
            return View();
        }
    }
}