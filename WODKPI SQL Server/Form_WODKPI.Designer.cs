namespace WODKPI_SQL_Server
{
    partial class Form_WODKPI
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
            this.Connect_To_WODKPI = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Connect_To_WODKPI
            // 
            this.Connect_To_WODKPI.Location = new System.Drawing.Point(12, 12);
            this.Connect_To_WODKPI.Name = "Connect_To_WODKPI";
            this.Connect_To_WODKPI.Size = new System.Drawing.Size(150, 50);
            this.Connect_To_WODKPI.TabIndex = 0;
            this.Connect_To_WODKPI.Text = "Connect to WODKPI";
            this.Connect_To_WODKPI.UseVisualStyleBackColor = true;
            this.Connect_To_WODKPI.Click += new System.EventHandler(this.Connect_To_WODKPI_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 68);
            this.button1.Name = "cleanDataButton";
            this.button1.Size = new System.Drawing.Size(150, 50);
            this.button1.TabIndex = 1;
            this.button1.Text = "Clean Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Clean_Data_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(12, 124);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 50);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Form_WODKPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 193);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Connect_To_WODKPI);
            this.Name = "Form_WODKPI";
            this.Text = "WODKPI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Connect_To_WODKPI;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cancelButton;
    }
}

