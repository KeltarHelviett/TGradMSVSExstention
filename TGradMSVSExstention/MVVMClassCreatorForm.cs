using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TGradMSVSExstention
{
    public partial class MVVMClassCreatorForm : Form
    {
        public MVVMClassCreatorForm()
        {
            InitializeComponent();
        }

        private void AcceptBtn_Click(object sender, EventArgs e)
        {
            string name = ClassNameTB.Text;
            MVVMClassCreator.Instance.GenerateMVVMClasses(name);
            this.Close();
        }
    }
}
