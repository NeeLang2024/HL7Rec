using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL7ProcessorWinForms
{
    public partial class MessageEditorForm : Form
    {
        public HL7Message EditedMessage { get; private set; }

        public MessageEditorForm(HL7Message message)
        {
            InitializeComponent();
            EditedMessage = new HL7Message
            {
                MessageControlID = message.MessageControlID,
                MessageType = message.MessageType,
                MessageDateTime = message.MessageDateTime,
                Patient = new PatientInfo
                {
                    PatientID = message.Patient.PatientID,
                    Name = message.Patient.Name,
                    DateOfBirth = message.Patient.DateOfBirth,
                    Gender = message.Patient.Gender
                }
            };
            LoadMessage();
        }

        private void LoadMessage()
        {
            messageIdTextBox.Text = EditedMessage.MessageControlID;
            messageTypeTextBox.Text = EditedMessage.MessageType;
            dateTimePicker.Value = EditedMessage.MessageDateTime;
            patientIdTextBox.Text = EditedMessage.Patient.PatientID;
            patientNameTextBox.Text = EditedMessage.Patient.Name;
            dobPicker.Value = EditedMessage.Patient.DateOfBirth;
            genderTextBox.Text = EditedMessage.Patient.Gender;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            EditedMessage.MessageControlID = messageIdTextBox.Text;
            EditedMessage.MessageType = messageTypeTextBox.Text;
            EditedMessage.MessageDateTime = dateTimePicker.Value;
            EditedMessage.Patient.PatientID = patientIdTextBox.Text;
            EditedMessage.Patient.Name = patientNameTextBox.Text;
            EditedMessage.Patient.DateOfBirth = dobPicker.Value;
            EditedMessage.Patient.Gender = genderTextBox.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}