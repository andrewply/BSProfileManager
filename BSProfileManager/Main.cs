using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BSProfileManager
{
    public partial class Main : Form
    {
        private string versionNumber = "v0.8";

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = string.Format(this.Text, versionNumber);
        }

    }
}
