using NLog;
using System;
using System.Threading;
using System.Windows.Forms;


namespace HL7ProcessorWinForms
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.startStopButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.messageListView = new System.Windows.Forms.ListView();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.filterButton = new System.Windows.Forms.Button();
            this.exportLogButton = new System.Windows.Forms.Button();
            this.resendContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.resendMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dbQueryButton = new System.Windows.Forms.Button();
            this.statsLabel = new System.Windows.Forms.Label();
            this.exportMessagesButton = new System.Windows.Forms.Button();
            this.toggleThemeButton = new System.Windows.Forms.Button();
            this.resendContextMenu.SuspendLayout();
            this.SuspendLayout();
         
            // 
            // startStopButton
            // 
            this.startStopButton.Location = new System.Drawing.Point(12, 12);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(100, 30);
            this.startStopButton.TabIndex = 0;
            this.startStopButton.Text = "Start Listener";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(12, 48);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(760, 150);
            this.logTextBox.TabIndex = 1;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 454);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 13);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Stopped";
            // 
            // messageListView
            // 
            this.messageListView.ContextMenuStrip = this.resendContextMenu;
            this.messageListView.FullRowSelect = true;
            this.messageListView.Location = new System.Drawing.Point(12, 204);
            this.messageListView.Name = "messageListView";
            this.messageListView.Size = new System.Drawing.Size(760, 190);
            this.messageListView.TabIndex = 3;
            this.messageListView.UseCompatibleStateImageBehavior = false;
            this.messageListView.View = System.Windows.Forms.View.Details;
            this.messageListView.Columns.Add("Message ID", 120);
            this.messageListView.Columns.Add("Type", 80);
            this.messageListView.Columns.Add("Patient ID", 80);
            this.messageListView.Columns.Add("Patient Name", 120);
            this.messageListView.Columns.Add("Message DateTime", 120);
            this.messageListView.Columns.Add("Patient Class", 80); // PV1-2
            this.messageListView.Columns.Add("Assigned Location", 120); // PV1-3
            this.messageListView.Columns.Add("Admission Type", 100); // PV1-4
            this.messageListView.Columns.Add("Attending Doctor", 120); // PV1-7
            this.messageListView.Columns.Add("Event Type Code", 100); // EVN-1
            this.messageListView.Columns.Add("Event DateTime", 120); // EVN-2
            this.messageListView.Columns.Add("Observations", 150); // OBX
            this.messageListView.Columns.Add("Status", 80);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(118, 12);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(150, 20);
            this.filterTextBox.TabIndex = 4;
            this.filterTextBox.Text = "Filter by Type or ID";
            // 
            // filterButton
            // 
            this.filterButton.Location = new System.Drawing.Point(274, 12);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(75, 30);
            this.filterButton.TabIndex = 5;
            this.filterButton.Text = "Filter";
            this.filterButton.UseVisualStyleBackColor = true;
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // exportLogButton
            // 
            this.exportLogButton.Location = new System.Drawing.Point(355, 12);
            this.exportLogButton.Name = "exportLogButton";
            this.exportLogButton.Size = new System.Drawing.Size(100, 30);
            this.exportLogButton.TabIndex = 6;
            this.exportLogButton.Text = "Export Log";
            this.exportLogButton.UseVisualStyleBackColor = true;
            this.exportLogButton.Click += new System.EventHandler(this.ExportLogButton_Click);
            // 
            // resendContextMenu
            // 
            this.resendContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.resendMenuItem,
                this.editMenuItem});
            this.resendContextMenu.Name = "resendContextMenu";
            this.resendContextMenu.Size = new System.Drawing.Size(153, 48);
            // 
            // resendMenuItem
            // 
            this.resendMenuItem.Name = "resendMenuItem";
            this.resendMenuItem.Size = new System.Drawing.Size(152, 22);
            this.resendMenuItem.Text = "Resend Message";
            this.resendMenuItem.Click += new System.EventHandler(this.ResendMessage_Click);
            // 
            // editMenuItem
            // 
            this.editMenuItem.Name = "editMenuItem";
            this.editMenuItem.Size = new System.Drawing.Size(152, 22);
            this.editMenuItem.Text = "Edit Message";
            this.editMenuItem.Click += new System.EventHandler(this.EditMessage_Click);
            // 
            // dbQueryButton
            // 
            this.dbQueryButton.Location = new System.Drawing.Point(461, 12);
            this.dbQueryButton.Name = "dbQueryButton";
            this.dbQueryButton.Size = new System.Drawing.Size(100, 30);
            this.dbQueryButton.TabIndex = 7;
            this.dbQueryButton.Text = "Database Query";
            this.dbQueryButton.UseVisualStyleBackColor = true;
            this.dbQueryButton.Click += new System.EventHandler(this.DatabaseQuery_Click);
            // 
            // statsLabel
            // 
            this.statsLabel.AutoSize = true;
            this.statsLabel.Location = new System.Drawing.Point(12, 400);
            this.statsLabel.Name = "statsLabel";
            this.statsLabel.Size = new System.Drawing.Size(35, 13);
            this.statsLabel.TabIndex = 8;
            this.statsLabel.Text = "Messages: 0 | Queue: 0";
            // 
            // exportMessagesButton
            // 
            this.exportMessagesButton.Location = new System.Drawing.Point(567, 12);
            this.exportMessagesButton.Name = "exportMessagesButton";
            this.exportMessagesButton.Size = new System.Drawing.Size(100, 30);
            this.exportMessagesButton.TabIndex = 9;
            this.exportMessagesButton.Text = "Export Messages";
            this.exportMessagesButton.UseVisualStyleBackColor = true;
            this.exportMessagesButton.Click += new System.EventHandler(this.ExportMessagesButton_Click);
            // 
            // toggleThemeButton
            // 
            this.toggleThemeButton.Location = new System.Drawing.Point(673, 12);
            this.toggleThemeButton.Name = "toggleThemeButton";
            this.toggleThemeButton.Size = new System.Drawing.Size(100, 30);
            this.toggleThemeButton.TabIndex = 10;
            this.toggleThemeButton.Text = "Toggle Theme";
            this.toggleThemeButton.UseVisualStyleBackColor = true;
            this.toggleThemeButton.Click += new System.EventHandler(this.ToggleTheme_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 812);
            this.Controls.Add(this.toggleThemeButton);
            this.Controls.Add(this.exportMessagesButton);
            this.Controls.Add(this.statsLabel);
            this.Controls.Add(this.dbQueryButton);
            this.Controls.Add(this.exportLogButton);
            this.Controls.Add(this.filterButton);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.messageListView);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.startStopButton);
            this.Name = "Form1";
            this.Text = "HL7 Processor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.resendContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button startStopButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ListView messageListView;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Button filterButton;
        private System.Windows.Forms.Button exportLogButton;
        private System.Windows.Forms.ContextMenuStrip resendContextMenu;
        private System.Windows.Forms.ToolStripMenuItem resendMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editMenuItem;
        private System.Windows.Forms.Button dbQueryButton;
        private System.Windows.Forms.Label statsLabel;
        private System.Windows.Forms.Button exportMessagesButton;
        private System.Windows.Forms.Button toggleThemeButton;
    }
}