﻿using SocialGoal.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Data.Configuration
{
    public class ReceiveDataLastConfiguration : EntityTypeConfiguration<ReceiveDataLast>
    {
        public ReceiveDataLastConfiguration()
        {

            Property(g => g.ReceiveDataLastId).IsRequired();
            Property(f => f.IMEI).HasMaxLength(50);

        }
    }
}