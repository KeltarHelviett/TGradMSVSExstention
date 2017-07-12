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

namespace TGradMSVSExtention
{
    /// <summary>
    /// Interaction logic for GeneralTemplatePanel.xaml
    /// </summary>
    public partial class GeneralTemplatePanel : UserControl
    {
        public GeneralTemplatePanel()
        {
            InitializeComponent();
            var tbs = new TextBox[] { ModelTB, ViewTB, ViewModelTB, RepositoryTB, DNRepositoryTB};
            var btns = new Button[] { ModelBrowseBtn, ViewBrowseBtn, ViewModelBrowseBtn, RepositoryBrowseBtn, DNRepositoryBrowseBtn};
            var prjTypes = Enum.GetValues(typeof(ProjectType)).Cast<ProjectType>().ToList();
            for (int i = 0; i < tbs.Length; ++i)
            {
                btns[i].Tag = tbs[i];
                tbs[i].Tag = prjTypes[i];
                tbs[i].Text = SettingsViewModel.TemplateSrcSettings.GetTemplateSource(prjTypes[i].ToString());
            }
        }

        private void BrowseBtnClick(object sender, RoutedEventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                var res = dlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
                {
                    var tb = (sender as Button).Tag as TextBox;
                    tb.Text = dlg.SelectedPath;
                }
            }
        }

        public void AcceptBtnClick(object sender, RoutedEventArgs e)
        {
            var tbs = GeneralTemplateGrid.Children.OfType<TextBox>().ToArray();
            foreach (var tb in tbs)
            {
                SettingsViewModel.TemplateSrcSettings.SetTemplateSource(((ProjectType)tb.Tag).ToString(), tb.Text);
            }
        }
    }
}
