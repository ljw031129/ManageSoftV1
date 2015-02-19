using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialGoal.Model.Models;

namespace SocialGoal.Data.Configuration
{
    public class EquipmentConfiguration : EntityTypeConfiguration<Equipment>
    {
        public EquipmentConfiguration()
        {
            Property(f => f.EquipmentId).HasMaxLength(36);
            Property(f => f.EquipmentName).HasMaxLength(50);
            Property(f => f.EquipmentNum).HasMaxLength(50);

        }
    }
}
