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

namespace SocialGoal.Controllers
{
    public class TerminalEquipmentController : Controller
    {
        private readonly ITerminalSimCardService _terminalSimCardService;
        public TerminalEquipmentController(ITerminalSimCardService terminalSimCardService)
        {
            this._terminalSimCardService = terminalSimCardService;
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
    }
}