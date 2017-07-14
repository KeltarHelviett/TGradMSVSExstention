using TGradMSVSExtension.Properties;
using System.ComponentModel;
using System.Windows;

namespace TGradMSVSExtension
{
    class SettingsModel : INotifyPropertyChanged
    {

        private static readonly SettingsModel instance = new SettingsModel();

        private SettingsModel () { }

        public static SettingsModel Instance
        {
            get
            {
                return instance;
            }
        }

        #region PublicProperties

        static public Settings Default
        {
            get { return Settings.Default; }
        }

        private string modelFolder;

        public string ModelFolder
        {
            set
            {
                modelFolder = value;
                RaisePropertyChanged("ModelFolder"); 
            }
            get { return modelFolder; }
        }

        private string viewFolder;

        public string ViewFolder
        {
            set
            {
                viewFolder = value;
                RaisePropertyChanged("ViewFolder");
            }
            get { return viewFolder; }
        }

        private string detailViewFolder;

        public string DetailViewFolder
        {
            set
            {
                detailViewFolder = value;
                RaisePropertyChanged("DetailViewFolder");
            }
            get { return detailViewFolder; }
        }

        private string masterViewFolder;

        public string MasterViewFolder
        {
            set
            {
                masterViewFolder = value;
                RaisePropertyChanged("MasterViewFolder");
            }
            get { return masterViewFolder; }
        }

        private string viewModelFolder;

        public string ViewModelFolder
        {
            set
            {
                viewModelFolder = value;
                RaisePropertyChanged("ViewModelFolder");
            }
            get { return viewModelFolder; }
        }

        private string detailViewModelFolder;

        public string DetailViewModelFolder
        {
            set
            {
                detailViewModelFolder = value;
                RaisePropertyChanged("DetailViewModelFolder");
            }
            get { return detailViewModelFolder; }
        }

        private string masterViewModelFolder;

        public string MasterViewModelFolder
        {
            set
            {
                masterViewModelFolder = value;
                RaisePropertyChanged("MasterViewModelFolder");
            }
            get { return masterViewModelFolder; }
        }

        private string repositoryFolder;
        public string RepositoryFolder
        {
            set
            {
                repositoryFolder = value;
                RaisePropertyChanged("RepositoryFolder");
            }
            get { return repositoryFolder; }
        }
        private string dNRepositoryFolder;
        public string DNRepositoryFolder
        {
            set
            {
                dNRepositoryFolder = value;
                RaisePropertyChanged("DNRepositoryFolder");
            }
            get { return dNRepositoryFolder; }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            ModelFolder           = Default["ModelFolder"] as string;
            ViewFolder            = Default["ViewFolder"] as string;
            DetailViewFolder      = Default["DetailViewFolder"] as string;
            MasterViewFolder      = Default["MasterViewFolder"] as string;
            ViewModelFolder       = Default["ViewModelFolder"] as string;
            DetailViewModelFolder = Default["DetailViewModelFolder"] as string;
            MasterViewModelFolder = Default["MasterViewModelFolder"] as string;
            RepositoryFolder      = Default["RepositoryFolder"] as string;
            DNRepositoryFolder    = Default["DatumNodeRepositoryFolder"] as string;
        }
        
        public void Save()
        {
            Default["ModelFolder"]               = ModelFolder;
            Default["ViewFolder"]                = ViewFolder;
            Default["DetailViewFolder"]          = DetailViewFolder;
            Default["MasterViewFolder"]          = MasterViewFolder;
            Default["ViewModelFolder"]           = ViewModelFolder;
            Default["DetailViewModelFolder"]     = DetailViewModelFolder;
            Default["MasterViewModelFolder"]     = MasterViewModelFolder;
            Default["RepositoryFolder"]          = RepositoryFolder;
            Default["DatumNodeRepositoryFolder"] = DNRepositoryFolder;
            Default.Save();
        }
    }
}
