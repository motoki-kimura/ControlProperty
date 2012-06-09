using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ControlProperty;

/// Using DynamicJson.
/// Download DyanmicJson.
/// After add project and add reference setting.
using Codeplex.Data;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set all Contorl value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, EventArgs e)
        {
            /// Read all control value by JSON file.
            ControlProperty.Property[] val = DynamicJson.Parse( System.IO.File.ReadAllText("ctrl.json"));

            /// Set all control value by ControProperty method.
            ControlProperty.ControlProperty.Set(this.Controls, val);
        }

        /// <summary>
        /// Save all control value.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            /// Get all control value by ControlProperty method.
            var ctrlList = ControlProperty.ControlProperty.Get(this.Controls);

            /// Write all control value use JSON file.
            System.IO.File.WriteAllText("ctrl.json", DynamicJson.Serialize(ctrlList.ToArray()));
        }




        private void Save_Click_exclusion(object sender, EventArgs e)
        {
            /// Make exclusion control list.
            var exclusionList = new[] { textBox6.Name };

            /// Get all control value by ControlProperty method.
            /// After, It excepts using LINQ.
            var ctrlList = ControlProperty.ControlProperty.Get(this.Controls).Where(p => (!exclusionList.Contains(p.Name)));

            System.IO.File.WriteAllText("ctrl.json", DynamicJson.Serialize(ctrlList.ToArray()));
        }


        /// <summary>
        /// Save conf file.
        /// </summary>
        private void SaveControlValue()
        {
            /// Write save code, for new control.
            System.IO.File.WriteAllText("conf.txt", vScrollBar6.Value.ToString());
        }

        /// <summary>
        /// Read conf file.
        /// </summary>
        private void ReadControlValue()
        {
            /// Write read code, for new control.
            var val = System.IO.File.ReadAllText("conf.txt");

            vScrollBar6.Value = Int32.Parse(val);
        }
    }
}
