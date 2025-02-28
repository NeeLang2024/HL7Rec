using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace HL7ProcessorWinForms
{
    public static class DatabaseHelper
    {
        private static string connectionString;

        public static void InitializeDatabase(string connString)
        {
            connectionString = connString;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS HL7Messages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        MessageType TEXT,
                        PatientID TEXT,
                        PatientName TEXT,
                        DateOfBirth TEXT,
                        Gender TEXT,
                        MessageDateTime TEXT,
                        ReceivedDateTime TEXT,
                        PatientClass TEXT, -- PV1-2
                        AssignedLocation TEXT, -- PV1-3
                        AdmissionType TEXT, -- PV1-4
                        AttendingDoctor TEXT, -- PV1-7
                        EventTypeCode TEXT, -- EVN-1
                        EventDateTime TEXT, -- EVN-2
                        Observations TEXT -- OBX (JSON或字符串格式)
                    )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static async Task StoreHL7MessageAsync(HL7Message message)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string insertQuery = @"
                    INSERT INTO HL7Messages (
                        MessageType, PatientID, PatientName, 
                        DateOfBirth, Gender, MessageDateTime, 
                        ReceivedDateTime, PatientClass, AssignedLocation, 
                        AdmissionType, AttendingDoctor, EventTypeCode, 
                        EventDateTime, Observations
                    ) VALUES (
                        @MessageType, @PatientID, @PatientName, 
                        @DateOfBirth, @Gender, @MessageDateTime, 
                        @ReceivedDateTime, @PatientClass, @AssignedLocation, 
                        @AdmissionType, @AttendingDoctor, @EventTypeCode, 
                        @EventDateTime, @Observations
                    )";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@MessageType", message.MessageType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PatientID", message.Patient.PatientID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@PatientName", message.Patient.Name ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", message.Patient.DateOfBirth.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Gender", message.Patient.Gender ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@MessageDateTime", message.MessageDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@ReceivedDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@PatientClass", message.Visit.PatientClass ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AssignedLocation", message.Visit.AssignedLocation ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AdmissionType", message.Visit.AdmissionType ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@AttendingDoctor", message.Visit.AttendingDoctor ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EventTypeCode", message.Event.EventTypeCode ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EventDateTime", message.Event.RecordedDateTime.ToString("yyyy-MM-dd HH:mm:ss") ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Observations", string.Join(";", message.Observations.Select(o => $"{o.ObservationID}:{o.Value}:{o.Units}:{o.ReferenceRange}:{o.AbnormalFlags}")) ?? (object)DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}