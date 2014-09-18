using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGoal.Model.ViewModels
{
    public class ProtocolTestViewModel
    {
        public List<KeyValue> PmFInterpreterResult { get; set; }
        public List<TerminalDataViewModel> ReceiveDataDisplayResult { get; set; }
    }
    public class KeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
