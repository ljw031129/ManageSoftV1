using System;
using System.ComponentModel.DataAnnotations;

namespace SocialGoal.Model.ViewModels
{
    public class TerminalEquipmentViewModel
    {
        public string TerminalEquipmentId { get; set; }

        //终端编号
        [Required(ErrorMessage = "终端编号不能为空")]
        [Display(Name = "终端编号")]
        public string TerminalEquipmentNum { get; set; }

        //终端类型
        public string TerminalEquipmentType { get; set; }

        //协议
        public string PmFInterpreterId { get; set; }

        // public virtual PmFInterpreter PmFInterpreter { get; set; }
        //绑定卡号
        public string TerminalSimCardId { get; set; }

        // public virtual TerminalSimCard TerminalSimCard { get; set; }
        public string OrgEnterpriseId { get; set; }

        public DateTime TerminalEquipmentCreateTime { get; set; }

        public DateTime TerminalEquipmentUpdateTime { get; set; }

        public string ReceiveDataLastId { get; set; }

        public string EquipmentId { get; set; }

        public string oper { get; set; }

        public string id { get; set; }
    }
}