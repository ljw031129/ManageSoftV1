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
using SocialGoal.CommandProcessor.Dispatcher;
using SocialGoal.Domain.Commands;
using SocialGoal.Data.Repository;
using SocialGoal.Core.Common;
using SocialGoal.Web.Core.Extensions;

namespace SocialGoal.Web.API.Controllers
{
    /// <summary>
    /// SIM卡管理
    /// </summary>
    public class ApiTerminalSimCardController : ApiController
    {
        private readonly ICommandBus commandBus;
        private readonly ITerminalSimCardRepository _terminalSimCardRepository;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandBus"></param>
        /// <param name="terminalSimCardRepository"></param>
        public ApiTerminalSimCardController(ICommandBus commandBus, ITerminalSimCardRepository terminalSimCardRepository)
        {
            this.commandBus = commandBus;
            this._terminalSimCardRepository = terminalSimCardRepository;
        }
        /// <summary>
        /// 获取SIM卡列表
        /// </summary>
        /// <param name="terminalEquipmentId"></param>
        /// <returns></returns>
        [Route("api/ApiTerminalSimCard/{terminalEquipmentId}")]
        public string GetAll(string terminalEquipmentId)
        {
            StringBuilder st = new StringBuilder();
            IEnumerable<TerminalSimCard> re = _terminalSimCardRepository.GetAll();
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
        public object Get([FromUri]JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalSimCard> orgStructure = _terminalSimCardRepository.GetPageJqGrid<TerminalSimCard>(jqGridSetting, out count);
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
        /// <summary>
        /// 执行增删改查操作
        /// </summary>
        /// <param name="sendForm"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post(TerminalSimCardViewModel sendForm)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateTerminalSimCardCommand command = Mapper.Map<TerminalSimCardViewModel, CreateOrUpdateTerminalSimCardCommand>(sendForm);
                //IEnumerable<ValidationResult> errors = commandBus.Validate(command);
               // ModelState.AddModelError("","sss");
               // ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    var result = commandBus.Submit(command);
                    if (result.Success)
                    {
                        var response = Request.CreateResponse(HttpStatusCode.Created, sendForm);
                        string uri = Url.Link("DefaultApi", new { id = sendForm.TerminalSimCardId });
                        response.Headers.Location = new Uri(uri);
                        return response;
                    }
                    else
                    {
                        ModelState.AddModelError("", "An unknown error occurred.");
                    }
                }
               
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

    }
}
