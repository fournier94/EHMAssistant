using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EHMAssistant
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
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

            // When GenerateDraft closes, close the MainMenu as well
            draftForm.FormClosed += (s, args) => this.Close();
        }

        // Vrais joueurs
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // Create the MenuDraftReel form
        //    var draftReelForm = new EHMAssistant.MenuDraftReel();

        //    // Hide the current form (MainMenu)
        //    this.Hide();

        //    // Show the MenuDraftReel form
        //    draftReelForm.Show();

        //    // When MenuDraftReel closes, show the MainMenu again
        //    draftReelForm.FormClosed += (s, args) => this.Show();
        //}
    }
}
