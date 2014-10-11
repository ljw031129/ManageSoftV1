using SocialGoal.CommandProcessor.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Domain.Commands
{
    public class CreateOrUpdateTerminalSimCardCommand : ICommand
    {
        public CreateOrUpdateTerminalSimCardCommand()
        {
        }
        public string TerminalSimCardId { get; set; }
        public string TerminalSimCardNum { get; set; }
        //状态
        public string TerminalSimCardState { get; set; }
        //描述
        public string TerminalSimCardDescribe { get; set; }
        public string TerminalSimCardSerialNum { get; set; }
        public DateTime TerminalSimCardCreateTime { get; set; }
        public DateTime TerminalSimCardUpdateTime { get; set; }
        //del  edit  add
        public string oper { get; set; }
        public string id { get; set; }
    }
}
