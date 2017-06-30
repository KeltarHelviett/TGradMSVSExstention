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
  
    public partial class MVVMClassCreatorWindow : Window
    {
        public MVVMClassCreatorWindow()
        {
            InitializeComponent();
            ClassNameTB.Text = Guid.NewGuid().ToString();
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            string className = ClassNameTB.Text;
            if (className == "")
            {
                MessageBox.Show("No class name was given");
                this.Close();
                return;
            }
        }
    }
}
