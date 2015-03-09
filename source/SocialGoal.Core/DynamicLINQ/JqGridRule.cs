using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialGoal.Core.DynamicLINQ
{
    public class JqGridRule
    {
        public string field { get; set; }
        public Operations op { get; set; }
        public string data { get; set; }
    }
}
