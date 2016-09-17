using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections;

namespace K3BPMServiceLibrary
{
    public class Config
    {
        public Dictionary<string, string> ReadAllSettings()
        {
            Dictionary<string, string> dictAppSettings = new Dictionary<string, string>();
            var appSettings = ConfigurationManager.AppSettings;
            foreach(var key in appSettings.AllKeys)
            {
                if (!dictAppSettings.ContainsKey(key))
                {
                    dictAppSettings.Add(key, appSettings[key]);
                }
            }
            return dictAppSettings;

        }
    }
}
