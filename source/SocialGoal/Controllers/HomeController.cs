using SocialGoal.Service;
using SocialGoal.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
       
        IUserService userService;
        

        public HomeController( IUserService userService)
        {           
            this.userService = userService;            
        }

        /// <summary>
        /// returns all notifications on first load
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index()     
        {  
            
            return View();
        }




        public ViewResult About()
        {
            //ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ViewResult Contact()
        {
            //ViewBag.Message = "Your quintessential contact page.";

            return View();
        }








    }
}