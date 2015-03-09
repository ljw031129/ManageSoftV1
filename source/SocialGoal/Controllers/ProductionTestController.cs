using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class ProductionTestController : Controller
    {
        // GET: ProductionTest
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReceiveData()
        {
            return View();
        }
        public ActionResult ReceiveDataHistory(string id)
        {
            ViewBag.IMEI = id;
            return View();
        }
    }
}