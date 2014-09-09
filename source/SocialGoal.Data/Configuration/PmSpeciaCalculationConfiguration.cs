using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{
    public class PmSpeciaCalculationConfiguration : EntityTypeConfiguration<PmSpeciaCalculation>
    {
        public PmSpeciaCalculationConfiguration()
        {
            Property(g => g.PmSpeciaCalculationId).IsRequired();

        }
    }
}
