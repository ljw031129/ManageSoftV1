using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Monitor", "ReceiveData", null);

        }
      
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return PartialView();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult FullScreenMap()
        {


            return View();
        }
    }
}
