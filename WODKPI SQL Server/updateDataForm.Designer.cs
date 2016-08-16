﻿namespace WODKPI_SQL_Server
{
    partial class updateDataForm
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
            this.abortUpdateButton = new System.Windows.Forms.Button();
            this.AppendDataButton = new System.Windows.Forms.Button();
            this.progressOverall = new System.Windows.Forms.ProgressBar();
            this.mainMessageLabel = new System.Windows.Forms.Label();
            this.progressCurrentRanker = new System.Windows.Forms.ProgressBar();
            this.copyDataButton = new System.Windows.Forms.Button();
            this.updateNewEntriesButton = new System.Windows.Forms.Button();
            this.convertDataButton = new System.Windows.Forms.Button();
            this.startDatePicker = new System.Windows.Forms.DateTimePicker();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.rankerSelectorCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.branchSelectorCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.EndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateLabel = new System.Windows.Forms.Label();
            this.dataTypeLabel = new System.Windows.Forms.Label();
            this.branchLabel = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.dateSelectorsLabel = new System.Windows.Forms.Label();
            this.currentProgressLabel = new System.Windows.Forms.Label();
            this.overallProgressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.uploadDataLabel = new System.Windows.Forms.Label();
            this.uploadToAccessButton = new System.Windows.Forms.Button();
            this.uploadToSQLButton = new System.Windows.Forms.Button();
            this.updateOverwriteEntriesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // abortUpdateButton
            // 
            this.abortUpdateButton.Location = new System.Drawing.Point(12, 600);
            this.abortUpdateButton.Name = "abortUpdateButton";
            this.abortUpdateButton.Size = new System.Drawing.Size(413, 50);
            this.abortUpdateButton.TabIndex = 7;
            this.abortUpdateButton.Text = "Abort";
            this.abortUpdateButton.UseVisualStyleBackColor = true;
            this.abortUpdateButton.Click += new System.EventHandler(this.buttonCancelCleanData_Click);
            // 
            // AppendDataButton
            // 
            this.AppendDataButton.Location = new System.Drawing.Point(531, 43);
            this.AppendDataButton.Name = "AppendDataButton";
            this.AppendDataButton.Size = new System.Drawing.Size(75, 23);
            this.AppendDataButton.TabIndex = 8;
            this.AppendDataButton.Text = "Clean Data";
            this.AppendDataButton.UseVisualStyleBackColor = true;
            this.AppendDataButton.Click += new System.EventHandler(this.buttonConfirmCleanData_Click);
            // 
            // progressOverall
            // 
            this.progressOverall.Location = new System.Drawing.Point(531, 161);
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(413, 23);
            this.progressOverall.TabIndex = 9;
            // 
            // mainMessageLabel
            // 
            this.mainMessageLabel.AutoSize = true;
            this.mainMessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainMessageLabel.Location = new System.Drawing.Point(110, 10);
            this.mainMessageLabel.Name = "mainMessageLabel";
            this.mainMessageLabel.Size = new System.Drawing.Size(230, 24);
            this.mainMessageLabel.TabIndex = 10;
            this.mainMessageLabel.Text = "Update Data Control Panel";
            // 
            // progressCurrentRanker
            // 
            this.progressCurrentRanker.Location = new System.Drawing.Point(531, 243);
            this.progressCurrentRanker.Name = "progressCurrentRanker";
            this.progressCurrentRanker.Size = new System.Drawing.Size(413, 23);
            this.progressCurrentRanker.TabIndex = 11;
            // 
            // copyDataButton
            // 
            this.copyDataButton.Location = new System.Drawing.Point(531, 14);
            this.copyDataButton.Name = "copyDataButton";
            this.copyDataButton.Size = new System.Drawing.Size(109, 23);
            this.copyDataButton.TabIndex = 13;
            this.copyDataButton.Text = "Copy Data Locally";
            this.copyDataButton.UseVisualStyleBackColor = true;
            this.copyDataButton.Click += new System.EventHandler(this.copyDataButton_Click);
            // 
            // updateNewEntriesButton
            // 
            this.updateNewEntriesButton.Location = new System.Drawing.Point(12, 433);
            this.updateNewEntriesButton.Name = "updateNewEntriesButton";
            this.updateNewEntriesButton.Size = new System.Drawing.Size(208, 50);
            this.updateNewEntriesButton.TabIndex = 14;
            this.updateNewEntriesButton.Text = "Update without Overwrite";
            this.updateNewEntriesButton.UseVisualStyleBackColor = true;
            // 
            // convertDataButton
            // 
            this.convertDataButton.Location = new System.Drawing.Point(531, 71);
            this.convertDataButton.Name = "convertDataButton";
            this.convertDataButton.Size = new System.Drawing.Size(138, 23);
            this.convertDataButton.TabIndex = 15;
            this.convertDataButton.Text = "Convert Data To CSV";
            this.convertDataButton.UseVisualStyleBackColor = true;
            this.convertDataButton.Click += new System.EventHandler(this.convertDataButton_Click);
            // 
            // startDatePicker
            // 
            this.startDatePicker.Location = new System.Drawing.Point(16, 373);
            this.startDatePicker.Name = "startDatePicker";
            this.startDatePicker.Size = new System.Drawing.Size(200, 20);
            this.startDatePicker.TabIndex = 17;
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDateLabel.Location = new System.Drawing.Point(15, 354);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(67, 16);
            this.startDateLabel.TabIndex = 19;
            this.startDateLabel.Text = "Start Date";
            // 
            // rankerSelectorCheckedListBox
            // 
            this.rankerSelectorCheckedListBox.CheckOnClick = true;
            this.rankerSelectorCheckedListBox.FormattingEnabled = true;
            this.rankerSelectorCheckedListBox.Items.AddRange(new object[] {
            "ALL",
            "1CE - Volume",
            "6PU - Volume",
            "AN6 - Volume",
            "OUT - Out of Stocks",
            "R81 - Refusals",
            "Z90 - Supply Chain Loss",
            "ZIA - Inventory",
            "Credit360",
            "Roadnet",
            "Dayforce"});
            this.rankerSelectorCheckedListBox.Location = new System.Drawing.Point(12, 90);
            this.rankerSelectorCheckedListBox.Name = "rankerSelectorCheckedListBox";
            this.rankerSelectorCheckedListBox.Size = new System.Drawing.Size(404, 94);
            this.rankerSelectorCheckedListBox.TabIndex = 20;
            this.rankerSelectorCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.rankerSelectorCheckedListBox_ItemCheck);
            // 
            // branchSelectorCheckedListBox
            // 
            this.branchSelectorCheckedListBox.CheckOnClick = true;
            this.branchSelectorCheckedListBox.FormattingEnabled = true;
            this.branchSelectorCheckedListBox.Items.AddRange(new object[] {
            "All",
            "Beach Cities",
            "Boston",
            "Canton",
            "Denver",
            "Hawaii",
            "Lima",
            "Louisville",
            "Loveland",
            "New Jersey",
            "New York",
            "Oakland",
            "Oklahoma City",
            "Oregon",
            "Pomona",
            "Portland",
            "Renton",
            "Riverside",
            "San Diego",
            "Sylmar",
            "Tulsa"});
            this.branchSelectorCheckedListBox.Location = new System.Drawing.Point(12, 224);
            this.branchSelectorCheckedListBox.Name = "branchSelectorCheckedListBox";
            this.branchSelectorCheckedListBox.Size = new System.Drawing.Size(404, 94);
            this.branchSelectorCheckedListBox.TabIndex = 21;
            this.branchSelectorCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.branchSelectorCheckedListBox_ItemCheck);
            // 
            // EndDatePicker
            // 
            this.EndDatePicker.Location = new System.Drawing.Point(222, 373);
            this.EndDatePicker.Name = "EndDatePicker";
            this.EndDatePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDatePicker.TabIndex = 22;
            // 
            // endDateLabel
            // 
            this.endDateLabel.AutoSize = true;
            this.endDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDateLabel.Location = new System.Drawing.Point(219, 354);
            this.endDateLabel.Name = "endDateLabel";
            this.endDateLabel.Size = new System.Drawing.Size(64, 16);
            this.endDateLabel.TabIndex = 23;
            this.endDateLabel.Text = "End Date";
            // 
            // dataTypeLabel
            // 
            this.dataTypeLabel.AutoSize = true;
            this.dataTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataTypeLabel.Location = new System.Drawing.Point(8, 67);
            this.dataTypeLabel.Name = "dataTypeLabel";
            this.dataTypeLabel.Size = new System.Drawing.Size(230, 20);
            this.dataTypeLabel.TabIndex = 24;
            this.dataTypeLabel.Text = "Step 1) Select Data Categories";
            // 
            // branchLabel
            // 
            this.branchLabel.AutoSize = true;
            this.branchLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.branchLabel.Location = new System.Drawing.Point(8, 201);
            this.branchLabel.Name = "branchLabel";
            this.branchLabel.Size = new System.Drawing.Size(182, 20);
            this.branchLabel.TabIndex = 25;
            this.branchLabel.Text = "Step 2) Select Branches";
            // 
            // dateSelectorsLabel
            // 
            this.dateSelectorsLabel.AutoSize = true;
            this.dateSelectorsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateSelectorsLabel.Location = new System.Drawing.Point(12, 334);
            this.dateSelectorsLabel.Name = "dateSelectorsLabel";
            this.dateSelectorsLabel.Size = new System.Drawing.Size(258, 20);
            this.dateSelectorsLabel.TabIndex = 26;
            this.dateSelectorsLabel.Text = "Step 3) Select Begin and End Date";
            // 
            // currentProgressLabel
            // 
            this.currentProgressLabel.AutoSize = true;
            this.currentProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentProgressLabel.Location = new System.Drawing.Point(528, 224);
            this.currentProgressLabel.Name = "currentProgressLabel";
            this.currentProgressLabel.Size = new System.Drawing.Size(177, 16);
            this.currentProgressLabel.TabIndex = 27;
            this.currentProgressLabel.Text = "Progress - Current Operation";
            // 
            // overallProgressLabel
            // 
            this.overallProgressLabel.AutoSize = true;
            this.overallProgressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overallProgressLabel.Location = new System.Drawing.Point(528, 142);
            this.overallProgressLabel.Name = "overallProgressLabel";
            this.overallProgressLabel.Size = new System.Drawing.Size(116, 16);
            this.overallProgressLabel.TabIndex = 28;
            this.overallProgressLabel.Text = "Progress - Overall";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 20);
            this.label1.TabIndex = 29;
            this.label1.Text = "Step 4) Confirm Data Update";
            // 
            // uploadDataLabel
            // 
            this.uploadDataLabel.AutoSize = true;
            this.uploadDataLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadDataLabel.Location = new System.Drawing.Point(14, 500);
            this.uploadDataLabel.Name = "uploadDataLabel";
            this.uploadDataLabel.Size = new System.Drawing.Size(155, 20);
            this.uploadDataLabel.TabIndex = 30;
            this.uploadDataLabel.Text = "Step 5) Upload Data";
            // 
            // uploadToAccessButton
            // 
            this.uploadToAccessButton.Location = new System.Drawing.Point(12, 530);
            this.uploadToAccessButton.Name = "uploadToAccessButton";
            this.uploadToAccessButton.Size = new System.Drawing.Size(202, 50);
            this.uploadToAccessButton.TabIndex = 31;
            this.uploadToAccessButton.Text = "Upload to Access (Old)";
            this.uploadToAccessButton.UseVisualStyleBackColor = true;
            // 
            // uploadToSQLButton
            // 
            this.uploadToSQLButton.Location = new System.Drawing.Point(225, 530);
            this.uploadToSQLButton.Name = "uploadToSQLButton";
            this.uploadToSQLButton.Size = new System.Drawing.Size(197, 50);
            this.uploadToSQLButton.TabIndex = 32;
            this.uploadToSQLButton.Text = "Upload To SQL (New)";
            this.uploadToSQLButton.UseVisualStyleBackColor = true;
            // 
            // updateOverwriteEntriesButton
            // 
            this.updateOverwriteEntriesButton.Location = new System.Drawing.Point(228, 433);
            this.updateOverwriteEntriesButton.Name = "updateOverwriteEntriesButton";
            this.updateOverwriteEntriesButton.Size = new System.Drawing.Size(197, 50);
            this.updateOverwriteEntriesButton.TabIndex = 33;
            this.updateOverwriteEntriesButton.Text = "Update with Overwrite";
            this.updateOverwriteEntriesButton.UseVisualStyleBackColor = true;
            // 
            // updateDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1033, 685);
            this.Controls.Add(this.updateOverwriteEntriesButton);
            this.Controls.Add(this.uploadToSQLButton);
            this.Controls.Add(this.uploadToAccessButton);
            this.Controls.Add(this.uploadDataLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.overallProgressLabel);
            this.Controls.Add(this.currentProgressLabel);
            this.Controls.Add(this.dateSelectorsLabel);
            this.Controls.Add(this.branchLabel);
            this.Controls.Add(this.dataTypeLabel);
            this.Controls.Add(this.endDateLabel);
            this.Controls.Add(this.EndDatePicker);
            this.Controls.Add(this.branchSelectorCheckedListBox);
            this.Controls.Add(this.rankerSelectorCheckedListBox);
            this.Controls.Add(this.startDateLabel);
            this.Controls.Add(this.startDatePicker);
            this.Controls.Add(this.convertDataButton);
            this.Controls.Add(this.updateNewEntriesButton);
            this.Controls.Add(this.copyDataButton);
            this.Controls.Add(this.progressCurrentRanker);
            this.Controls.Add(this.mainMessageLabel);
            this.Controls.Add(this.progressOverall);
            this.Controls.Add(this.AppendDataButton);
            this.Controls.Add(this.abortUpdateButton);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "updateDataForm";
            this.Text = "Update Data Control Panel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button abortUpdateButton;
        private System.Windows.Forms.Button AppendDataButton;
        private System.Windows.Forms.ProgressBar progressOverall;
        private System.Windows.Forms.Label mainMessageLabel;
        private System.Windows.Forms.ProgressBar progressCurrentRanker;
        private System.Windows.Forms.Button copyDataButton;
        private System.Windows.Forms.Button updateNewEntriesButton;
        private System.Windows.Forms.Button convertDataButton;
        private System.Windows.Forms.DateTimePicker startDatePicker;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.CheckedListBox rankerSelectorCheckedListBox;
        private System.Windows.Forms.CheckedListBox branchSelectorCheckedListBox;
        private System.Windows.Forms.DateTimePicker EndDatePicker;
        private System.Windows.Forms.Label endDateLabel;
        private System.Windows.Forms.Label dataTypeLabel;
        private System.Windows.Forms.Label branchLabel;
        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Label dateSelectorsLabel;
        private System.Windows.Forms.Label currentProgressLabel;
        private System.Windows.Forms.Label overallProgressLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label uploadDataLabel;
        private System.Windows.Forms.Button uploadToAccessButton;
        private System.Windows.Forms.Button uploadToSQLButton;
        private System.Windows.Forms.Button updateOverwriteEntriesButton;
    }
}