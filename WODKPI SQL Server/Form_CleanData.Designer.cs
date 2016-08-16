namespace WODKPI_SQL_Server
{
    partial class Form_CleanData
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
            this._ZIA = new System.Windows.Forms.CheckBox();
            this._Z90 = new System.Windows.Forms.CheckBox();
            this._R81 = new System.Windows.Forms.CheckBox();
            this._OUT = new System.Windows.Forms.CheckBox();
            this._AN6 = new System.Windows.Forms.CheckBox();
            this._1CE = new System.Windows.Forms.CheckBox();
            this._6PU = new System.Windows.Forms.CheckBox();
            this.buttonCancelCleanData = new System.Windows.Forms.Button();
            this.buttonConfirmCleanData = new System.Windows.Forms.Button();
            this.progressOverall = new System.Windows.Forms.ProgressBar();
            this.formOutput = new System.Windows.Forms.Label();
            this.progressCurrentRanker = new System.Windows.Forms.ProgressBar();
            this.tempGraphicLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _ZIA
            // 
            this._ZIA.AutoSize = true;
            this._ZIA.Checked = true;
            this._ZIA.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ZIA.Location = new System.Drawing.Point(11, 150);
            this._ZIA.Name = "_ZIA";
            this._ZIA.Size = new System.Drawing.Size(96, 17);
            this._ZIA.TabIndex = 0;
            this._ZIA.Text = "ZIA - Inventory";
            this._ZIA.UseVisualStyleBackColor = true;
            // 
            // _Z90
            // 
            this._Z90.AutoSize = true;
            this._Z90.Checked = true;
            this._Z90.CheckState = System.Windows.Forms.CheckState.Checked;
            this._Z90.Location = new System.Drawing.Point(11, 127);
            this._Z90.Name = "_Z90";
            this._Z90.Size = new System.Drawing.Size(141, 17);
            this._Z90.TabIndex = 1;
            this._Z90.Text = "Z90 - Supply Chain Loss";
            this._Z90.UseVisualStyleBackColor = true;
            // 
            // _R81
            // 
            this._R81.AutoSize = true;
            this._R81.Checked = true;
            this._R81.CheckState = System.Windows.Forms.CheckState.Checked;
            this._R81.Location = new System.Drawing.Point(11, 104);
            this._R81.Name = "_R81";
            this._R81.Size = new System.Drawing.Size(96, 17);
            this._R81.TabIndex = 2;
            this._R81.Text = "R81 - Refusals";
            this._R81.UseVisualStyleBackColor = true;
            // 
            // _OUT
            // 
            this._OUT.AutoSize = true;
            this._OUT.Checked = true;
            this._OUT.CheckState = System.Windows.Forms.CheckState.Checked;
            this._OUT.Location = new System.Drawing.Point(12, 81);
            this._OUT.Name = "_OUT";
            this._OUT.Size = new System.Drawing.Size(123, 17);
            this._OUT.TabIndex = 3;
            this._OUT.Text = "OUT - Out of Stocks";
            this._OUT.UseVisualStyleBackColor = true;
            // 
            // _AN6
            // 
            this._AN6.AutoSize = true;
            this._AN6.Checked = true;
            this._AN6.CheckState = System.Windows.Forms.CheckState.Checked;
            this._AN6.Location = new System.Drawing.Point(12, 58);
            this._AN6.Name = "_AN6";
            this._AN6.Size = new System.Drawing.Size(91, 17);
            this._AN6.TabIndex = 4;
            this._AN6.Text = "AN6 - Volume";
            this._AN6.UseVisualStyleBackColor = true;
            // 
            // _1CE
            // 
            this._1CE.AutoSize = true;
            this._1CE.Checked = true;
            this._1CE.CheckState = System.Windows.Forms.CheckState.Checked;
            this._1CE.Location = new System.Drawing.Point(12, 12);
            this._1CE.Name = "_1CE";
            this._1CE.Size = new System.Drawing.Size(90, 17);
            this._1CE.TabIndex = 5;
            this._1CE.Text = "1CE - Volume";
            this._1CE.UseVisualStyleBackColor = true;
            // 
            // _6PU
            // 
            this._6PU.AutoSize = true;
            this._6PU.Checked = true;
            this._6PU.CheckState = System.Windows.Forms.CheckState.Checked;
            this._6PU.Location = new System.Drawing.Point(11, 35);
            this._6PU.Name = "_6PU";
            this._6PU.Size = new System.Drawing.Size(91, 17);
            this._6PU.TabIndex = 6;
            this._6PU.Text = "6PU - Volume";
            this._6PU.UseVisualStyleBackColor = true;
            // 
            // buttonCancelCleanData
            // 
            this.buttonCancelCleanData.Location = new System.Drawing.Point(141, 244);
            this.buttonCancelCleanData.Name = "buttonCancelCleanData";
            this.buttonCancelCleanData.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelCleanData.TabIndex = 7;
            this.buttonCancelCleanData.Text = "Cancel";
            this.buttonCancelCleanData.UseVisualStyleBackColor = true;
            this.buttonCancelCleanData.Click += new System.EventHandler(this.buttonCancelCleanData_Click);
            // 
            // buttonConfirmCleanData
            // 
            this.buttonConfirmCleanData.Location = new System.Drawing.Point(60, 244);
            this.buttonConfirmCleanData.Name = "buttonConfirmCleanData";
            this.buttonConfirmCleanData.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirmCleanData.TabIndex = 8;
            this.buttonConfirmCleanData.Text = "Clean Data";
            this.buttonConfirmCleanData.UseVisualStyleBackColor = true;
            this.buttonConfirmCleanData.Click += new System.EventHandler(this.buttonConfirmCleanData_Click);
            // 
            // progressOverall
            // 
            this.progressOverall.Location = new System.Drawing.Point(3, 186);
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(339, 23);
            this.progressOverall.TabIndex = 9;
            // 
            // formOutput
            // 
            this.formOutput.AutoSize = true;
            this.formOutput.Location = new System.Drawing.Point(8, 170);
            this.formOutput.Name = "formOutput";
            this.formOutput.Size = new System.Drawing.Size(336, 13);
            this.formOutput.TabIndex = 10;
            this.formOutput.Text = "Select the rankers you want to clean and click \"Clean Data\" to begin.";
            // 
            // progressCurrentRanker
            // 
            this.progressCurrentRanker.Location = new System.Drawing.Point(3, 215);
            this.progressCurrentRanker.Name = "progressCurrentRanker";
            this.progressCurrentRanker.Size = new System.Drawing.Size(339, 23);
            this.progressCurrentRanker.TabIndex = 11;
            // 
            // tempGraphicLabel
            // 
            this.tempGraphicLabel.AutoSize = true;
            this.tempGraphicLabel.Location = new System.Drawing.Point(214, 81);
            this.tempGraphicLabel.Name = "tempGraphicLabel";
            this.tempGraphicLabel.Size = new System.Drawing.Size(99, 13);
            this.tempGraphicLabel.TabIndex = 12;
            this.tempGraphicLabel.Text = "Insert Graphic Here";
            // 
            // Form_CleanData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 285);
            this.Controls.Add(this.tempGraphicLabel);
            this.Controls.Add(this.progressCurrentRanker);
            this.Controls.Add(this.formOutput);
            this.Controls.Add(this.progressOverall);
            this.Controls.Add(this.buttonConfirmCleanData);
            this.Controls.Add(this.buttonCancelCleanData);
            this.Controls.Add(this._6PU);
            this.Controls.Add(this._1CE);
            this.Controls.Add(this._AN6);
            this.Controls.Add(this._OUT);
            this.Controls.Add(this._R81);
            this.Controls.Add(this._Z90);
            this.Controls.Add(this._ZIA);
            this.Name = "Form_CleanData";
            this.Text = "Clean Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox _ZIA;
        private System.Windows.Forms.CheckBox _Z90;
        private System.Windows.Forms.CheckBox _R81;
        private System.Windows.Forms.CheckBox _OUT;
        private System.Windows.Forms.CheckBox _AN6;
        private System.Windows.Forms.CheckBox _1CE;
        private System.Windows.Forms.CheckBox _6PU;
        private System.Windows.Forms.Button buttonCancelCleanData;
        private System.Windows.Forms.Button buttonConfirmCleanData;
        private System.Windows.Forms.ProgressBar progressOverall;
        private System.Windows.Forms.Label formOutput;
        private System.Windows.Forms.ProgressBar progressCurrentRanker;
        private System.Windows.Forms.Label tempGraphicLabel;
    }
}