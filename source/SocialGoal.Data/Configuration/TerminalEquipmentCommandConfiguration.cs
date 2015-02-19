using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{

    public class TerminalEquipmentCommandConfiguration : EntityTypeConfiguration<TerminalEquipmentCommand>
    {
        public TerminalEquipmentCommandConfiguration()
        {
            Property(g => g.TerminalEquipmentCommandId).IsRequired();
            Property(f => f.IMEI).HasMaxLength(50);

            Property(a => a.SerNum).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
        

        }
    }
}
