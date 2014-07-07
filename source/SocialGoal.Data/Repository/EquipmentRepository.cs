using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;

namespace SocialGoal.Data.Repository
{

    public class EquipmentRepository : RepositoryBase<Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
    public interface IEquipmentRepository : IRepository<Equipment>
    {
    }
}
