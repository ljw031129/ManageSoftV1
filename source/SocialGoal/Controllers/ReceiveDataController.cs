using ProtocolUtils.Lbs;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class ReceiveDataController : Controller
    {
     
        private readonly IReceiveDataService _receiveDataService;
        public ReceiveDataController(IReceiveDataService receiveDataService)
        {
            this._receiveDataService = receiveDataService;
        }
        // GET: ReceiveData
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetReceiveDataMap(string devId, string dateRange, int pageNum, int pageSize)
        {
            int count = 0;
            IEnumerable<ReceiveData> ReceiveDatas = await _receiveDataService.GetReceiveDataMapata(devId, dateRange, pageNum, pageSize, out count);
           

            var result = new
            {
                total = (int)Math.Ceiling((double)count / pageSize),
                page = pageNum,
                records = count,
                rows = (from item in ReceiveDatas.ToList()
                        select new
                        {
                            ReceiveTime = item.ReceiveTime,
                            GpsPlat = item.GpsPlat,
                            GpsPlog = item.GpsPlog,
                            GpsPos = item.GpsPos
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}