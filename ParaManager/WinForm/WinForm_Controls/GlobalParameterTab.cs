using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParaManager.WinForm.WinForm_Controls
{
    public partial class GlobalParameterTab : UserControl
    {
        
        // Public static properties
        public static string ElementName1 { get; set; }
        public static List<string> paramNames1 { get; set; } = new List<string>(); // The name of the parameters we want it to be created
        public GlobalParameterTab()
        {
            InitializeComponent();
            // clear the items in listBox2 and paraNames in the WF constructor otherwise it will save the data from last run
            listBox2.Items.Clear();
            paramNames1.Clear();
            // 

            if (ExtCmd.doc.IsFamilyDocument)
            {
                comboBox1.Enabled = false;
            }
            else
            {
                comboBox1.Items.Add("Select the Element");
                comboBox1.SelectedIndex = 0;
                // File the comboList with the familyInstance names
                List<string> familyInstanceNames = rvt.getFamilyInstancesName(ExtCmd.doc);
                foreach (string name in familyInstanceNames)
                {
                    comboBox1.Items.Add(name).ToString();
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ElementName1 = comboBox1.SelectedItem.ToString();

            // Check if the selected item is not "Select the Element"
            if (comboBox1.SelectedItem != null && ElementName1 != "Select the Element")
            {
                Element selectedElement = rvt.getElementByName(ElementName1);
                foreach (Parameter parameter in selectedElement.Parameters)
                {
                    listBox1.Items.Add(parameter.Definition.Name);
                }
            }
            else
            {
                // Handle the case where the selected element is not found
                listBox1.Items.Add("Selected element not found.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (object item in listBox1.SelectedItems)
            {
                listBox2.Items.Add(item.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Loop through the selected items in listBox1
            for (int i = listBox2.SelectedIndices.Count - 1; i >= 0; i--)
            {
                int selectedIndex = listBox2.SelectedIndices[i];
                if (selectedIndex >= 0)
                {
                    // Remove the selected item from listBox1
                    string selectedItem = listBox2.Items[selectedIndex].ToString();
                    listBox2.Items.RemoveAt(selectedIndex);

                    // Add the selected item to listBox2
                    listBox1.Items.Add(selectedItem);

                    // Delete the item from the parametersName
                    paramNames1.Remove(selectedItem);

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Getting parameters names from listBox2
            foreach (object item in listBox2.Items)
            {
                paramNames1.Add(item.ToString());
            }

            if (listBox2.Items.Count > 0)
            {
                
                if (!ExtCmd.doc.IsFamilyDocument)
                {
                    ExtEvent.request = Request.createGlobalParameter;
                    FamilyParameterTab.ex.Raise();
                }
            }
            else
            {
                MessageBox.Show("No Selected Parameters were found");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
