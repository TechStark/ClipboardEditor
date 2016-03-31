using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClipboardEditor
{
    public partial class MainForm : Form
    {
        private Dictionary<string, string> dict;

        public MainForm()
        {
            InitializeComponent();

            initFormats();
        }

        private void initFormats()
        {
            var items = comboBoxFormatType.Items;
            var fields = typeof(Formats).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field.IsLiteral && !field.IsInitOnly)
                {
                    var format = field.GetRawConstantValue();
                    items.Add(format);
                }
            }

            if (items.Count > 0)
            {
                comboBoxFormatType.SelectedIndex = 0;
            }
        }

        private void buttonGetData_Click(object sender, EventArgs e)
        {
            string format = comboBoxFormatType.Text;

            try
            {
                string data = ClipboardUtil.GetData(format);
                textBoxContent.Text = data;
            }
            catch (Exception ex)
            {
                textBoxContent.Text = "ERROR:\n";
                textBoxContent.AppendText(ex.Message);
            }

        }

        private void buttonSetData_Click(object sender, EventArgs e)
        {
            string format = comboBoxFormatType.Text;
            string data = textBoxContent.Text;

            try
            {
                ClipboardUtil.SetData(format, data);
            }
            catch (Exception ex)
            {
                textBoxContent.Text = "ERROR:\n";
                textBoxContent.AppendText(ex.Message);
            }
        }

        private Dictionary<string, string> GetDict()
        {
            if (dict == null)
            {
                dict = new Dictionary<string, string>();
            }

            return dict;
        }

        private void Sync()
        {
            string data = textBoxPreview.Text;
            dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }

        private void UpdatePreview()
        {
            string json = JsonConvert.SerializeObject(GetDict(), Formatting.Indented);
            textBoxPreview.Text = json;
        }

        private void buttonNewClone_Click(object sender, EventArgs e)
        {
            try
            {
                dict = ClipboardUtil.GetData();
                UpdatePreview();
            }
            catch (Exception ex)
            {
                textBoxPreview.Text = "ERROR:\n";
                textBoxPreview.AppendText(ex.Message);
            }
        }

        private void buttonRemoveFormat_Click(object sender, EventArgs e)
        {
            Sync();

            string format = comboBoxFormatType.Text;
            GetDict().Remove(format);

            UpdatePreview();
        }

        private void buttonUpdateFormat_Click(object sender, EventArgs e)
        {
            Sync();

            string format = comboBoxFormatType.Text;
            string data = textBoxContent.Text;
            GetDict()[format] = data;

            UpdatePreview();
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            Sync();

            ClipboardUtil.SetData(GetDict());
        }

    }
}
