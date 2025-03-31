
namespace System
{
    partial class MeasureResult
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
            this.lbmeasureResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "量测结果：";
            // 
            // lbmeasureResult
            // 
            this.lbmeasureResult.AutoSize = true;
            this.lbmeasureResult.Location = new System.Drawing.Point(215, 46);
            this.lbmeasureResult.Name = "lbmeasureResult";
            this.lbmeasureResult.Size = new System.Drawing.Size(190, 24);
            this.lbmeasureResult.TabIndex = 1;
            this.lbmeasureResult.Text = "lbmeasureResult";
            // 
            // MeasureResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 245);
            this.Controls.Add(this.lbmeasureResult);
            this.Controls.Add(this.label1);
            this.Name = "MeasureResult";
            this.Text = "量测";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.MeasureResult_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Windows.Forms.Label label1;
        public Windows.Forms.Label lbmeasureResult;
    }
}