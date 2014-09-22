using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{    
    public class TerminalSimCardConfiguration : EntityTypeConfiguration<TerminalSimCard>
    {
        public TerminalSimCardConfiguration()
        {
            Property(g => g.TerminalSimCardId).IsRequired();
        }
    }
}
