using System;
using System.Windows.Forms;

namespace ConverterSDX
{
    public partial class ConverterSDX : Form
    {
        public ConverterSDX()
        {
            InitializeComponent();
        }

        // Upon button click the file explore will allow only sdf to be searched
        // then when the file is displayed in the test box the second button will appear.
        private void btn_file_explore_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "SQL files (*.sdf)|*.sdf";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBX.Text = fileDialog.FileName;
                convert_sdx.Visible = true;
            }
        }

        // Upon button click the filter will check for sdf.
        // Then will check for the file type [Global or project]
        // The file type will dictate which function shall be called
        private void btn_convert_Click(object sender, EventArgs e)
        {
            ProjectSDF projectSDF = new ProjectSDF();
            projectSDF.FileSaveHandler(textBX.Text);

        }
    }
}
