using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialGoal.Core.DynamicLINQ
{
    public class JqGridFilter
    {
        public GroupOp groupOp { get; set; }
        public List<JqGridRule> rules { get; set; }
        public List<JqGridFilter> groups { get; set; }
    }
}
