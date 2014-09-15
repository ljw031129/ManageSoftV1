using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.ViewModels
{
    public class TerminalDataViewModel
    {
        //终端编号
        public string TerminalNum { get; set; }
        //显示KEY
        public string DictionaryKey { get; set; }
        //显示Value
        public string DictionaryValue { get; set; }
        public string ShowIcon { get; set; }
        public string ShowPostion { get; set; }
        public string ShowColor { get; set; }
        public string ShowUnit { get; set; }
        public int ShowOrder { get; set; }
    }

}
