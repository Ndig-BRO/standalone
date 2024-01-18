
namespace ConverterSDX
{
    partial class ConverterSDX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConverterSDX));
            this.label1 = new System.Windows.Forms.Label();
            this.textBX = new System.Windows.Forms.TextBox();
            this.search_btn = new System.Windows.Forms.Button();
            this.convert_sdx = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "File Name";
            // 
            // textBX
            // 
            this.textBX.Location = new System.Drawing.Point(48, 37);
            this.textBX.Name = "textBX";
            this.textBX.Size = new System.Drawing.Size(705, 22);
            this.textBX.TabIndex = 0;
            // 
            // search_btn
            // 
            this.search_btn.Location = new System.Drawing.Point(759, 34);
            this.search_btn.Name = "search_btn";
            this.search_btn.Size = new System.Drawing.Size(48, 29);
            this.search_btn.TabIndex = 1;
            this.search_btn.Text = "...";
            this.search_btn.UseVisualStyleBackColor = true;
            this.search_btn.Click += new System.EventHandler(this.btn_file_explore_Click);
            // 
            // convert_sdx
            // 
            this.convert_sdx.Location = new System.Drawing.Point(48, 74);
            this.convert_sdx.Name = "convert_sdx";
            this.convert_sdx.Size = new System.Drawing.Size(133, 28);
            this.convert_sdx.TabIndex = 2;
            this.convert_sdx.Text = "SDX Convert";
            this.convert_sdx.UseVisualStyleBackColor = true;
            this.convert_sdx.Visible = false;
            this.convert_sdx.Click += new System.EventHandler(this.btn_convert_Click);
            // 
            // ConverterSDX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(847, 130);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.convert_sdx);
            this.Controls.Add(this.search_btn);
            this.Controls.Add(this.textBX);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConverterSDX";
            this.Text = "ConverterSDX";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBX;
        private System.Windows.Forms.Button search_btn;
        private System.Windows.Forms.Button convert_sdx;
    }
}

