using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travelling.Services;

namespace Travelling.Web.Controllers
{
    public class HomeController : Controller
    {
        HotelService hotelService = new HotelService();
        public ActionResult Index()
        {
            //has some issues , can not locate the index file correctly , will fix later , you can 
            // get the data by calling Travelling.Data layer .
            var HotelList = hotelService.GetHotelDescriptions().Take(10);

            return View(HotelList);
        }

      
    }
}