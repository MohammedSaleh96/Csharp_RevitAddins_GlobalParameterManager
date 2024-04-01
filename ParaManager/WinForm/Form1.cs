using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection.Emit;
using ParaManager.WinForm.WinForm_Controls;

namespace ParaManager.WinForm
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        // Private Properties
        private FamilyParameterTab familyParameter = new FamilyParameterTab(ExtCmd.doc);
        public Form1(Document doc)
        {
            InitializeComponent();
            // Calling the familyParameter when the form opens
            addUserControl(familyParameter);
        }
        /// <summary>
        /// Create a function to be resposibile to open the tab with
        /// </summary>
        /// <param name="userControl"></param>
        private void addUserControl(UserControl userControl)
        {
            userControl.Dock = DockStyle.Fill;
            panelContainer.Controls.Clear();
            panelContainer.Controls.Add(userControl);
            userControl.BringToFront();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            // FamilyParameterTab familyParameter = new FamilyParameterTab(ExtCmd.doc);
            addUserControl(familyParameter);

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GlobalParameterTab globalParameterTab = new GlobalParameterTab();
            addUserControl(globalParameterTab);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}