using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.Models
{
    public class TerminalSimCard
    {
        public string TerminalSimCardId { get; set; }
        public string TerminalSimCardNum { get; set; }
        //状态
        public string TerminalSimCardState { get; set; }
        //描述
        public string TerminalSimCardDescribe { get; set; }
        public string TerminalSimCardSerialNum { get; set; }
        public DateTime TerminalSimCardCreateTime { get; set; }
        public DateTime TerminalSimCardUpdateTime { get; set; }
    }
}
