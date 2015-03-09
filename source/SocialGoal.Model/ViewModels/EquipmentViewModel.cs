using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialGoal.Model.ViewModels
{
    public class EquipmentViewModel
    {
        public string EquipmentId { get; set; }
        //车牌号
        [Display(Name = "车号")]
        [Required]
        public string EquipmentNum { get; set; }
        //机器系列号
        [Display(Name = "车牌号")]
        public string EquipmentName { get; set; }
        //发动机号
        [Display(Name = "发动机号")]
        public string EngineNum { get; set; }
        //机主/车主信息
        [Display(Name = "车主姓名")]
        public string OwnerName { get; set; }
        [Display(Name = "车主联系电话")]
        public string OwnerPhone { get; set; }
        [Display(Name = "车主住址")]
        public string OwnerAddress { get; set; }
        /// <summary>
        /// /////安装信息
        /// </summary>
        [Display(Name = "安装时间")]
        public string InstallTime { get; set; }
        [Display(Name = "安装人")]
        public string InstallUser { get; set; }

        [Display(Name = "安装人联系电话")]
        public string InstallUserPhone { get; set; }
        [Display(Name = "安装位置")]
        public string InstallPlace { get; set; }
        [Display(Name = "安装地点")]
        public string InstallSite { get; set; }
        public string EquipmentTypeId { get; set; }

        public string TerminalEquipmentCount { get; set; }

        //所属企业ID   
        public string OrgEnterpriseId { get; set; }
        public string OrgEnterpriseIdSelect2 { get; set; }
        public string TerminalEquipmentIdSelect2 { get; set; }
        public string OrgEnterpriseName { get; set; }
        public DateTime EquipmentCreatTime { get; set; }
        public DateTime EquipmentUpDateTime { get; set; }
        //del  edit  add
        public string oper { get; set; }
        public string id { get; set; }
        public EquipmentViewModel()
        {
            //EquipmentUpDateTime = DateTime.Now;
            //EquipmentCreatTime = DateTime.Now;
            //  EquipmentId = Guid.NewGuid().ToString();
        }
    }
}