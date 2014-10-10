using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Web.API.Controllers
{
    public class ApiReceiveDataController : ApiController
    {
        private readonly IReceiveDataService _receiveDataService;
        public ApiReceiveDataController(IReceiveDataService receiveDataService)
        {
            this._receiveDataService = receiveDataService;
        }
        [Route("api/ApiReceiveData/GetReceiveDataMap")]
        public async Task<Object> GetReceiveDataMap([FromUri]string devId, [FromUri]string dateRange, [FromUri]int pageNum, [FromUri]int pageSize)
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
            return result;
        }
    }
}
