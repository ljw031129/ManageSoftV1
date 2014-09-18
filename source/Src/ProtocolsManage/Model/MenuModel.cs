using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolsManage.Model
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }

        public virtual List<MenuModel> ChildMenu { get; set; }

       
    }
}
