using AutoMapper;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialGoal.Controllers
{
    public class ApiTerminalSimCardController : ApiController
    {
        private readonly ITerminalSimCardService _terminalSimCardService;
        public ApiTerminalSimCardController(ITerminalSimCardService terminalSimCardService)
        {
            this._terminalSimCardService = terminalSimCardService;
        }
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
        public async Task<Object> Get([FromUri]JqGridSetting jqGridSetting)
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
                            TerminalSimCardState=item.TerminalSimCardState,
                            TerminalSimCardUpdateTime = item.TerminalSimCardUpdateTime,
                            TerminalSimCardCreateTime = item.TerminalSimCardCreateTime
                        }).ToArray()
            };
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
