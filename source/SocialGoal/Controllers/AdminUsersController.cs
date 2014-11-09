using Microsoft.AspNet.Identity;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace SocialGoal.Controllers
{
    public class AdminUsersController : Controller
    {
        private UserManager<ApplicationUser> UserManager;
        private readonly IUserService _userService;
        public AdminUsersController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            this.UserManager = userManager;
            this._userService = userService;
        }
        // GET: AdminUsers
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Get(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<ApplicationUser> reD = await _userService.GeApplicationUser(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in reD
                        select new
                        {
                            TerminalSimCardId = item.Id,
                            TerminalSimCardNum = item.UserName,
                            TerminalSimCardSerialNum = item.Email
                          
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}