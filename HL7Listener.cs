using NLog;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HL7ProcessorWinForms
{
    public class HL7Listener
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private TcpListener listener;
        private bool isRunning = false;
        private readonly Form1 form;
        private readonly ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private readonly ConcurrentDictionary<string, HL7Message> messageCache = new ConcurrentDictionary<string, HL7Message>();
        private const int MaxCacheSize = 100; // 限制缓存大小为100条
        private Task queueTask;

        public HL7Listener(Form1 form)
        {
            this.form = form;
            queueTask = Task.Run(() => ProcessQueueAsync());
        }

        public async Task StartAsync(int port)
        {
            try
            {
                listener = new TcpListener(System.Net.IPAddress.Any, port);
                listener.Start();
                isRunning = true;
                Logger.Info($"HL7 Listener started on port {port}");
                form.UpdateLog($"HL7 Listener started on port {port}");

                while (isRunning)
                {
                    form.UpdateLog("Waiting for client connection...");
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    form.UpdateLog($"Client connected: {client.Client.RemoteEndPoint}");
                    _ = Task.Run(() => ProcessClientAsync(client));
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in HL7Listener");
                form.UpdateLog($"Error: {ex.Message}");
            }
            finally
            {
                listener?.Stop();
                form.UpdateLog("Listener stopped");
            }
        }

        public void Stop()
        {
            isRunning = false;
            listener?.Stop();
            queueTask?.Wait(2000);
        }

        public int QueueLength => messageQueue.Count;

        private async Task ProcessClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                using (NetworkStream stream = client.GetStream())
                {
                    stream.ReadTimeout = 5000;
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    StringBuilder messageBuilder = new StringBuilder();

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        string chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        messageBuilder.Append(chunk);

                        if (messageBuilder.ToString().EndsWith("\x1c\r"))
                        {
                            break;
                        }
                    }

                    string rawMessage = messageBuilder.ToString();
                    if (string.IsNullOrEmpty(rawMessage))
                    {
                        form.UpdateLog("No data received from client");
                        return;
                    }

                    if (rawMessage.StartsWith("\v") && rawMessage.EndsWith("\x1c\r"))
                    {
                        string hl7Message = rawMessage.Substring(1, rawMessage.Length - 3);
                        Logger.Debug("Received HL7 message: {0}", hl7Message);
                        form.UpdateLog($"Received raw: {rawMessage}");
                        messageQueue.Enqueue(hl7Message);

                        HL7Message parsedMessage = HL7Parser.ParseHL7Message(hl7Message);
                        string ack = HL7Response.GenerateACK(parsedMessage.MessageType, parsedMessage.MessageControlID);
                        string mllpAck = $"\v{ack}\x1c\r";
                        byte[] ackBytes = Encoding.UTF8.GetBytes(mllpAck);
                        await stream.WriteAsync(ackBytes, 0, ackBytes.Length);
                        Logger.Debug("Sent ACK: {0}", ack);
                        form.UpdateLog($"Sent ACK: {ack}");
                    }
                    else
                    {
                        form.UpdateLog("Invalid MLLP format");
                        form.DisplayMessageDetails(new HL7Message(), "Invalid");
                        await SendAlertAsync("Invalid HL7 Message", $"Received invalid MLLP message: {rawMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error processing HL7 message");
                form.UpdateLog($"Processing error: {ex.Message}");
                form.DisplayMessageDetails(new HL7Message(), "Error: " + ex.Message);
                await SendAlertAsync("HL7 Processing Error", $"Error processing message: {ex.Message}");
            }
        }

        private async Task SendAlertAsync(string subject, string body)
        {
            var settingsSection = Program.Configuration.GetSection("AlertSettings");
            var settings = settingsSection.Get<Dictionary<string, string>>();
            bool enabled = settings != null && bool.Parse(settings["Enabled"]);
            if (enabled)
            {
                try
                {
                    using (var client = new System.Net.Mail.SmtpClient(settings["SmtpServer"], int.Parse(settings["Port"]))
                    {
                        EnableSsl = true,
                        Credentials = new System.Net.NetworkCredential(settings["Username"], settings["Password"])
                    })
                    {
                        var mail = new System.Net.Mail.MailMessage
                        {
                            From = new System.Net.Mail.MailAddress(settings["Username"]),
                            Subject = subject,
                            Body = body
                        };
                        mail.To.Add(settings["Recipient"]);
                        await client.SendMailAsync(mail);
                        form.UpdateLog($"Alert sent: {subject}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to send alert");
                    form.UpdateLog($"Failed to send alert: {ex.Message}");
                }
            }
        }

        private async Task ProcessQueueAsync()
        {
            while (true)
            {
                if (messageQueue.TryDequeue(out string hl7Message))
                {
                    try
                    {
                        if (HL7Validator.ValidateHL7Message(hl7Message))
                        {
                            HL7Message parsedMessage = HL7Parser.ParseHL7Message(hl7Message);
                            // 缓存消息
                            CacheMessage(parsedMessage);
                            await DatabaseHelper.StoreHL7MessageAsync(parsedMessage);
                            form.DisplayMessageDetails(parsedMessage, "Processed");
                        }
                        else
                        {
                            form.DisplayMessageDetails(new HL7Message(), "Invalid");
                            await SendAlertAsync("Invalid HL7 Message", $"Invalid HL7 message: {hl7Message}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex, "Error processing queued message");
                        form.UpdateLog($"Queue processing error: {ex.Message}");
                        form.DisplayMessageDetails(new HL7Message(), "Error: " + ex.Message);
                        await SendAlertAsync("HL7 Queue Processing Error", $"Error processing queued message: {ex.Message}");
                    }
                }
                await Task.Delay(100);
            }
        }

        private void CacheMessage(HL7Message message)
        {
            string key = message.MessageControlID ?? Guid.NewGuid().ToString();
            if (messageCache.Count >= MaxCacheSize)
            {
                // 移除最旧的缓存项
                string oldestKey = messageCache.Keys.OrderBy(k => messageCache[k].MessageDateTime).First();
                messageCache.TryRemove(oldestKey, out _);
            }
            messageCache.TryAdd(key, message);
        }

        public HL7Message GetCachedMessage(string messageControlId)
        {
            if (messageCache.TryGetValue(messageControlId, out HL7Message message))
            {
                return message;
            }
            return null;
        }
    }
}