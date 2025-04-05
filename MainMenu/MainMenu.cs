using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using EHMAssistant.ModifyRoster;

namespace EHMAssistant
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center the window
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Text File",
                Filter = "Text files (*.txt)|*.txt",
                RestoreDirectory = true
            };

            // Show the dialog and check if user clicked OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Read the contents of the file
                    string fileContent = System.IO.File.ReadAllText(openFileDialog.FileName);

                    // Do something with the file content
                    // For example, display it in a text box:
                    // textBox1.Text = fileContent;

                    // Or you could process the content here
                    MessageBox.Show("File loaded successfully!", "Success");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Joueurs fictifs
        private void button1_Click(object sender, EventArgs e)
        {
            // Create the GenerateDraft form
            var draftForm = new GenerateDraft();

            // Hide the current form (MainMenu)
            this.Hide();

            // Show the GenerateDraft form
            draftForm.Show();

            // When GenerateDraft closes, show the MainMenu again
            draftForm.FormClosed += (s, args) => this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set filter to only show .ehm files
                openFileDialog.Filter = "EHM Files (*.ehm)|*.ehm";
                openFileDialog.Title = "Veuillez selectionner votre fichier players.ehm";

                // Show dialog and check if the user selected a file
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = openFileDialog.FileName;

                    try
                    {
                        // Read all content from the file
                        string fileContent = File.ReadAllText(filePath);

                        // Example: Process the file content
                        ProcessEHMFile(fileContent);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProcessEHMFile(string content)
        {
            var rosterMenu = new RosterMenu();
            rosterMenu.Show();
        }
    }
}
