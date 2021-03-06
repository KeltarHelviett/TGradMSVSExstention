﻿using System;
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

namespace TGradMSVSExtension
{
    public partial class GeneralTemplatePanel : UserControl
    {
        public GeneralTemplatePanel(double width, double height)
        {
            InitializeComponent();
            this.DataContext = SettingsModel.Instance;
            this.Width = width; this.Height = height;
            GeneralTemplateSP.Width = width; GeneralTemplateSP.Height = height;
            GeneralTemplateGrid.Width = width; GeneralTemplateGrid.Height = height;
            var tbs = new TextBox[] { ModelTB, ViewTB, ViewModelTB, RepositoryTB, DNRepositoryTB, DetailViewTB, MasterViewTB,
                DetailVMTB, MasterVMTB };
            var btns = new Button[] { ModelBrowseBtn, ViewBrowseBtn, ViewModelBrowseBtn, RepositoryBrowseBtn, DNRepositoryBrowseBtn,
                DetailViewBrowseBtn, MasterViewBrowseBtn, DetailViewModelBrowseBtn, MasterViewModelBrowseBtn };
            var prjTypes = Enum.GetValues(typeof(ClassType)).Cast<ClassType>().ToList();
            for (int i = 0; i < tbs.Length; ++i)
            {
                btns[i].Tag = tbs[i];
                tbs[i].Tag = prjTypes[i];
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
            SettingsViewModel.Save();
        }
    }
}
