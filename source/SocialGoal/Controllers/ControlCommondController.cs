using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class ControlCommondController : Controller
    {
        // GET: ControlCommond
        public ActionResult Index()
        {
            return View();
        }
    }
}