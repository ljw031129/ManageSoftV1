using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class ReDataDisplayFormatRepository : RepositoryBase<ReDataDisplayFormat>, IReDataDisplayFormatRepository
    {
        public ReDataDisplayFormatRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool CanUpData(string p)
        {
            if (this.DataContext.ReDataDisplayFormats.Where(pb => pb.ReDataDisplayFormatId == p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void DeleteByDisplayId(string p)
        {

            this.DataContext.Database.ExecuteSqlCommand("DELETE FROM ReDataDisplayFormats where ReceiveDataDisplayId='" + p + "'");
        }
    }
    public interface IReDataDisplayFormatRepository : IRepository<ReDataDisplayFormat>
    {

        bool CanUpData(string p);

        void DeleteByDisplayId(string p);

    }
}
