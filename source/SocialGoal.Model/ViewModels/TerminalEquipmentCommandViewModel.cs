using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.ViewModels
{
    public class TerminalEquipmentCommandViewModel
    {
        public string TerminalEquipmentCommandId { get; set; }
        public string IMEI { get; set; }
        public string OperateTime { get; set; }
        public string OperateDateTime { get; set; }
        //数据操作类型
        public string Dtype { get; set; }
        public string CommandJsonData { get; set; }
        public string ReceiveTData { get; set; }
        
        public string CommandFromTo { get; set; }
        public string OperateDataHex { get; set; }
        public string OperateStatue { get; set; }

        //用户ID--操作指令的用户
        public string UserName { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
