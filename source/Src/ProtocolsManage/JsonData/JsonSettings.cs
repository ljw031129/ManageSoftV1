using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProtocolsManage.JsonData
{

    public static class JsonSettings<T> where T : new()
    {
        private const string DEFAULT_FILENAME = "JsonData";

        //public static void Save(string fileName = DEFAULT_FILENAME)
        //{
        //    File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
        //}

        public static void Save(T pSettings, string fileName = DEFAULT_FILENAME)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(pSettings));
        }

        public static T Load(string fileName = DEFAULT_FILENAME)
        {

            T t = new T();
            try
            {
                if (File.Exists(fileName))
                    t = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));

            }

            catch (Exception)
            {

                throw;
            }            
            return t;
        }
    }
}
