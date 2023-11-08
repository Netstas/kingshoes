using kingshoes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kingshoes.Controllers
{
    public class AdministrativeunitsController : Controller
    {
        private KingshoesContext db = new KingshoesContext();

        public JsonResult GetCities()
        {
            var cities = db.Cities.ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistricts(int cityId)
        {
            var districts = db.Districts.Where(d => d.CityId == cityId).ToList();
            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWards(int districtId)
        {
            var wards = db.Wards.Where(w => w.DistrictId == districtId).ToList();
            return Json(wards, JsonRequestBehavior.AllowGet);
        }
    }

}