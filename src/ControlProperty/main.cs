using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Dynamic;

namespace ControlProperty
{
    /// <summary>
    /// Control's property.
    /// </summary>
    public struct Property
    {
        public string Name { get; set; }
        public dynamic Value { get; set; }
    }

    /// <summary>
    /// GetValue() Value is get.
    /// SetValue() Value is set;
    /// </summary>
    public struct GetSet
    {
        public Func<dynamic, dynamic> GetValue;
        public Action<dynamic, dynamic> SetValue;
    }

    /// <summary>
    /// Control setter/getter class.
    /// </summary>
    public class ControlProperty
    {

        /// <summary>
        /// Register control, SetValue, GetValue.
        /// </summary>
        /// <returns>control setter/getter dictionary.</returns>
        public static Dictionary<Type, GetSet> GetTypeList()
        {
            /// Register Dictionary.
            /// SetValue for control type.
            /// GetValue for control type.

            /// ex)
            /// dic.Add( typeof(Control type), 
            ///     new GetSet() { GetValue = (p) => (Control setter),
            ///                    SetValue = (p) => (Control getter) });
             
            var dic = new Dictionary<Type, GetSet>();

            dic.Add(typeof(TextBox), new GetSet() { GetValue = (p) => p.Text, SetValue = (p, v) => p.Text = v });
            dic.Add(typeof(RadioButton), new GetSet() { GetValue = (p) => p.Checked, SetValue = (p, v) => p.Checked = v });
            dic.Add(typeof(ComboBox), new GetSet() { GetValue = (p) => p.SelectedIndex, SetValue = (p, v) => p.SelectedIndex = (int)v });
            dic.Add(typeof(DateTimePicker), new GetSet() { GetValue = (p) => p.Value, SetValue = (p, v) => p.Value = DateTime.Parse(v) });
            dic.Add(typeof(CheckBox), new GetSet() { GetValue = (p) => p.Checked, SetValue = (p, v) => p.Checked = v });
            dic.Add(typeof(ListBox), new GetSet() { GetValue = (p) => p.SelectedIndex, SetValue = (p, v) => p.SelectedIndex = (int)v });
            dic.Add(typeof(TrackBar), new GetSet() { GetValue = (p) => p.Value, SetValue = (p, v) => p.Value = (int)v });
            dic.Add(typeof(VScrollBar), new GetSet() { GetValue = (p)=> p.Value, SetValue = (p, v) => p.Value = (int)v });

            return dic;
        }


        /// <summary>
        /// Control value is get
        /// </summary>
        /// <param name="coll">Control collection</param>
        /// <returns>Control value</returns>
        public static Property[] Get(System.Windows.Forms.Control.ControlCollection coll)
        {
            var dic = GetTypeList();
            var ret = new List<Property>();

            /// Control scanning
            foreach(Control ctrl in coll)
            {
                /// Child control recurisve call
                if(ctrl.Controls.Count > 0 )
                {
                    var c_ctrl = Get(ctrl.Controls);

                    ret.AddRange(c_ctrl);
                }
                /// know control, value is get
                else
                {
                    if (dic.ContainsKey(ctrl.GetType()))
                    {
                        ret.Add(new Property() { Name = ctrl.Name, Value = dic[ctrl.GetType()].GetValue(ctrl) });
                    }
                }
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Control value is set
        /// </summary>
        /// <param name="coll">Control collection</param>
        /// <param name="val">Configuration file of a value</param>
        public static void Set(System.Windows.Forms.Control.ControlCollection coll, Property[] val)
        {
            var dic = GetTypeList();
            var ret = new List<Property>();

            /// Control scanning 
            foreach (Control ctrl in coll)
            {
                /// Child control recursive call
                if (ctrl.Controls.Count > 0)
                {
                    Set(ctrl.Controls, val);
                }
                /// know control, value is set
                else
                {
                    if (dic.ContainsKey(ctrl.GetType()))
                    {
                        var currentValue = val.Where(p => p.Name == ctrl.Name);
                        if (currentValue.Any())
                        {
                            dic[ctrl.GetType()].SetValue(ctrl, currentValue.First().Value);
                        }
                    }
                }
            }
        }
    }
}
