﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SocialGoal.Model.Models;
using SocialGoal.Models;
using SocialGoal.Data;
using SocialGoal.Core.xFilter.Expressions;
using System.Text;

namespace SocialGoal.Controllers
{
    // [Authorize(Roles = "Admin")]
    [Authorize]
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public ActionResult Index()
        {
            //await UserManager.Users.ToListAsync()
            return View();
        }
        public async Task<ActionResult> Get(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<ApplicationUser> applicationUser = await UserManager.Users.ToListAsync();
            
            count = applicationUser.Count();
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in applicationUser
                        select new
                        {
                            Id = item.Id,
                            UserName = item.UserName,
                            Email = item.Email,
                            OrgEnterpriseId = item.OrgEnterprise.OrgEnterpriseName,
                            Roles = GetUserRoles(item.Id),
                            DateCreated = item.DateCreated

                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string GetUserRoles(string id)
        {
            StringBuilder st = new StringBuilder();
            var roles = UserManager.GetRolesAsync(id);
            foreach (var item in roles.Result)
            {
                st.Append(item);
                st.Append(";");
            }
            return st.ToString(); ;
        }

        [HttpPost]
        public async Task<JsonResult> Post(AddUsersViewModel addUsersViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = addUsersViewModel.Email, Email = addUsersViewModel.Email, OrgEnterpriseId = addUsersViewModel.OrgEnterpriseId };

                switch (addUsersViewModel.oper)
                {
                    case "add":
                        var adminresult = await UserManager.CreateAsync(user, addUsersViewModel.Password);
                        if (!adminresult.Succeeded)
                        {
                            ModelState.AddModelError("", adminresult.Errors.First());
                        }
                        else
                        {
                            return Json(new { success = true });
                        }
                        break;
                    case "edit":
                        adminresult = await UserManager.CreateAsync(user, addUsersViewModel.Password);
                        break;

                    case "del":
                        adminresult = await UserManager.CreateAsync(user, addUsersViewModel.Password);
                        break;
                }
            }
           

            // 定义错误代码;
            HttpContext.Response.StatusCode = 400;
            return Json(new { success = false, errors = GetErrorsFromModelState() });
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.UserName, Email = userViewModel.Email, OrgEnterpriseId = userViewModel.OrgEnterpriseId };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View();

                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                OrgEnterpriseId = user.OrgEnterpriseId,
                OrgEnterpriseName = user.OrgEnterprise.OrgEnterpriseName,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,UserName,OrgEnterpriseId")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.UserName;
                user.Email = editUser.Email;
                user.OrgEnterpriseId = editUser.OrgEnterpriseId;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };

                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return Json("请选择要删除的用户？");
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return Json("当前用户无效！");
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // ModelState.AddModelError("", result.Errors.First());
                    return Json(result.Errors.First());
                }
                return Json("操作成功！");
            }
            return Json("信息验证失败！");
        }
    }
}
