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
    public class TerminalEquipmentCommandCurrentConfiguration : EntityTypeConfiguration<TerminalEquipmentCommandCurrent>
    {
        public TerminalEquipmentCommandCurrentConfiguration()
        {
            Property(g => g.TerminalEquipmentCommandCurrentId).IsRequired();
            Property(f => f.IMEI).HasMaxLength(50);
            Property(a => a.SerNum).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            

        }
    }
}
