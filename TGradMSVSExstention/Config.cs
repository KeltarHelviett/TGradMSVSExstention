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
            var ts = Settings.Default[name] as TemplateSrc;
            if (ts != null)
            {
                if (Directory.Exists(value))
                {
                    ts.Folder = value;
                    ts.File = "";
                }
                else if (File.Exists(value))
                {
                    ts.File = value;
                    ts.Folder = "";
                }
                else
                {
                    MessageBox.Show("Inccorrect input");
                }
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
            var ts = Settings.Default[name] as TemplateSrc;
            if (ts != null)
            {
                return ts.Folder != "" ? ts.Folder : ts.File;
            }
            return "";
        }

        static public bool Load()
        {
            string[] names = new string[] { "Model", "View", "ViewModel"};
            foreach (string name in names)
            {
                Settings.Default.Properties[name].DefaultValue = new TemplateSrc();
                if (Settings.Default[name] == null)
                    Settings.Default[name] = Settings.Default.Properties[name].DefaultValue;
            }
            Settings.Default.Save();
            Settings.Default.Reload();
            return true;
        }

        static public void Save()
        {
            Settings.Default.Save();
        }
    }
}
