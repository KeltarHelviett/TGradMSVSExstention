using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Configuration;
using TGradMSVSExtention.Properties;
using System.Xml;
using System.Xml.Serialization;

namespace TGradMSVSExtention
{
    static class Config
    {
        static public void SetTemplateSource(string name, string value)
        {
            if (Directory.Exists(value))
            {
                Settings.Default[name + "Folder"] = value;
            }
        }

        static public void SetTemplateSources(string[] names, string[] values)
        {
           for (int i = 0; i < names.Length; ++i)
            {
                SetTemplateSource(names[i], values[i]);
            }
        }

        static public string GetTemplateSource(string name)
        {
            
            return Settings.Default[name + "Folder"] as string;
        }

        static public bool Load()
        {
            return true;
        }

        static public void Save()
        {
            Settings.Default.Save();
        }
    }
}
