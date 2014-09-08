using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFilter.Expressions;

namespace SocialGoal.Core.xFilter.Expressions
{
    public class JqGridSetting
    {
        public bool _search { get; set; }
        public string filters { get; set; }
        public int rows { get; set; }
        public int page { get; set; }
        public string sidx { get; set; }
        public string sord { get; set; }
        public string searchField { get; set; }
        public string searchOper { get; set; }
        public string searchString { get; set; }
        public Group Where { get; set; }
    }
}
