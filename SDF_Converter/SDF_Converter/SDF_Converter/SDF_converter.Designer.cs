
namespace SDF_Converter
{
    partial class SDF_converter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txt_file_search = new System.Windows.Forms.TextBox();
            this.btn_file_explore = new System.Windows.Forms.Button();
            this.btn_convert = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Name";
            // 
            // txt_file_search
            // 
            this.txt_file_search.Location = new System.Drawing.Point(16, 75);
            this.txt_file_search.Name = "txt_file_search";
            this.txt_file_search.Size = new System.Drawing.Size(695, 30);
            this.txt_file_search.TabIndex = 1;
            // 
            // btn_file_explore
            // 
            this.btn_file_explore.Location = new System.Drawing.Point(717, 75);
            this.btn_file_explore.Name = "btn_file_explore";
            this.btn_file_explore.Size = new System.Drawing.Size(47, 30);
            this.btn_file_explore.TabIndex = 2;
            this.btn_file_explore.Text = "...";
            this.btn_file_explore.UseVisualStyleBackColor = true;
            this.btn_file_explore.Click += new System.EventHandler(this.btn_file_explore_Click);
            // 
            // btn_convert
            // 
            this.btn_convert.Location = new System.Drawing.Point(16, 111);
            this.btn_convert.Name = "btn_convert";
            this.btn_convert.Size = new System.Drawing.Size(103, 44);
            this.btn_convert.TabIndex = 3;
            this.btn_convert.Text = "Convert";
            this.btn_convert.UseVisualStyleBackColor = true;
            this.btn_convert.Visible = false;
            this.btn_convert.Click += new System.EventHandler(this.btn_convert_Click);
            // 
            // SDF_converter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 196);
            this.Controls.Add(this.btn_convert);
            this.Controls.Add(this.btn_file_explore);
            this.Controls.Add(this.txt_file_search);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SDF_converter";
            this.Text = "SDF Converter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_file_search;
        private System.Windows.Forms.Button btn_file_explore;
        private System.Windows.Forms.Button btn_convert;
    }
}

