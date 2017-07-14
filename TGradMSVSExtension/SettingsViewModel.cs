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
using TGradMSVSExtension.Properties;
using System.Xml;
using System.Xml.Serialization;

namespace TGradMSVSExtension
{
    static class SettingsViewModel
    {
        public static class TemplateSrcSettings
        {
            static public void SetTemplateSource(string name, string value)
            {
                if (Directory.Exists(value))
                {
                    SettingsModel.Default[name + "Folder"] = value;
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

                return SettingsModel.Default[name + "Folder"] as string;
            }
        }

        static public bool Load()
        {
            SettingsModel.Instance.Refresh();
            return true;
        }

        static public void Undo()
        {
            SettingsModel.Instance.Refresh();
        }

        static public void Save()
        {
            SettingsModel.Instance.Save();
            SettingsModel.Instance.Refresh();
        }
    }
}
