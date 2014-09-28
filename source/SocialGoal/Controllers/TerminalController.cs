using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class TerminalController : Controller
    {

        private readonly IReceiveDataLastService _receiveDataLastService;
        public TerminalController(IReceiveDataLastService eceiveDataLastService)
        {
            this._receiveDataLastService = eceiveDataLastService;

        }
        // GET: Terminal
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Detail(string Id)
        {          
            ReceiveDataLast rdl = _receiveDataLastService.GetReceiveDataLastByTerminalNum(Id);
            return View(rdl);
        }
        public ActionResult Map()
        {
           
            return View();
        }
        public JsonResult GetreceiveDataLast(string terminalNum)
        {
            List<TerminalDataViewModel> tdLs = _receiveDataLastService.GetTerminalDataByTerminalNum(terminalNum);
            return Json(tdLs, JsonRequestBehavior.AllowGet);
        }
    }
}