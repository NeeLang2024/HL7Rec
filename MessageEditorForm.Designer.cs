namespace HL7ProcessorWinForms
{
    partial class MessageEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.messageIdLabel = new System.Windows.Forms.Label();
            this.messageIdTextBox = new System.Windows.Forms.TextBox();
            this.messageTypeLabel = new System.Windows.Forms.Label();
            this.messageTypeTextBox = new System.Windows.Forms.TextBox();
            this.dateTimeLabel = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.patientIdLabel = new System.Windows.Forms.Label();
            this.patientIdTextBox = new System.Windows.Forms.TextBox();
            this.patientNameLabel = new System.Windows.Forms.Label();
            this.patientNameTextBox = new System.Windows.Forms.TextBox();
            this.dobLabel = new System.Windows.Forms.Label();
            this.dobPicker = new System.Windows.Forms.DateTimePicker();
            this.genderLabel = new System.Windows.Forms.Label();
            this.genderTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // messageIdLabel
            // 
            this.messageIdLabel.AutoSize = true;
            this.messageIdLabel.Location = new System.Drawing.Point(12, 15);
            this.messageIdLabel.Name = "messageIdLabel";
            this.messageIdLabel.Size = new System.Drawing.Size(65, 13);
            this.messageIdLabel.TabIndex = 0;
            this.messageIdLabel.Text = "Message ID:";
            // 
            // messageIdTextBox
            // 
            this.messageIdTextBox.Location = new System.Drawing.Point(120, 12);
            this.messageIdTextBox.Name = "messageIdTextBox";
            this.messageIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.messageIdTextBox.TabIndex = 1;
            // 
            // messageTypeLabel
            // 
            this.messageTypeLabel.AutoSize = true;
            this.messageTypeLabel.Location = new System.Drawing.Point(12, 41);
            this.messageTypeLabel.Name = "messageTypeLabel";
            this.messageTypeLabel.Size = new System.Drawing.Size(75, 13);
            this.messageTypeLabel.TabIndex = 2;
            this.messageTypeLabel.Text = "Message Type:";
            // 
            // messageTypeTextBox
            // 
            this.messageTypeTextBox.Location = new System.Drawing.Point(120, 38);
            this.messageTypeTextBox.Name = "messageTypeTextBox";
            this.messageTypeTextBox.Size = new System.Drawing.Size(200, 20);
            this.messageTypeTextBox.TabIndex = 3;
            // 
            // dateTimeLabel
            // 
            this.dateTimeLabel.AutoSize = true;
            this.dateTimeLabel.Location = new System.Drawing.Point(12, 67);
            this.dateTimeLabel.Name = "dateTimeLabel";
            this.dateTimeLabel.Size = new System.Drawing.Size(61, 13);
            this.dateTimeLabel.TabIndex = 4;
            this.dateTimeLabel.Text = "DateTime:";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePicker.Location = new System.Drawing.Point(120, 64);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker.TabIndex = 5;
            // 
            // patientIdLabel
            // 
            this.patientIdLabel.AutoSize = true;
            this.patientIdLabel.Location = new System.Drawing.Point(12, 93);
            this.patientIdLabel.Name = "patientIdLabel";
            this.patientIdLabel.Size = new System.Drawing.Size(58, 13);
            this.patientIdLabel.TabIndex = 6;
            this.patientIdLabel.Text = "Patient ID:";
            // 
            // patientIdTextBox
            // 
            this.patientIdTextBox.Location = new System.Drawing.Point(120, 90);
            this.patientIdTextBox.Name = "patientIdTextBox";
            this.patientIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.patientIdTextBox.TabIndex = 7;
            // 
            // patientNameLabel
            // 
            this.patientNameLabel.AutoSize = true;
            this.patientNameLabel.Location = new System.Drawing.Point(12, 119);
            this.patientNameLabel.Name = "patientNameLabel";
            this.patientNameLabel.Size = new System.Drawing.Size(79, 13);
            this.patientNameLabel.TabIndex = 8;
            this.patientNameLabel.Text = "Patient Name:";
            // 
            // patientNameTextBox
            // 
            this.patientNameTextBox.Location = new System.Drawing.Point(120, 116);
            this.patientNameTextBox.Name = "patientNameTextBox";
            this.patientNameTextBox.Size = new System.Drawing.Size(200, 20);
            this.patientNameTextBox.TabIndex = 9;
            // 
            // dobLabel
            // 
            this.dobLabel.AutoSize = true;
            this.dobLabel.Location = new System.Drawing.Point(12, 145);
            this.dobLabel.Name = "dobLabel";
            this.dobLabel.Size = new System.Drawing.Size(70, 13);
            this.dobLabel.TabIndex = 10;
            this.dobLabel.Text = "Date of Birth:";
            // 
            // dobPicker
            // 
            this.dobPicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dobPicker.CustomFormat = "yyyy-MM-dd";
            this.dobPicker.Location = new System.Drawing.Point(120, 142);
            this.dobPicker.Name = "dobPicker";
            this.dobPicker.Size = new System.Drawing.Size(200, 20);
            this.dobPicker.TabIndex = 11;
            // 
            // genderLabel
            // 
            this.genderLabel.AutoSize = true;
            this.genderLabel.Location = new System.Drawing.Point(12, 171);
            this.genderLabel.Name = "genderLabel";
            this.genderLabel.Size = new System.Drawing.Size(43, 13);
            this.genderLabel.TabIndex = 12;
            this.genderLabel.Text = "Gender:";
            // 
            // genderTextBox
            // 
            this.genderTextBox.Location = new System.Drawing.Point(120, 168);
            this.genderTextBox.Name = "genderTextBox";
            this.genderTextBox.Size = new System.Drawing.Size(200, 20);
            this.genderTextBox.TabIndex = 13;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(120, 200);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 30);
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(201, 200);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 30);
            this.cancelButton.TabIndex = 15;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // MessageEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 242);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.genderTextBox);
            this.Controls.Add(this.genderLabel);
            this.Controls.Add(this.dobPicker);
            this.Controls.Add(this.dobLabel);
            this.Controls.Add(this.patientNameTextBox);
            this.Controls.Add(this.patientNameLabel);
            this.Controls.Add(this.patientIdTextBox);
            this.Controls.Add(this.patientIdLabel);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.dateTimeLabel);
            this.Controls.Add(this.messageTypeTextBox);
            this.Controls.Add(this.messageTypeLabel);
            this.Controls.Add(this.messageIdTextBox);
            this.Controls.Add(this.messageIdLabel);
            this.Name = "MessageEditorForm";
            this.Text = "Edit HL7 Message";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label messageIdLabel;
        private System.Windows.Forms.TextBox messageIdTextBox;
        private System.Windows.Forms.Label messageTypeLabel;
        private System.Windows.Forms.TextBox messageTypeTextBox;
        private System.Windows.Forms.Label dateTimeLabel;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label patientIdLabel;
        private System.Windows.Forms.TextBox patientIdTextBox;
        private System.Windows.Forms.Label patientNameLabel;
        private System.Windows.Forms.TextBox patientNameTextBox;
        private System.Windows.Forms.Label dobLabel;
        private System.Windows.Forms.DateTimePicker dobPicker;
        private System.Windows.Forms.Label genderLabel;
        private System.Windows.Forms.TextBox genderTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
    }
}