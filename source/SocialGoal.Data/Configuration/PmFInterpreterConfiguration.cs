using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{
    public class PmFInterpreterConfiguration:EntityTypeConfiguration<PmFInterpreter>
    {
        public PmFInterpreterConfiguration()
        {
            Property(g => g.PmFInterpreterId).IsRequired();          
        }
    }
}
