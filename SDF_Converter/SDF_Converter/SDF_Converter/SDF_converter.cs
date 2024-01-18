using System;
using System.Windows.Forms;
using System.IO;

namespace SDF_Converter
{
    public partial class SDF_converter : Form
    {
        public SDF_converter()
        {
            InitializeComponent();
        }

        private void btn_file_explore_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "SQL files (*.sdf)|*.sdf|All files (*.*)|*.*";
            if(fileDialog.ShowDialog() == DialogResult.OK)
            {
                txt_file_search.Text = fileDialog.FileName;
                btn_convert.Visible = true;
            }
        }

        private void btn_convert_Click(object sender, EventArgs e)
        {

            // SDF_reader reader = new SDF_reader();
            // reader.ExtractSDF(txt_file_search.Text);
            GlobalSDX globalConverter = new GlobalSDX();
            globalConverter.ExtractGlobalData(txt_file_search.Text);
        }
    }
}
