using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class AdminRolesController : Controller
    {
        private RoleManager<IdentityRole> RoleManager;
        public AdminRolesController(RoleManager<IdentityRole> roleManager)
        {
            this.RoleManager = roleManager;
        }
        // GET: AdminRoles
        public ActionResult Index()
        {
            return View();
        }
    }
}