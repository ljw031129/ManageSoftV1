using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using SocialGoal.Model.Models;

namespace SocialGoal.Models
{ 
    public class EquipmentRepository : IEquipmentRepository
    {
        SocialGoalWebContext context = new SocialGoalWebContext();

        public IQueryable<Equipment> All
        {
            get { return context.Equipment; }
        }

        public IQueryable<Equipment> AllIncluding(params Expression<Func<Equipment, object>>[] includeProperties)
        {
            IQueryable<Equipment> query = context.Equipment;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Equipment Find(string id)
        {
            return context.Equipment.Find(id);
        }

        public void InsertOrUpdate(Equipment equipment)
        {
            if (equipment.EquipmentId == default(string)) {
                // New entity
                context.Equipment.Add(equipment);
            } else {
                // Existing entity
                context.Entry(equipment).State = EntityState.Modified;
            }
        }

        public void Delete(string id)
        {
            var equipment = context.Equipment.Find(id);
            context.Equipment.Remove(equipment);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IEquipmentRepository : IDisposable
    {
        IQueryable<Equipment> All { get; }
        IQueryable<Equipment> AllIncluding(params Expression<Func<Equipment, object>>[] includeProperties);
        Equipment Find(string id);
        void InsertOrUpdate(Equipment equipment);
        void Delete(string id);
        void Save();
    }
}