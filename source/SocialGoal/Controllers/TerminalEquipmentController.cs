using AutoMapper;
using SocialGoal.Core.Common;
using SocialGoal.Core.DynamicLINQ;
using SocialGoal.Core.xFilter.Expressions;
using SocialGoal.Model.Models;
using SocialGoal.Model.ViewModels;
using SocialGoal.Service;
using SocialGoal.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.Utilities;
using Microsoft.AspNet.Identity;

namespace SocialGoal.Controllers
{
    [Authorize]
    public class TerminalEquipmentController : Controller
    {
        private readonly ITerminalEquipmentService _terminalEquipmentService;
        private readonly ITerminalSimCardService _terminalSimCardService;
        private readonly IProtocolManageService _protocolManageService;
        private readonly IReceiveDataLastService _receiveDataLastService;
        private readonly IOrgEnterpriseService _orgEnterpriseService;
        public TerminalEquipmentController(IOrgEnterpriseService orgEnterpriseService, ITerminalSimCardService terminalSimCardService, ITerminalEquipmentService terminalEquipmentService, IProtocolManageService protocolManageService, IReceiveDataLastService eceiveDataLastService)
        {
            this._orgEnterpriseService = orgEnterpriseService;
            this._protocolManageService = protocolManageService;
            this._terminalSimCardService = terminalSimCardService;
            this._terminalEquipmentService = terminalEquipmentService;
            this._receiveDataLastService = eceiveDataLastService;
        }
        // GET: TerminalEquipment
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.TerminalEquipmentType = GetDropdownList();
            IEnumerable<TerminalSimCard> listT = await _terminalSimCardService.GetAllByTerminalEquipment();
            //Dropdownlist SIM卡号
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in listT.ToList())
            {
                list.Add(new SelectListItem { Value = item.TerminalSimCardId, Text = item.TerminalSimCardNum });
            }
            ViewBag.TerminalSimCardId = new SelectList(list, "Value", "Text", "");
            //协议类型
            IEnumerable<PmFInterpreter> listPm = _protocolManageService.GetPmFInterpreter();
            List<SelectListItem> listPmSelect = new List<SelectListItem>();
            foreach (var item in listPm.ToList())
            {
                listPmSelect.Add(new SelectListItem { Value = item.PmFInterpreterId, Text = item.ProtocolName });
            }
            ViewBag.PmFInterpreterId = new SelectList(listPmSelect, "Value", "Text", "");

            string userId = User.Identity.GetUserId();
            OrgEnterprise al = _orgEnterpriseService.GetOrgEnterpriseByUserId(userId);
            return View(new TerminalEquipmentViewModel()
            {
                OrgEnterpriseId = al.OrgEnterpriseId,
                OrgEnterpriseName = al.OrgEnterpriseName,
            });
        }
        [HttpPost]
        public async Task<ActionResult> Create(TerminalEquipmentViewModel newTerminalEquipment)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ValidationResult> errors = _terminalEquipmentService.Validate(newTerminalEquipment);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    TerminalEquipment terminalEquipment = Mapper.Map<TerminalEquipmentViewModel, TerminalEquipment>(newTerminalEquipment);
                    terminalEquipment.TerminalEquipmentId = Guid.NewGuid().ToString();
                    terminalEquipment.OrgEnterpriseId = newTerminalEquipment.OrgEnterpriseIdSelect2;
                    terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                    terminalEquipment.TerminalEquipmentCreateTime = DateTime.Now;
                    // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                    await _terminalEquipmentService.CreateAsync(terminalEquipment);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.TerminalEquipmentType = GetDropdownList();
            IEnumerable<TerminalSimCard> listT = await _terminalSimCardService.GetAllByTerminalEquipment();
            //Dropdownlist SIM卡号
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in listT.ToList())
            {
                list.Add(new SelectListItem { Value = item.TerminalSimCardId, Text = item.TerminalSimCardNum });
            }
            ViewBag.TerminalSimCardId = new SelectList(list, "Value", "Text", "");
            //协议类型
            IEnumerable<PmFInterpreter> listPm = _protocolManageService.GetPmFInterpreter();
            List<SelectListItem> listPmSelect = new List<SelectListItem>();
            foreach (var item in listPm.ToList())
            {
                listPmSelect.Add(new SelectListItem { Value = item.PmFInterpreterId, Text = item.ProtocolName });
            }
            ViewBag.PmFInterpreterId = new SelectList(listPmSelect, "Value", "Text", "");
            string userId = User.Identity.GetUserId();
            OrgEnterprise al = _orgEnterpriseService.GetOrgEnterpriseByUserId(userId);
            return View(new TerminalEquipmentViewModel()
            {
                OrgEnterpriseId = al.OrgEnterpriseId,
                OrgEnterpriseName = al.OrgEnterpriseName,
            });
        }
        public async Task<ActionResult> Edit(string id)
        {
            //Get the list of Roles
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var te = await _terminalEquipmentService.FindById(id);
            if (te == null)
            {
                return HttpNotFound();
            }
            ViewBag.TerminalEquipmentType = GetDropdownList(te.TerminalEquipmentType);
            IEnumerable<TerminalSimCard> listT = await _terminalSimCardService.GetAllByTerminalEquipment();
            //Dropdownlist SIM卡号
            List<SelectListItem> list = new List<SelectListItem>();
            if (list.Count() == 0)
            {
                list.Add(new SelectListItem { Value = te.TerminalSimCardId, Text = te.TerminalSimCardId });
            }
            else
            {
                foreach (var item in listT.ToList())
                {
                    list.Add(new SelectListItem { Value = item.TerminalSimCardId, Text = item.TerminalSimCardNum });
                }
            }
            ViewBag.TerminalSimCardId = new SelectList(list, "Value", "Text", te.TerminalSimCardId);
            //协议类型
            IEnumerable<PmFInterpreter> listPm = _protocolManageService.GetPmFInterpreter();
            List<SelectListItem> listPmSelect = new List<SelectListItem>();
            foreach (var item in listPm.ToList())
            {
                listPmSelect.Add(new SelectListItem { Value = item.PmFInterpreterId, Text = item.ProtocolName });
            }
            ViewBag.PmFInterpreterId = new SelectList(listPmSelect, "Value", "Text", te.PmFInterpreterId);
            return View(new TerminalEquipmentViewModel()
            {
                OrgEnterpriseId = te.OrgEnterpriseId,
                OrgEnterpriseIdSelect2 = te.OrgEnterpriseId,
                TerminalEquipmentId = te.TerminalEquipmentId,
                TerminalEquipmentNum = te.TerminalEquipmentNum,
                OrgEnterpriseName = te.OrgEnterprise.OrgEnterpriseName,
                TerminalEquipmentUpdateTime = te.TerminalEquipmentUpdateTime,
                TerminalEquipmentCreateTime = te.TerminalEquipmentCreateTime,
                ReceiveDataLastId = te.ReceiveDataLastId
            });

        }
        [HttpPost]
        public async Task<ActionResult> Edit(TerminalEquipmentViewModel newTerminalEquipment)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<ValidationResult> errors = _terminalEquipmentService.Validate(newTerminalEquipment);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    TerminalEquipment terminalEquipment = Mapper.Map<TerminalEquipmentViewModel, TerminalEquipment>(newTerminalEquipment);
                    terminalEquipment.OrgEnterpriseId = newTerminalEquipment.OrgEnterpriseIdSelect2;
                    terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                    await _terminalEquipmentService.UpdateAsync(terminalEquipment);
                    return RedirectToAction("Index");
                }
            }
            var te = await _terminalEquipmentService.FindById(newTerminalEquipment.TerminalEquipmentId);
            if (te == null)
            {
                return HttpNotFound();
            }
            ViewBag.TerminalEquipmentType = GetDropdownList(te.TerminalEquipmentType);
            IEnumerable<TerminalSimCard> listT = await _terminalSimCardService.GetAllByTerminalEquipment();
            //Dropdownlist SIM卡号
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in listT.ToList())
            {
                list.Add(new SelectListItem { Value = item.TerminalSimCardId, Text = item.TerminalSimCardNum });
            }
            ViewBag.TerminalSimCardId = new SelectList(list, "Value", "Text", te.TerminalSimCardId);
            //协议类型
            IEnumerable<PmFInterpreter> listPm = _protocolManageService.GetPmFInterpreter();
            List<SelectListItem> listPmSelect = new List<SelectListItem>();
            foreach (var item in listPm.ToList())
            {
                listPmSelect.Add(new SelectListItem { Value = item.PmFInterpreterId, Text = item.ProtocolName });
            }
            ViewBag.PmFInterpreterId = new SelectList(listPmSelect, "Value", "Text", te.PmFInterpreterId);
            return View(new TerminalEquipmentViewModel()
            {
                OrgEnterpriseId = te.OrgEnterpriseId,
                TerminalEquipmentId = te.TerminalEquipmentId,
                TerminalEquipmentNum = te.TerminalEquipmentNum,
                TerminalEquipmentUpdateTime = te.TerminalEquipmentUpdateTime,
                TerminalEquipmentCreateTime = te.TerminalEquipmentCreateTime,
                ReceiveDataLastId = te.ReceiveDataLastId
            });
        }

        public ActionResult Detail(string Id)
        {
            ViewBag.IMEI = Id;
            ReceiveDataLast rdl = _receiveDataLastService.GetReceiveDataLastByTerminalNum(Id);
            ReceiveDataLastViewModel rv = new ReceiveDataLastViewModel();
            rv.IMEI = rdl.IMEI;
            rv.AccStatus = rdl.AccStatus == "1" ? "开启" : "关闭";
            rv.Rtime = DateUtils.GetTime(rdl.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            rv.Ptime = DateUtils.GetTime(rdl.Ptime.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            rv.Lat = rdl.Lat;
            rv.Lng = rdl.Lng;
            rv.Pos = rdl.Pos;
            rv.IsPos = rdl.IsPos;
            return View(rv);
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
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<String> listS = new List<System.String>(al);
            Select2PagedResult orgEnterprises = await _terminalEquipmentService.GetSelect2PagedResult(listS, searchTerm, pageSize, pageNum);
            //Return the data as a jsonp result
            return new JsonpResult
            {
                Data = orgEnterprises,
                JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.AllowGet
            };
        }

        public async Task<ActionResult> GetList(JqSearchIn jqGridSetting)
        {
            int count = 0;
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<String> st = new List<System.String>(al);
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetTerminalEquipments(jqGridSetting, st, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            EquipmentId = item.Equipment != null ? item.Equipment.EquipmentId : null,
                            ReceiveDataLastId = item.ReceiveDataLast != null ? item.ReceiveDataLast.ReceiveDataLastId : null,
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime

                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ProductionTestGetList(JqSearchIn jqGridSetting)
        {
            int count = 0;
            string userId = User.Identity.GetUserId();
            string[] al = await _orgEnterpriseService.GetOrgEnterpriseArraylist(userId);
            List<String> st = new List<System.String>(al);
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetTerminalEquipments(jqGridSetting, st, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            EquipmentId = item.Equipment != null ? item.Equipment.EquipmentId : null,
                            ReceiveDataLastId = item.ReceiveDataLast != null ? item.ReceiveDataLast.ReceiveDataLastId : null,
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime,
                            //定位信息
                            Pos = item.ReceiveDataLast != null && item.ReceiveDataLast.Pos!=null ? item.ReceiveDataLast.Pos.ToString() : "",
                            Lat = item.ReceiveDataLast != null && item.ReceiveDataLast.Lat != null ? item.ReceiveDataLast.Lat.ToString() : "",
                            Lng = item.ReceiveDataLast != null && item.ReceiveDataLast.Lng != null ? item.ReceiveDataLast.Lng.ToString() : "",
                            IsPos = item.ReceiveDataLast != null && item.ReceiveDataLast.IsPos != null ? item.ReceiveDataLast.IsPos.ToString() : "",
                            LAC = item.ReceiveDataLast != null && item.ReceiveDataLast.LAC != null ? item.ReceiveDataLast.LAC.ToString() : "",
                            CID = item.ReceiveDataLast != null && item.ReceiveDataLast.CID != null ? item.ReceiveDataLast.CID.ToString() : "",
                            ProtocolVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.ProtocolVersion != null ? item.ReceiveDataLast.ProtocolVersion.ToString() : "",
                            SoftVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.SoftVersion != null ? item.ReceiveDataLast.SoftVersion.ToString() : "",
                            HardwareVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.HardwareVersion != null ? item.ReceiveDataLast.HardwareVersion.ToString() : "",


                            BatteryVoltage = item.ReceiveDataLast != null && item.ReceiveDataLast.BatteryVoltage != null ? item.ReceiveDataLast.BatteryVoltage.ToString() : "",
                            GsmSignal = item.ReceiveDataLast != null && item.ReceiveDataLast.GsmSignal != null ? item.ReceiveDataLast.GsmSignal.ToString() : "",
                            BlindSign = item.ReceiveDataLast != null && item.ReceiveDataLast.BlindSign != null ? item.ReceiveDataLast.BlindSign.ToString() : "",
                            BlindDataCount = item.ReceiveDataLast != null && item.ReceiveDataLast.BlindDataCount != null ? item.ReceiveDataLast.BlindDataCount.ToString() : "",
                            SeeSatelliteCount = item.ReceiveDataLast != null && item.ReceiveDataLast.SeeSatelliteCount != null ? item.ReceiveDataLast.SeeSatelliteCount.ToString() : "",
                            UseSatelliteCount = item.ReceiveDataLast != null && item.ReceiveDataLast.UseSatelliteCount != null ? item.ReceiveDataLast.UseSatelliteCount.ToString() : "",


                            WorkStatue = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkStatue != null ? item.ReceiveDataLast.WorkStatue.ToString() : "",
                            WorkModel = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkModel != null ? item.ReceiveDataLast.WorkModel.ToString() : "",
                            WorkTime = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkTime != null ? item.ReceiveDataLast.WorkTime.ToString() : "",
                            SleepTime = item.ReceiveDataLast != null && item.ReceiveDataLast.SleepTime != null ? item.ReceiveDataLast.SleepTime.ToString() : "",
                            IntervalTime = item.ReceiveDataLast != null && item.ReceiveDataLast.IntervalTime != null ? item.ReceiveDataLast.IntervalTime.ToString() : "",
                            TootalWorkTime = item.ReceiveDataLast != null && item.ReceiveDataLast.TootalWorkTime != null ? item.ReceiveDataLast.TootalWorkTime.ToString() : "",
                            StartCount = item.ReceiveDataLast != null && item.ReceiveDataLast.StartCount != null ? item.ReceiveDataLast.StartCount.ToString() : "",


                            Ptime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Ptime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",
                            Rtime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",

                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> ProductionTestGetHistoryList(JqSearchIn jqGridSetting, string terminalEquipmentNum)
        {
            int count = 0;

            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetTerminalEquipmentDataHistory(terminalEquipmentNum, jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            EquipmentId = item.Equipment != null ? item.Equipment.EquipmentId : null,
                            ReceiveDataLastId = item.ReceiveDataLast != null ? item.ReceiveDataLast.ReceiveDataLastId : null,
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime,
                            //定位信息
                            Pos = item.ReceiveDataLast != null && item.ReceiveDataLast.Pos != null ? item.ReceiveDataLast.Pos.ToString() : "",
                            Lat = item.ReceiveDataLast != null && item.ReceiveDataLast.Lat != null ? item.ReceiveDataLast.Lat.ToString() : "",
                            Lng = item.ReceiveDataLast != null && item.ReceiveDataLast.Lng != null ? item.ReceiveDataLast.Lng.ToString() : "",
                            IsPos = item.ReceiveDataLast != null && item.ReceiveDataLast.IsPos != null ? item.ReceiveDataLast.IsPos.ToString() : "",
                            LAC = item.ReceiveDataLast != null && item.ReceiveDataLast.LAC != null ? item.ReceiveDataLast.LAC.ToString() : "",
                            CID = item.ReceiveDataLast != null && item.ReceiveDataLast.CID != null ? item.ReceiveDataLast.CID.ToString() : "",
                            ProtocolVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.ProtocolVersion != null ? item.ReceiveDataLast.ProtocolVersion.ToString() : "",
                            SoftVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.SoftVersion != null ? item.ReceiveDataLast.SoftVersion.ToString() : "",
                            HardwareVersion = item.ReceiveDataLast != null && item.ReceiveDataLast.HardwareVersion != null ? item.ReceiveDataLast.HardwareVersion.ToString() : "",


                            BatteryVoltage = item.ReceiveDataLast != null && item.ReceiveDataLast.BatteryVoltage != null ? item.ReceiveDataLast.BatteryVoltage.ToString() : "",
                            GsmSignal = item.ReceiveDataLast != null && item.ReceiveDataLast.GsmSignal != null ? item.ReceiveDataLast.GsmSignal.ToString() : "",
                            BlindSign = item.ReceiveDataLast != null && item.ReceiveDataLast.BlindSign != null ? item.ReceiveDataLast.BlindSign.ToString() : "",
                            BlindDataCount = item.ReceiveDataLast != null && item.ReceiveDataLast.BlindDataCount != null ? item.ReceiveDataLast.BlindDataCount.ToString() : "",
                            SeeSatelliteCount = item.ReceiveDataLast != null && item.ReceiveDataLast.SeeSatelliteCount != null ? item.ReceiveDataLast.SeeSatelliteCount.ToString() : "",
                            UseSatelliteCount = item.ReceiveDataLast != null && item.ReceiveDataLast.UseSatelliteCount != null ? item.ReceiveDataLast.UseSatelliteCount.ToString() : "",


                            WorkStatue = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkStatue != null ? item.ReceiveDataLast.WorkStatue.ToString() : "",
                            WorkModel = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkModel != null ? item.ReceiveDataLast.WorkModel.ToString() : "",
                            WorkTime = item.ReceiveDataLast != null && item.ReceiveDataLast.WorkTime != null ? item.ReceiveDataLast.WorkTime.ToString() : "",
                            SleepTime = item.ReceiveDataLast != null && item.ReceiveDataLast.SleepTime != null ? item.ReceiveDataLast.SleepTime.ToString() : "",
                            IntervalTime = item.ReceiveDataLast != null && item.ReceiveDataLast.IntervalTime != null ? item.ReceiveDataLast.IntervalTime.ToString() : "",
                            TootalWorkTime = item.ReceiveDataLast != null && item.ReceiveDataLast.TootalWorkTime != null ? item.ReceiveDataLast.TootalWorkTime.ToString() : "",
                            StartCount = item.ReceiveDataLast != null && item.ReceiveDataLast.StartCount != null ? item.ReceiveDataLast.StartCount.ToString() : "",


                            Ptime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Ptime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",
                            Rtime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",

                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> GetSubGrid(JqGridSetting jqGridSetting)
        {
            int count = 0;
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetSubGridByEquipmentId(jqGridSetting, out count);
            var result = new
            {
                total = (int)Math.Ceiling((double)count / jqGridSetting.rows),
                page = jqGridSetting.page,
                records = count,
                rows = (from item in orgStructure.ToList()
                        select new
                        {
                            TerminalEquipmentId = item.TerminalEquipmentId,
                            TerminalEquipmentNum = item.TerminalEquipmentNum,
                            TerminalEquipmentType = item.TerminalEquipmentType,
                            OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                            PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                            // TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                            TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                            TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> GetTerminalEquipmentById(string id)
        {
            IEnumerable<TerminalEquipment> orgStructure = await _terminalEquipmentService.GetSubGridByEquipmentId(id);

            var result = new
            {
                aaData = (from item in orgStructure.ToList()
                          select new
                          {
                              // TerminalEquipmentId = item.TerminalEquipmentId,
                              TerminalEquipmentNum = item.TerminalEquipmentNum,
                              TerminalEquipmentType = item.TerminalEquipmentType,
                              OrgEnterpriseId = !string.IsNullOrWhiteSpace(item.OrgEnterprise.ToString()) ? item.OrgEnterprise.OrgEnterpriseName : "",
                              //PmFInterpreterId = item.PmFInterpreter.ProtocolName,
                              TerminalSimCardId = item.TerminalSimCard.TerminalSimCardNum,
                              TerminalEquipmentCreateTime = item.TerminalEquipmentCreateTime,
                              TerminalEquipmentUpdateTime = item.TerminalEquipmentUpdateTime,
                              //最新信息
                              GpsPos = item.ReceiveDataLast != null ? item.ReceiveDataLast.Pos.ToString() : "",
                              GpsPlat = item.ReceiveDataLast != null ? item.ReceiveDataLast.Lat.ToString() : "",
                              GpsPlog = item.ReceiveDataLast != null ? item.ReceiveDataLast.Lng.ToString() : "",
                              GpsSpeed = item.ReceiveDataLast != null ? item.ReceiveDataLast.Gspeed.ToString() : "",
                              GpsDirection = item.ReceiveDataLast != null ? item.ReceiveDataLast.GsDirection.ToString() : "",
                              GpsTime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Ptime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",
                              ReceiveTime = item.ReceiveDataLast != null ? DateUtils.GetTime(item.ReceiveDataLast.Rtime.ToString()).ToString("yyyy-MM-dd HH:mm:ss") : "",
                          }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public string UpdateEquipmentId(string TerminalEquipmentId, string EquipmentIds)
        {
            _terminalEquipmentService.UpdateEquipmentId(TerminalEquipmentId, EquipmentIds);
            return "true";
        }

        [HttpPost]
        public string UpdateOrgEnterprise(string OrgEnterpriseId, string TerminalEquipmentIds)
        {
            _terminalEquipmentService.UpdateTerminalEquipmentOrgEnterpriseId(OrgEnterpriseId, TerminalEquipmentIds);
            return "true";
        }
        [HttpPost]
        public string UpdateterminalEquipmentByEquipmentId(string TerminalEquipmentId)
        {
            _terminalEquipmentService.UpdateEquipmentId(TerminalEquipmentId);
            return "true";
        }
        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> Post(TerminalEquipmentViewModel newTerminalEquipment)
        {
            if (newTerminalEquipment.oper == "del")
            {
                bool rec = await _terminalEquipmentService.DeleteAsync(newTerminalEquipment.id);
                if (rec)
                {
                    return Json(new { success = true });
                }
            }
            if (ModelState.IsValid)
            {
                IEnumerable<ValidationResult> errors = _terminalEquipmentService.Validate(newTerminalEquipment);
                ModelState.AddModelErrors(errors);
                if (ModelState.IsValid)
                {
                    TerminalEquipment terminalEquipment = Mapper.Map<TerminalEquipmentViewModel, TerminalEquipment>(newTerminalEquipment);
                    switch (newTerminalEquipment.oper)
                    {
                        case "add":
                            terminalEquipment.TerminalEquipmentId = Guid.NewGuid().ToString();
                            terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                            terminalEquipment.TerminalEquipmentCreateTime = DateTime.Now;
                            // var errors = _orgEnterpriseService.CanAdd(equipment).ToList();
                            await _terminalEquipmentService.CreateAsync(terminalEquipment);
                            return Json(new { success = true });

                        case "edit":
                            terminalEquipment.TerminalEquipmentUpdateTime = DateTime.Now;
                            await _terminalEquipmentService.UpdateAsync(terminalEquipment);
                            return Json(new { success = true });

                    }
                }
            }
            HttpContext.Response.StatusCode = 400;
            return Json(new { success = false, errors = GetErrorsFromModelState() });

        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        public static SelectList GetDropdownList(string currSelection = "1")
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Value = "1", Text = "无线GPS" });
            list.Add(new SelectListItem { Value = "2", Text = "有线GPS" });
            list.Add(new SelectListItem { Value = "3", Text = "代理GPS" });
            return new SelectList(list, "Value", "Text", currSelection);
        }



    }
}