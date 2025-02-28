using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace HL7ProcessorWinForms
{
    public partial class DatabaseQueryForm : Form
    {
        private readonly string connectionString = Program.Configuration["Database:ConnectionString"];

        public DatabaseQueryForm()
        {
            InitializeComponent();
            LoadMessages();
        }

        private void LoadMessages(string filter = "")
        {
            messageListView.Items.Clear();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM HL7Messages WHERE PatientID LIKE @filter OR MessageType LIKE @filter";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@filter", $"%{filter}%");
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["Id"].ToString());
                            item.SubItems.Add(reader["MessageType"].ToString());
                            item.SubItems.Add(reader["PatientID"].ToString());
                            item.SubItems.Add(reader["PatientName"].ToString());
                            item.SubItems.Add(reader["DateOfBirth"].ToString());
                            item.SubItems.Add(reader["Gender"].ToString());
                            item.SubItems.Add(reader["MessageDateTime"].ToString());
                            messageListView.Items.Add(item);
                        }
                    }
                }
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            LoadMessages(searchTextBox.Text);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (messageListView.SelectedItems.Count > 0)
            {
                var item = messageListView.SelectedItems[0];
                string id = item.Text;
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM HL7Messages WHERE Id = @id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
                messageListView.Items.Remove(item);
            }
        }
    }
}