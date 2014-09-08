using System;
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
        IPagedList<Equipment> GetEquipments(string userId, GroupFilter filter, Page page);
        Task<bool> DeleteEquipmentAsync(string equipmentId);

        Task<bool> UpdateEquipmentAsync(Equipment equipment);

        Task<IPagedList<Equipment>> GetEquipmentsAsync(string gridSettings);

        IQueryable<Equipment> GetIQueryableAll();


        IEnumerable<Equipment> GetEquipmentsJqGrid(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count);
    }

    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EquipmentService(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork)
        {
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
                equipment = _equipmentRepository.Get(g => g.EquipmentName == newEquipment.EquipmentName);
            else
                equipment = _equipmentRepository.Get(g => g.EquipmentName == newEquipment.EquipmentName && g.EquipmentId != newEquipment.EquipmentId);
            if (equipment != null)
            {
                yield return new ValidationResult("EquipmentName", Resources.GroupExists);
            }
        }

        public IPagedList<Equipment> GetEquipments(string userId, GroupFilter filter, Page page)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEquipmentAsync(string id)
        {
            var equipment = _equipmentRepository.GetById(id);
            _equipmentRepository.Delete(equipment);
            _equipmentRepository.Delete(gu => gu.EquipmentId == id);
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


        public IEnumerable<Equipment> GetEquipmentsJqGrid(Core.xFilter.Expressions.JqGridSetting jqGridSetting, out int count)
        {
            IEnumerable<Equipment> equipments = _equipmentRepository.GetPageJqGrid<Equipment>(jqGridSetting, out count);
            return equipments;
        }
    }
}
