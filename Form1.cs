using NLog;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using CsvHelper;

namespace HL7ProcessorWinForms
{
    public partial class Form1 : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private HL7Listener listener;
        private bool isRunning = false;
        private int messageCount = 0;
        private bool isDisposed = false;
        private System.Windows.Forms.Timer statsTimer;
        private Chart messageChart;
        private bool isDarkTheme = false;

        public Form1()
        {
            InitializeComponent();
            LoadSettings();
            Logger.Info("Application started");
            UpdateLog("Application started");
            UpdateStats();

            // 确保messageChart初始化
            if (messageChart == null)
            {
                messageChart = new Chart();
                messageChart.Location = new System.Drawing.Point(12, 600);
                messageChart.Size = new System.Drawing.Size(760, 200);
                messageChart.ChartAreas.Add(new ChartArea("Messages"));
                messageChart.Series.Add(new Series("MessagesPerMinute")
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.DateTime
                });
                messageChart.Series.Add(new Series("QueueLength")
                {
                    ChartType = SeriesChartType.Line,
                    XValueType = ChartValueType.DateTime,
                    YAxisType = AxisType.Secondary
                });
                this.Controls.Add(messageChart);
            }

            // 启动定时器，每分钟更新图表
            statsTimer = new System.Windows.Forms.Timer();
            statsTimer.Interval = 60000; // 1分钟
            statsTimer.Tick += StatsTimer_Tick;
            statsTimer.Start();

            ApplyTheme(); // 初始应用主题
        }

        private void ApplyTheme()
        {
            if (isDarkTheme)
            {
                this.BackColor = System.Drawing.Color.FromArgb(40, 40, 40);
                this.ForeColor = System.Drawing.Color.White;
                logTextBox.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                logTextBox.ForeColor = System.Drawing.Color.White;
                messageListView.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                messageListView.ForeColor = System.Drawing.Color.White;
                statusLabel.ForeColor = System.Drawing.Color.White;
                statsLabel.ForeColor = System.Drawing.Color.White;
                filterTextBox.BackColor = System.Drawing.Color.FromArgb(60, 60, 60);
                filterTextBox.ForeColor = System.Drawing.Color.White;
                startStopButton.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                startStopButton.ForeColor = System.Drawing.Color.White;
                filterButton.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                filterButton.ForeColor = System.Drawing.Color.White;
                exportLogButton.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                exportLogButton.ForeColor = System.Drawing.Color.White;
                exportMessagesButton.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                exportMessagesButton.ForeColor = System.Drawing.Color.White;
                dbQueryButton.BackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                dbQueryButton.ForeColor = System.Drawing.Color.White;
                if (messageChart != null && !messageChart.IsDisposed)
                {
                    messageChart.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
                    messageChart.ForeColor = System.Drawing.Color.White;
                }
            }
            else
            {
                this.BackColor = System.Drawing.Color.White;
                this.ForeColor = System.Drawing.Color.Black;
                logTextBox.BackColor = System.Drawing.Color.White;
                logTextBox.ForeColor = System.Drawing.Color.Black;
                messageListView.BackColor = System.Drawing.Color.White;
                messageListView.ForeColor = System.Drawing.Color.Black;
                statusLabel.ForeColor = System.Drawing.Color.Black;
                statsLabel.ForeColor = System.Drawing.Color.Black;
                filterTextBox.BackColor = System.Drawing.Color.White;
                filterTextBox.ForeColor = System.Drawing.Color.Black;
                startStopButton.BackColor = System.Drawing.SystemColors.Control;
                startStopButton.ForeColor = System.Drawing.Color.Black;
                filterButton.BackColor = System.Drawing.SystemColors.Control;
                filterButton.ForeColor = System.Drawing.Color.Black;
                exportLogButton.BackColor = System.Drawing.SystemColors.Control;
                exportLogButton.ForeColor = System.Drawing.Color.Black;
                exportMessagesButton.BackColor = System.Drawing.SystemColors.Control;
                exportMessagesButton.ForeColor = System.Drawing.Color.Black;
                dbQueryButton.BackColor = System.Drawing.SystemColors.Control;
                dbQueryButton.ForeColor = System.Drawing.Color.Black;
                if (messageChart != null && !messageChart.IsDisposed)
                {
                    messageChart.BackColor = System.Drawing.Color.White;
                    messageChart.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void ToggleTheme_Click(object sender, EventArgs e)
        {
            isDarkTheme = !isDarkTheme;
            ApplyTheme();
            UpdateLog($"Switched to {(isDarkTheme ? "Dark" : "Light")} theme");
        }

        private void StatsTimer_Tick(object sender, EventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart() // 添加UpdateChart方法
        {
            if (!isDisposed && !messageChart.IsDisposed)
            {
                messageChart.Series["MessagesPerMinute"].Points.AddXY(DateTime.Now, messageCount);
                messageChart.Series["QueueLength"].Points.AddXY(DateTime.Now, listener?.QueueLength ?? 0);
                if (messageChart.Series["MessagesPerMinute"].Points.Count > 60) // 保持1小时数据
                {
                    messageChart.Series["MessagesPerMinute"].Points.RemoveAt(0);
                    messageChart.Series["QueueLength"].Points.RemoveAt(0);
                }
            }
        }

        private async void StartStopButton_Click(object sender, EventArgs e)
        {
            if (!isRunning)
            {
                await StartListenerAsync();
            }
            else
            {
                StopListener();
            }
        }

        private async Task StartListenerAsync()
        {
            int port;
            if (!int.TryParse(Program.Configuration["HL7Listener:Port"], out port))
            {
                port = 5555;
                UpdateLog("Invalid port in configuration, using default: 5555");
            }
            listener = new HL7Listener(this);
            isRunning = true;
            startStopButton.Text = "Stop Listener";
            statusLabel.Text = "Running";
            UpdateLog($"Starting listener on port {port}");
            try
            {
                await listener.StartAsync(port);
            }
            catch (Exception ex)
            {
                UpdateLog($"Failed to start listener: {ex.Message}");
                isRunning = false;
                startStopButton.Text = "Start Listener";
                statusLabel.Text = "Stopped";
            }
        }

        private void StopListener()
        {
            isRunning = false;
            listener?.Stop();
            startStopButton.Text = "Start Listener";
            statusLabel.Text = "Stopped";
            UpdateLog("Listener stopped");
        }

        public void UpdateLog(string message)
        {
            if (isDisposed) return;

            if (logTextBox.InvokeRequired)
            {
                logTextBox.Invoke(new Action<string>(UpdateLog), message);
            }
            else
            {
                if (!logTextBox.IsDisposed)
                {
                    logTextBox.AppendText($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\r\n");
                }
            }
        }

        public void DisplayMessageDetails(HL7Message message, string status = "Processed")
        {
            if (isDisposed) return;

            if (messageListView.InvokeRequired)
            {
                messageListView.Invoke(new Action<HL7Message, string>(DisplayMessageDetails), message, status);
            }
            else
            {
                if (!messageListView.IsDisposed && MatchesFilter(message))
                {
                    ListViewItem item = new ListViewItem(message.MessageControlID ?? "N/A");
                    item.SubItems.Add(message.MessageType ?? "N/A");
                    item.SubItems.Add(message.Patient.PatientID ?? "N/A");
                    item.SubItems.Add(message.Patient.Name ?? "N/A");
                    item.SubItems.Add(message.MessageDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    item.SubItems.Add(message.Visit.PatientClass ?? "N/A"); // PV1-2: Patient Class
                    item.SubItems.Add(message.Visit.AssignedLocation ?? "N/A"); // PV1-3: Assigned Location
                    item.SubItems.Add(message.Visit.AdmissionType ?? "N/A"); // PV1-4: Admission Type
                    item.SubItems.Add(message.Visit.AttendingDoctor ?? "N/A"); // PV1-7: Attending Doctor
                    item.SubItems.Add(message.Event.EventTypeCode ?? "N/A"); // EVN-1: Event Type Code
                    item.SubItems.Add(message.Event.RecordedDateTime.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A"); // EVN-2: Recorded Date/Time
                    string observations = string.Join("; ", message.Observations.Select(o => $"{o.ObservationID}: {o.Value} ({o.Units})"));
                    item.SubItems.Add(observations ?? "N/A"); // OBX: Observations
                    item.SubItems.Add(status);
                    item.Tag = message;
                    messageListView.Items.Add(item);
                    messageCount++;
                    UpdateStats();
                }
            }
        }

        private bool MatchesFilter(HL7Message message)
        {
            string filterText = filterTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText)) return true;
            return (message.MessageType?.ToLower().Contains(filterText) ?? false) ||
                   (message.Patient.PatientID?.ToLower().Contains(filterText) ?? false);
        }

        private void UpdateStats()
        {
            if (!isDisposed && !statsLabel.IsDisposed)
            {
                statsLabel.Text = $"Messages: {messageCount} | Queue: {listener?.QueueLength ?? 0}";
            }
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            messageListView.Items.Clear();
            messageCount = 0;
            UpdateStats();
            UpdateLog("Filter applied: " + filterTextBox.Text);
        }

        private void ExportLogButton_Click(object sender, EventArgs e)
        {
            if (!isDisposed)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt";
                    sfd.FileName = $"HL7_Log_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(sfd.FileName, logTextBox.Text);
                        UpdateLog($"Log exported to {sfd.FileName}");
                    }
                }
            }
        }

        private void ExportMessagesButton_Click(object sender, EventArgs e)
        {
            if (!isDisposed)
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "CSV Files (*.csv)|*.csv";
                    sfd.FileName = $"HL7_Messages_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (var writer = new StreamWriter(sfd.FileName))
                        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            csv.WriteField("Message ID");
                            csv.WriteField("Type");
                            csv.WriteField("Patient ID");
                            csv.WriteField("Patient Name");
                            csv.WriteField("DateTime");
                            csv.WriteField("Status");
                            csv.NextRecord();

                            foreach (ListViewItem item in messageListView.Items)
                            {
                                csv.WriteField(item.Text);
                                for (int i = 1; i < item.SubItems.Count; i++)
                                {
                                    csv.WriteField(item.SubItems[i].Text);
                                }
                                csv.NextRecord();
                            }
                        }
                        UpdateLog($"Messages exported to {sfd.FileName}");
                    }
                }
            }
        }

        private void ResendMessage_Click(object sender, EventArgs e)
        {
            if (!isDisposed && messageListView.SelectedItems.Count > 0)
            {
                var item = messageListView.SelectedItems[0];
                var message = item.Tag as HL7Message;
                if (message != null)
                {
                    ResendMessageAsync(message);
                    UpdateLog($"Resending message: {message.MessageControlID}");
                }
            }
        }

        private void EditMessage_Click(object sender, EventArgs e)
        {
            if (!isDisposed && messageListView.SelectedItems.Count > 0)
            {
                var item = messageListView.SelectedItems[0];
                var message = item.Tag as HL7Message;
                if (message != null)
                {
                    using (var editor = new MessageEditorForm(message))
                    {
                        if (editor.ShowDialog() == DialogResult.OK)
                        {
                            item.SubItems[0].Text = editor.EditedMessage.MessageControlID ?? "N/A";
                            item.SubItems[1].Text = editor.EditedMessage.MessageType ?? "N/A";
                            item.SubItems[2].Text = editor.EditedMessage.Patient.PatientID ?? "N/A";
                            item.SubItems[3].Text = editor.EditedMessage.Patient.Name ?? "N/A";
                            item.SubItems[4].Text = editor.EditedMessage.MessageDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                            item.Tag = editor.EditedMessage;
                            UpdateLog($"Message edited: {editor.EditedMessage.MessageControlID}");
                        }
                    }
                }
            }
        }

        private async void ResendMessageAsync(HL7Message message)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 5555))
                using (NetworkStream stream = client.GetStream())
                {
                    string hl7Message = $"MSH|^~\\&|HL7Processor|Facility|RECEIVER|FACILITY|{DateTime.Now:yyyyMMddHHmmss}||{message.MessageType}|{message.MessageControlID}|P|2.5\r" +
                                       $"PID|1||{message.Patient.PatientID}||{message.Patient.Name}||{message.Patient.DateOfBirth:yyyyMMdd}|{message.Patient.Gender}\r";
                    string mllpMessage = $"\v{hl7Message}\x1c\r";
                    byte[] data = Encoding.UTF8.GetBytes(mllpMessage);
                    await stream.WriteAsync(data, 0, data.Length);
                    await stream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                UpdateLog($"Resend failed: {ex.Message}");
            }
        }

        private void DatabaseQuery_Click(object sender, EventArgs e)
        {
            if (!isDisposed)
            {
                using (var dbForm = new DatabaseQueryForm())
                {
                    dbForm.ShowDialog();
                }
            }
        }

        private void LoadSettings()
        {
            if (!isDisposed)
            {
                filterTextBox.Text = ConfigurationManager.AppSettings["LastFilter"] ?? "";
                if (int.TryParse(ConfigurationManager.AppSettings["WindowLeft"], out int left))
                    this.Left = left;
                if (int.TryParse(ConfigurationManager.AppSettings["WindowTop"], out int top))
                    this.Top = top;
            }
        }

        private void SaveSettings()
        {
            if (!isDisposed)
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("LastFilter");
                config.AppSettings.Settings.Add("LastFilter", filterTextBox.Text);
                config.AppSettings.Settings.Remove("WindowLeft");
                config.AppSettings.Settings.Add("WindowLeft", this.Left.ToString());
                config.AppSettings.Settings.Remove("WindowTop");
                config.AppSettings.Settings.Add("WindowTop", this.Top.ToString());
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // 添加Form1_FormClosing方法
        {
            isDisposed = true;
            if (isRunning)
            {
                StopListener();
            }
            statsTimer?.Stop();
            statsTimer?.Dispose();
            messageChart?.Dispose();
            SaveSettings();
            Logger.Info("Application closing");
        }
    }
}