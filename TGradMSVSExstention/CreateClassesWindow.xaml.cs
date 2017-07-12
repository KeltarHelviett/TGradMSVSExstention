using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace TGradMSVSExtention
{
  
    public partial class MVVMClassCreatorWindow : Window
    {
        class TypedComboBox
        {
            public ProjectType Type { set; get; }
            public ComboBox ComBox { set; get; }
        }

        public MVVMClassCreatorWindow()
        {
            InitializeComponent();
            var cbs = new CheckBox[] { ModelCB, ViewCB, ViewModelCB, RepositoryCB, DatumNodeRepositoryCB};
            var comboxes = new ComboBox[] { ModelComBox, ViewComBox, ViewModelComBox, RepositoryComBox, DatumNodeRepositoryComBox };
            var types = new ProjectType[] { ProjectType.Model, ProjectType.View, ProjectType.ViewModel, ProjectType.Repository, ProjectType.DatumNodeRepository };
            for (int i = 0; i < cbs.Length; ++i)
            {
                cbs[i].Checked += MVVMClassCheckBoxChecked;
                cbs[i].Unchecked += MVVMClassCheckBoxUnchecked;
                cbs[i].Tag = new TypedComboBox() { Type = types[i],  ComBox = comboxes[i]};
            }
            FillComboBoxes();
        }

        private void MVVMClassCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            (cb.Tag as TypedComboBox).ComBox.IsEnabled = true;
            
        }

        private void MVVMClassCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            (cb.Tag as TypedComboBox).ComBox.IsEnabled = false;
        }

        private void AcceptBtnClick(object sender, RoutedEventArgs e)
        {
            string className = ClassNameTB.Text;
            if (className == "")
            {
                MessageBox.Show("No class name was given");
                this.Close();
                return;
            }
            List<string> templatesFileNames = new List<string>();
            List<ProjectType> types = new List<ProjectType>();
            CheckBox[] cbs = new CheckBox[] { ModelCB, ViewCB, ViewModelCB, RepositoryCB, DatumNodeRepositoryCB };
            foreach (var cb in cbs)
            {
                if (cb.IsChecked.HasValue && cb.IsChecked.Value)
                {
                    templatesFileNames.Add(((cb.Tag as TypedComboBox).ComBox.SelectedItem as ComboBoxItem).Content.ToString());
                    types.Add((cb.Tag as TypedComboBox).Type);
                }
            }
            MVVMSolutionManager.FillActiveSolution(types, className, templatesFileNames);
            this.Close();
        }

        private void FillComboBoxes()
        {
            ComboBox[] cbs = new ComboBox[] { ModelComBox, ViewComBox, ViewModelComBox, RepositoryComBox, DatumNodeRepositoryComBox };
            string[] names = new string[] { "Model", "View", "ViewModel", "Repository", "DatumNodeRepository" };
            var pairs = cbs.Zip(names, (cb, n) => new { CB = cb, Name = n });
            foreach (var pair in pairs)
            {
                string folder = SettingsViewModel.TemplateSrcSettings.GetTemplateSource(pair.Name);
                if (folder != null && folder != "")
                {
                    var files = Directory.GetFiles(folder, "*.cst", SearchOption.TopDirectoryOnly).Select(System.IO.Path.GetFileName);
                    foreach (var file in files)
                    {
                        pair.CB.Items.Add(new ComboBoxItem() { Content = file });
                    }
                }
                pair.CB.Items.Add(new ComboBoxItem() { Content = "Default" });
                pair.CB.SelectedIndex = 0;
            }
        }
    }
}
