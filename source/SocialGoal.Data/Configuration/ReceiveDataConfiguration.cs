using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{
    public class ReceiveDataConfiguration : EntityTypeConfiguration<ReceiveData>
    {
        public ReceiveDataConfiguration()
        {

            Property(g => g.ReceiveDataId).IsRequired();
            Property(f => f.IMEI).HasMaxLength(50);

        }
    }
}
