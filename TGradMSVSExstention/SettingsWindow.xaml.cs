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
  
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void GeneralTemplatesTVISelected(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Children.Clear();
            StackPanel sp = new StackPanel();
            SettingsPanel.Children.Add(sp);
            sp.Width = SettingsPanel.Width;
            sp.Height = SettingsPanel.Height;
            string[] classNames = { "Model", "View", "View Model"};
            Thickness margin = new Thickness(0, 0, 20, 0);
            int rowCounter = -1;
            Grid g = new Grid() { Width = sp.Width, Height = sp.Height };
            sp.Children.Add(g);
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            g.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            RoutedEventHandler eh = new RoutedEventHandler((sndr, rea) =>
            {
                using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
                {
                    var res = dlg.ShowDialog();
                    if (res == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dlg.SelectedPath))
                    {
                        ((sndr as Button).Tag as TextBox).Text = dlg.SelectedPath;
                    }
                }
            });
            foreach (string name in classNames)
            {
                Label l = new Label() { Margin = margin, Content = name/*, Width = 50, Height = 20*/};

                TextBox tb = new TextBox() { Margin = margin, Width = 200,  Height = 20, Text = Config.GetTemplateSource(name.Replace(" ", ""))};
                Button b = new Button() { Margin = margin, Content = "Brows...", Height = 20, Tag = tb };
                g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
                rowCounter++;
                b.Click += eh;
                g.Children.Add(l);
                g.Children.Add(tb);
                g.Children.Add(b);
                //sp.Children.Add(l);
                //sp.Children.Add(tb);
                //sp.Children.Add(b);
                Grid.SetRow(l, rowCounter);
                Grid.SetRow(tb, rowCounter);
                Grid.SetRow(b, rowCounter);
                Grid.SetColumn(l, 0);
                Grid.SetColumn(tb, 1);
                Grid.SetColumn(b, 2);
            }
        }
    }
}
