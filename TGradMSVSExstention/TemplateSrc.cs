using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TGradMSVSExtention
{
    [Serializable]
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public sealed class TemplateSrc: ApplicationSettingsBase
    {
        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string Folder
        {
            get { return (string)(this["Folder"]); }
            set { this["Folder"] = value; }
        }

        [UserScopedSettingAttribute()]
        [DefaultSettingValueAttribute("")]
        public string File
        {
            get { return (string)(this["File"]); }
            set { this["File"] = value; }
        }

        public TemplateSrc()
        {
            Folder = "";
            File = "";
        }

        public TemplateSrc(string folder, string file)
        {
            Folder = folder;
            File = file;
        }
    }

    //public sealed class TemplateSrcSettings: ApplicationSettingsBase
    //{
    //    [UserScopedSetting]
    //    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    //    public TemplateSrc TemplateSrc
    //    {
    //        get; set;
    //    }
    //}
}
