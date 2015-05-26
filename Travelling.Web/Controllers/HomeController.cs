using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travelling.Data;
using Travelling.Model.DTO;

namespace Travelling.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DB_HotelHelper helper = new DB_HotelHelper();
            var HotelList = helper.GetHotelDescription().ToList<HotelDescriptionDTO>().Take(10);

            return View(HotelList);
        }

      
    }
}