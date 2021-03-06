﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using SocialGoal.Core.Common;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Data.Repository;
using SocialGoal.Model.Models;
using SocialGoal.Service.Properties;

namespace SocialGoal.Service
{
    public interface IEquipmentService
    {
        IEnumerable<Equipment> GetEquipments();
        IEnumerable<Equipment> GetEquipments(IEnumerable<int> id);
        IEnumerable<Equipment> SearchEquipment(string equipment);
        Equipment GetEquipment(string id);
        Equipment CreateEquipment(Equipment equipment, string userId);
        Task<Equipment> CreateEquipmentAsync(Equipment equipment, string userId);
        void UpdateEquipment(Equipment equipment);
        void DeleteEquipment(string id);
        void SaveEquipment();
        IEnumerable<ValidationResult> CanAddEquipment(Equipment equipment);
        Task<bool> DeleteEquipmentAsync(string equipmentId);

        Task<bool> UpdateEquipmentAsync(Equipment equipment);

        Task<IPagedList<Equipment>> GetEquipmentsAsync(string gridSettings);

        IQueryable<Equipment> GetIQueryableAll();


        Task<IEnumerable<Equipment>> GetEquipmentsJqGrid(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);

        Task<Equipment> FindById(string id);

        List<string> GetCurrentUserEquipments(string[] al);

        Task<IEnumerable<Equipment>> GetEquipmentsJqGridByCurrentUser(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> st, out int count);
    }

    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly ITerminalEquipmentRepository _terminalEquipmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentService(IEquipmentRepository equipmentRepository, ITerminalEquipmentRepository terminalEquipmentRepository, IUnitOfWork unitOfWork)
        {
            this._terminalEquipmentRepository = terminalEquipmentRepository;
            this._equipmentRepository = equipmentRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Equipment> GetEquipments()
        {
            var equipments = _equipmentRepository.GetAll().OrderByDescending(g => g.EquipmentCreatTime);
            return equipments;
        }

        public IEnumerable<Equipment> GetEquipments(IEnumerable<string> id)
        {
            List<Equipment> equipments = new List<Equipment> { };
            foreach (string item in id)
            {
                var equipment = GetEquipment(item);
                if (!equipments.Contains(equipment))
                {
                    equipments.Add(equipment);
                }
            }
            return equipments;
        }

        public IEnumerable<Equipment> SearchEquipment(string equipment)
        {
            return _equipmentRepository.GetMany(g => g.EquipmentName.ToLower().Contains(equipment.ToLower())).OrderBy(g => g.EquipmentName);
        }

        public Equipment GetEquipment(string id)
        {
            var equipment = _equipmentRepository.Get(g => g.EquipmentId == id);
            return equipment;
        }

        public Equipment CreateEquipment(Equipment equipment, string userId)
        {
            _equipmentRepository.Add(equipment);
            SaveEquipment();
            return equipment;
        }

        public void UpdateEquipment(Equipment equipment)
        {
            _equipmentRepository.Update(equipment);
            SaveEquipment();
        }

        public void DeleteEquipment(string id)
        {
            var equipment = _equipmentRepository.GetById(id);
            _equipmentRepository.Delete(equipment);
            _equipmentRepository.Delete(gu => gu.EquipmentId == id);
            SaveEquipment();
        }

        public void SaveEquipment()
        {
            _unitOfWork.Commit();
        }
        /// <summary>
        /// 重复性验证
        /// </summary>
        /// <param name="newEquipment"></param>
        /// <returns></returns>

        public IEnumerable<ValidationResult> CanAddEquipment(Equipment newEquipment)
        {
            Equipment equipment;
            if (newEquipment.EquipmentId == "")
                equipment = _equipmentRepository.Get(g => g.EquipmentNum == newEquipment.EquipmentNum);
            else
                equipment = _equipmentRepository.Get(g => g.EquipmentNum == newEquipment.EquipmentNum && g.EquipmentId != newEquipment.EquipmentId);
            if (equipment != null)
            {
                yield return new ValidationResult("EquipmentNum", "车号已存在");
                //yield return new ValidationResult("EquipmentName", Resources.GroupExists);
            }
        }



        public Task<bool> DeleteEquipmentAsync(string id)
        {
            var equipment = _equipmentRepository.GetById(id);
            _equipmentRepository.Delete(equipment);
           // _equipmentRepository.Delete(gu => gu.EquipmentId == id);

            //取消当前设备绑定信息
            _terminalEquipmentRepository.UpdateSetTerminalEquipment(id);
            SaveEquipment();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateEquipmentAsync(Equipment equipment)
        {
            _equipmentRepository.Update(equipment);
            SaveEquipment();
            return Task.FromResult(true);
        }

        public Task<IPagedList<Equipment>> GetEquipmentsAsync(string gridSettings)
        {
            IPagedList<Equipment> ipagPagedList = _equipmentRepository.GetPage<Equipment>(gridSettings);
            return Task.FromResult(ipagPagedList);
        }


        public IEnumerable<Equipment> GetEquipments(IEnumerable<int> id)
        {
            throw new NotImplementedException();
        }




        public Task<Equipment> CreateEquipmentAsync(Equipment equipment, string userId)
        {
            _equipmentRepository.Add(equipment);
            SaveEquipment();
            return Task.FromResult(equipment);
        }


        public IQueryable<Equipment> GetIQueryableAll()
        {
            var equipment = _equipmentRepository.GetIQueryableAll();
            return equipment;
        }


        public Task<IEnumerable<Equipment>> GetEquipmentsJqGrid(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<Equipment> equipments = _equipmentRepository.GetPageJqGrid<Equipment>(jqGridSetting, out count);
            return Task.FromResult(equipments);
        }


        public Task<Equipment> FindById(string id)
        {
            Equipment te = _equipmentRepository.GetById(id);
            return Task.FromResult(te);
        }


        public List<string> GetCurrentUserEquipments(string[] orgIds)
        {
            IEnumerable<Equipment> te = _equipmentRepository.GetMany(m => orgIds.Contains(m.OrgEnterpriseId));
            List<string> al = new List<string>();
            foreach (var item in te.ToList())
            {
                al.Add(item.EquipmentId.Trim());
            }
            return al;
        }


        public Task<IEnumerable<Equipment>> GetEquipmentsJqGridByCurrentUser(Core.xFilter.Expressions.JqGridSetting jqGridSetting, List<string> st, out int count)
        {

            IEnumerable<Equipment> receiveDataLast = _equipmentRepository.GetEquipmentsJqGridByCurrentUser(jqGridSetting, st, out count);

            return Task.FromResult(receiveDataLast);
        }
    }
}
