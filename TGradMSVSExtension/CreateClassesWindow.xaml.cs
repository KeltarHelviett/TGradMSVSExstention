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

namespace TGradMSVSExtension
{
  
    public partial class MVVMClassCreatorWindow : Window
    {
        class TypedComboBox
        {
            public ClassType Type { set; get; }
            public ComboBox ComBox { set; get; }
        }

        public MVVMClassCreatorWindow()
        {
            InitializeComponent();
            var cbs = new CheckBox[] { ModelCB, ViewCB, ViewModelCB, RepositoryCB, DNRepositoryCB, DetailViewCB, DetailViewModelCB,
                MasterViewCB, MasterViewModelCB };
            var comboxes = new ComboBox[] { ModelComBox, ViewComBox, ViewModelComBox, RepositoryComBox, DNRepositoryComBox, DetailViewComBox,
                DetailVMComBox, MasterViewComBox, MasterVMComBox };
            var types = new ClassType[] { ClassType.Model, ClassType.View, ClassType.ViewModel, ClassType.Repository, ClassType.DatumNodeRepository,
                ClassType.DetailView, ClassType.DetailViewModel, ClassType.MasterView, ClassType.MasterViewModel };
            for (int i = 0; i < cbs.Length; ++i)
            {
                cbs[i].Checked += MVVMClassCheckBoxChecked;
                cbs[i].Unchecked += MVVMClassCheckBoxUnchecked;
                cbs[i].Tag = new TypedComboBox() { Type = types[i],  ComBox = comboxes[i]};
            }
            FillComboBoxes();
        }

        public MVVMClassCreatorWindow(string className): this()
        {
            ClassNameTB.Text = className;
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
            List<ClassType> types = new List<ClassType>();
            var cbs = CreateClassesGrid.Children.OfType<CheckBox>().ToArray();
            foreach (var cb in cbs)
            {
                if (cb.IsChecked.HasValue && cb.IsChecked.Value && (cb.Tag is TypedComboBox))
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
            var cbs = CreateClassesGrid.Children.OfType<CheckBox>().ToArray();
            foreach (var cb in cbs)
            {
                if (cb.Tag is TypedComboBox)
                {
                    var tcombox = cb.Tag as TypedComboBox;
                    string folder = SettingsViewModel.TemplateSrcSettings.GetTemplateSource(tcombox.Type.ToString());
                    if (folder != null && folder != "")
                    {
                        var files = Directory.GetFiles(folder, "*.cst", SearchOption.TopDirectoryOnly).Select(System.IO.Path.GetFileName);
                        foreach (var file in files)
                        {
                            tcombox.ComBox.Items.Add(new ComboBoxItem() { Content = file });
                        }
                    }
                    tcombox.ComBox.Items.Add(new ComboBoxItem() { Content = "Default" });
                    tcombox.ComBox.SelectedIndex = 0;
                }
            }
        }

        private void AllCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            var cbs = CreateClassesGrid.Children.OfType<CheckBox>().ToArray();
            var self = sender as CheckBox;
            foreach (var cb in cbs)
            {
                if (cb != self)
                    cb.IsChecked = true;
            }
        }
    }
}
