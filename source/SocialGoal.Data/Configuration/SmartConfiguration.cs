using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialGoal.Model.Models;

namespace SocialGoal.Data.Configuration
{
    public class SmartConfiguration : EntityTypeConfiguration<Smart>
    {
        public SmartConfiguration()
        {
            Property(f => f.SmartId).HasMaxLength(32);
            Property(f => f.SmartNum).HasMaxLength(8);
        }
    }
}
