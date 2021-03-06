﻿using AutoMapper;
using SocialGoal.Core.Common;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SocialGoal.Web.Core.Extensions;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class TerminalSimCardController : Controller
    {
        public TerminalSimCardController(ITerminalSimCardService terminalSimCardService)
        {
            this._terminalSimCardService = terminalSimCardService;
        }
        private readonly ITerminalSimCardService _terminalSimCardService;
        // GET: TerminalSimCard
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> GetAll()
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<TerminalSimCard> re = await _terminalSimCardService.GetAll();
            st.Append("<select>");
            foreach (var item in re)
            {
                st.Append("<option value='" + item.TerminalSimCardId + "'>" + item.TerminalSimCardNum + "</option>");

            }
            st.Append("</select>"); ;
            return st.ToString();
        }
        public async Task<string> GetAllByTerminalEquipment()
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<TerminalSimCard> re = await _terminalSimCardService.GetAllByTerminalEquipment();
            st.Append("<select>");
            foreach (var item in re)
            {
                st.Append("<option value='" + item.TerminalSimCardId + "'>" + item.TerminalSimCardNum + "</option>");

            }
            st.Append("</select>"); ;
            return st.ToString();
        }
        public async Task<ActionResult> Get(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalSimCard> orgStructure = await _terminalSimCardService.GeTerminalSimCard(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure
                        select new
                        {
                            TerminalSimCardId = item.TerminalSimCardId,
                            TerminalSimCardNum = item.TerminalSimCardNum,
                            TerminalSimCardSerialNum = item.TerminalSimCardSerialNum,
                            TerminalSimCardState = item.TerminalSimCardState,
                            TerminalSimCardDescribe = item.TerminalSimCardDescribe,
                            TerminalSimCardUserTime = item.TerminalSimCardUserTime,
                            TerminalSimCardUpdateTime = item.TerminalSimCardUpdateTime,
                            TerminalSimCardCreateTime = item.TerminalSimCardCreateTime
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> Post(TerminalSimCardViewModel newTerminalSimCardViewModel)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ValidationResult> errors = _terminalSimCardService.Validate(newTerminalSimCardViewModel);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    TerminalSimCard terminalSimCard = Mapper.Map<TerminalSimCardViewModel, TerminalSimCard>(newTerminalSimCardViewModel);

                    switch (newTerminalSimCardViewModel.oper)
                    {
                        case "add":
                            terminalSimCard.TerminalSimCardId = newTerminalSimCardViewModel.TerminalSimCardNum;
                            terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                            terminalSimCard.TerminalSimCardCreateTime = DateTime.Now;
                            // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                            await _terminalSimCardService.CreateAsync(terminalSimCard);
                            return Json(new { success = true });

                        case "edit":
                            terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                            await _terminalSimCardService.UpdateAsync(terminalSimCard);
                            return Json(new { success = true });

                        case "del":
                            bool rec = await _terminalSimCardService.DeleteAsync(newTerminalSimCardViewModel.id);
                            if (rec)
                            {
                                return Json(new { success = true });
                            }
                            break;
                    }
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
    }
}