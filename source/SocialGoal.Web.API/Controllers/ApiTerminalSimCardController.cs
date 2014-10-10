using AutoMapper;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace SocialGoal.Web.API.Controllers
{
    /// <summary>
    /// SIM卡管理
    /// </summary>
    public class ApiTerminalSimCardController : ApiController
    {
        private readonly ITerminalSimCardService _terminalSimCardService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="terminalSimCardService"></param>
        public ApiTerminalSimCardController(ITerminalSimCardService terminalSimCardService)
        {
            this._terminalSimCardService = terminalSimCardService;
        }
        /// <summary>
        /// 获取SIM卡列表
        /// </summary>
        /// <param name="terminalEquipmentId"></param>
        /// <returns></returns>
        [Route("api/ApiTerminalSimCard/{terminalEquipmentId}")]
        public async Task<string> GetAll(string terminalEquipmentId)
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
        /// <summary>
        /// 获取SIM卡数据
        /// </summary>
        /// <param name="jqGridSetting"></param>
        /// <returns></returns>
        public async Task<object> Get([FromUri]JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalSimCard> orgStructure = await _terminalSimCardService.GeTerminalSimCard(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                userdata = "{totalinvoice:240.00}",
                rows = (from item in orgStructure
                        select new
                        {
                            TerminalSimCardId = item.TerminalSimCardId,
                            TerminalSimCardNum = item.TerminalSimCardNum,
                            TerminalSimCardSerialNum = item.TerminalSimCardSerialNum,
                            TerminalSimCardState = item.TerminalSimCardState,
                            TerminalSimCardUpdateTime = item.TerminalSimCardUpdateTime,
                            TerminalSimCardCreateTime = item.TerminalSimCardCreateTime
                        }).ToArray()
            };

            // var response = Request.CreateResponse(HttpStatusCode.Created, result);
            return result;
        }
        [HttpPost]
        public async Task<IHttpActionResult> Post(TerminalSimCardViewModel newTerminalSimCardViewModel)
        {
            if (ModelState.IsValid)
            {

                TerminalSimCard terminalSimCard = Mapper.Map<TerminalSimCardViewModel, TerminalSimCard>(newTerminalSimCardViewModel);
                switch (newTerminalSimCardViewModel.oper)
                {
                    case "add":
                        terminalSimCard.TerminalSimCardId = Guid.NewGuid().ToString();
                        terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                        terminalSimCard.TerminalSimCardCreateTime = DateTime.Now;
                        // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                        await _terminalSimCardService.CreateAsync(terminalSimCard);
                        return Ok();

                    case "edit":
                        terminalSimCard.TerminalSimCardUpdateTime = DateTime.Now;
                        await _terminalSimCardService.UpdateAsync(terminalSimCard);
                        return Ok();

                    case "del":
                        bool rec = await _terminalSimCardService.DeleteAsync(newTerminalSimCardViewModel.id);
                        if (rec)
                        {
                            return Ok();
                        }
                        break;

                }

            }
            //if (viewModel.ReturnStatus == true)
            //{
            //    var response = Request.CreateResponse<CustomerMaintenanceViewModel>
            //                   (HttpStatusCode.OK, viewModel);

            //    return response;
            //}

            //var badResponse = Request.CreateResponse<CustomerMaintenanceViewModel>
            //                  (HttpStatusCode.BadRequest, viewModel);
            // ModelState.AddModelErrors(errors);
            return BadRequest(ModelState);
        }
    }
}
