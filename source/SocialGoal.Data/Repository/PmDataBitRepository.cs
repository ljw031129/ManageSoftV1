using SocialGoal.Data.Infrastructure;
using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Repository
{
    public class PmDataBitRepository : RepositoryBase<PmDataBit>, IPmDataBitRepository
    {
        public PmDataBitRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool CanUpData(string p)
        {
            if (this.DataContext.PmDataBits.Where(pb=>pb.PmDataBitId==p).Count()>0)
            {
                return false;
            }
            else {
                return true;
            }
           
        }


        public void DeleteByte(string p)
        {
            this.DataContext.Database.ExecuteSqlCommand("update PmDataBodies set PmDataByteId=NULL where PmDataBodyId='"+p+"'");
            this.DataContext.Database.ExecuteSqlCommand("DELETE FROM PmDataBytes where PmDataByteId in (select top 1 PmDataByteId from PmDataBodies where PmDataBodyId='" + p + "')");
        }

        public void DeleteBit(string p)
        {

            this.DataContext.Database.ExecuteSqlCommand("DELETE FROM PmDataBits where PmDataBodyId='" + p + "'");
        }
    }
    public interface IPmDataBitRepository : IRepository<PmDataBit>
    {

        bool CanUpData(string p);

        void DeleteByte(string p);

        void DeleteBit(string p);
    }
}
