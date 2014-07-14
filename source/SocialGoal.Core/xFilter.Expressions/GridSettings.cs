using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xFilter.Expressions;

namespace SocialGoal.Core.xFilter.Expressions
{
   public class GridSettings
    {
       public bool IsSearch { get; set; }
       public int PageSize { get; set; }
       public int PageIndex { get; set; }
       public string SortColumn { get; set; }
       public string SortOrder { get; set; }
       public  Group Where { get; set; }


    }
}
