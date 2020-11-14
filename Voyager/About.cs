using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Voyager
{
    public partial class About : Form
    {
        #region Constructor

        public About()
        {
            InitializeComponent();

            WebBrowser wbversion = new WebBrowser();
            this.version.Text = "Voyager " + String.Format(AssemblyVersion);
            this.wbversion.Text = "Web Browser Version:  " + wbversion.Version.Major.ToString() + "." + wbversion.Version.Minor.ToString() + "." + wbversion.Version.Build.ToString();
            this.TopMost = true;
        }

        #endregion

        #region Assembly Attribute Accessors

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #endregion

        private void okbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
