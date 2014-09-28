using SocialGoal.Core.Common;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialGoal.Controllers
{
    public class TerminalEquipmentController : Controller
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        private readonly ITerminalSimCardService _terminalSimCardService;
        public TerminalEquipmentController(ITerminalSimCardService terminalSimCardService, ITerminalEquipmentService terminalEquipmentService)
        {
            this._terminalSimCardService = terminalSimCardService;
            this._terminalEquipmentService = terminalEquipmentService;
        }
        // GET: TerminalEquipment
        public ActionResult Index()
        {
            return View();
        }
        public async Task<string> Get(string terminalEquipmentId)
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
        public async Task<JsonpResult> GetTerminalEquipment(int pageSize, int pageNum, string searchTerm)
        {
            Select2PagedResult orgEnterprises = await _terminalEquipmentService.GetSelect2PagedResult(searchTerm, pageSize, pageNum);
            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = orgEnterprises,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }
    }
}