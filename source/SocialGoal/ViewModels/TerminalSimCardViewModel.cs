using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialGoal.ViewModels
{
    public class TerminalSimCardViewModel
    {
        public string TerminalSimCardId { get; set; }
        public string TerminalSimCardNum { get; set; }
        public string TerminalSimCardSerialNum { get; set; }
        public DateTime TerminalSimCardCreateTime { get; set; }
        public DateTime TerminalSimCardUpdateTime { get; set; }

        //del  edit  add
        public string oper { get; set; }
        public string id { get; set; }
    }
}