using Newtonsoft.Json;
using ProtocolsManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtocolsManage.Common;
using System.Collections;

using System.IO;
using ProtocolsManage.JsonData;

namespace ProtocolsManage
{
    class Program
    {
        static void Main(string[] args)
        {
            List<MenuModel> menulist = new List<MenuModel>();

            MenuModel ChildMenu;
            MenuModel menu = new MenuModel();
            menu.Id = 0;
            menu.Icon = "icon-folder-open-alt";
            menu.MenuUrl = "www.baidu.com";
            menu.MenuName = "一级菜单";

            List<MenuModel> ChildMenuList01 = new List<MenuModel>();
            ChildMenu = new MenuModel();
            ChildMenu.Id = 1;
            ChildMenu.Icon = "icon-folder-open-alt";
            ChildMenu.MenuUrl = "www.baidu.com";
            ChildMenu.MenuName = "二级菜单03-01";
            ChildMenuList01.Add(ChildMenu);

            List<MenuModel> ChildMenuList = new List<MenuModel>();
            ChildMenu = new MenuModel();
            ChildMenu.Id = 1;
            ChildMenu.Icon = "icon-folder-open-alt";
            ChildMenu.MenuUrl = "www.baidu.com";
            ChildMenu.MenuName = "二级菜单01";
            ChildMenuList.Add(ChildMenu);
            ChildMenu = new MenuModel();
            ChildMenu.Id = 2;
            ChildMenu.Icon = "icon-folder-open-alt";
            ChildMenu.MenuUrl = "www.baidu.com";
            ChildMenu.MenuName = "二级菜单02";
            ChildMenuList.Add(ChildMenu);
            ChildMenu = new MenuModel();
            ChildMenu.Id = 2;
            ChildMenu.Icon = "icon-folder-open-alt";
            ChildMenu.MenuUrl = "www.baidu.com";
            ChildMenu.MenuName = "二级菜单03";

            ChildMenu.ChildMenu = ChildMenuList01;
            ChildMenuList.Add(ChildMenu);


            menu.ChildMenu = ChildMenuList;

            menulist.Add(menu);
            string ww22 = JsonConvert.SerializeObject(menulist);
            int i5 = 360000;
            string s =FInterpreterUtil.RenH2L(i5.ToString("X8")); //转16进制
           
            int xx = 20;
            int valI = ((int)xx & (Convert.ToInt16("1".PadLeft(0 +8, '1'), 2))) >> (0);
         string  xxx= Convert.ToString(valI, 2).PadLeft(8,'0');
            string ss = valI.ToString().PadLeft(2,'0');

            BitArray ba1 = new BitArray(16);
            BitArray ba2 = new BitArray(8);
            byte[] a = { 0x12,0x13 };
            byte[] b = { 13 };

            // 把值 60 和 13 存储到点阵列中
            ba1 = new BitArray(a);
            ba2 = new BitArray(b);

            // ba1 的内容
            Console.WriteLine("Bit array ba1: 60");
            for (int i = 0; i < ba1.Count; i++)
            {
                Console.Write("{0, -6} ", ba1[i]);
            }
            Console.WriteLine();


            byte[] bytestrS = {0x08,0x80};
            double SSS = BitConverter.ToInt16(bytestrS, 0);


            FInterpretersModel fInterpretersModel =  JsonSettings<FInterpretersModel>.Load(@"./JsonData/jsonData.js");
            string ww = JsonConvert.SerializeObject(fInterpretersModel);
            //定义回传信息
            //byte[] bytestr = {0xF1,0xF2,0xF3,
            //                     0x45,
            //                     0x00,0x05,
            //                     0x02,
            //                     0x01,0x10,
            //                     0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x01,
            //                     0x0A,0x0A,
            //                     0x01,
            //                     0x01,
            //                     0x01,
            //                     0x0A,
            //                     0x0A,
            //                     0x59,

            //                     0x02,0x13,
            //                     0x1F,
            //                     0x25,0x2C,0x5B,0xB0,
            //                     0x25,0x2C,0x5B,0xB0,
            //                     0x25,0x2C,0x5B,0xB0,
            //                     0x25,0x2C,0x5B,0xB0,                             
            //                 };

            byte[] bytestr = {0xF1,0xF2,0xF3,
                                 0x45,
                                 0x00,0x05,
                                 0x03,
                                 0x01,0x10,
                                 0x43,0x32,0x33,0x34,0x30,0x30,0x30,0x38,
                                 0x0A,0x0A,
                                 0x01,
                                 0x01,
                                 0x01,
                                 0x0A,
                                 0x0A,
                                 0x59,

                                 0x02,0x11,
                                 0x1F,
                                 0x25,0x2C,0x5B,0xB0,
                                 0x25,0x2C,0x5B,0xB0,
                                 0x25,0x2C,0x5B,0xB0,
                                 0x25,0x2C,0x5B,0xB0,  
                           
                                 0x03,0x14,
                                 0x01,0x01,0x01,0x01,
                                 0x01,0x01,0x01,0x01,
                                 0x25,
                                 0x25,
                                 0x25,0x2C,  
                                 0x01,0x01,0x01,0x01,0x01,0x01,
                                 0x25,
                                 0x25
                             };         

            //分隔字符串
            Dictionary<string, byte[]> dicSplitDatas = FInterpreterUtil.SplitData(fInterpretersModel, bytestr);

            //数据字典内容

            Dictionary<string, string> dicDatas = new Dictionary<string, string>();


            foreach (var item in dicSplitDatas)
            {              

                List<DataBodyModel> dataBodyList = fInterpretersModel.DataBodyModels.Where(t => t.InfoTypeNumber == item.Key).ToList();

                foreach (DataBodyModel itemDataBodyModel in dataBodyList)
                {
                    switch (itemDataBodyModel.DataType)
                    {
                        //byte
                        case 1:
                            FInterpreterUtil.AnalysisByte(itemDataBodyModel, dicDatas, item.Value);
                            break;
                        case 2:
                            FInterpreterUtil.AnalysisBit(itemDataBodyModel, dicDatas, item.Value);
                            break;
                        default:
                            break;
                    }

                }

              
            }

            foreach (var dicItem in dicDatas)
            {
                Console.WriteLine(dicItem.Key+":"+ dicItem.Value);
                Console.WriteLine("-----------------------");
            }


            Console.ReadLine();


        }
    }
}
