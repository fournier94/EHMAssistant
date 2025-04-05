using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EHMAssistant
{
    public partial class EligiblePlayersFromFile : Form
    {
        // Dictionary to store eligible players with line numbers as keys and player names as values
        private Dictionary<int, string> eligiblePlayers = new Dictionary<int, string>();

        public EligiblePlayersFromFile()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set form properties
            this.Text = "EHM Eligible Players";
            this.Size = new Size(600, 500);

            // Create button to browse for EHM file
            Button btnBrowse = new Button
            {
                Text = "Browse for EHM File",
                Location = new Point(20, 20),
                Size = new Size(150, 30)
            };
            btnBrowse.Click += BtnBrowse_Click;
            this.Controls.Add(btnBrowse);

            // Create label to show file status
            Label lblStatus = new Label
            {
                Name = "lblStatus",
                Location = new Point(180, 25),
                Size = new Size(400, 20),
                Text = "No file loaded"
            };
            this.Controls.Add(lblStatus);

            // Create ListBox to display eligible players
            ListBox listBoxPlayers = new ListBox
            {
                Name = "listBoxPlayers",
                Location = new Point(20, 60),
                Size = new Size(540, 380),
                Font = new Font("Consolas", 10)
            };
            this.Controls.Add(listBoxPlayers);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "EHM Files (*.ehm)|*.ehm|All Files (*.*)|*.*";
                openFileDialog.Title = "Select an EHM Player File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Update status label
                        Label lblStatus = this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblStatus");
                        if (lblStatus != null)
                        {
                            lblStatus.Text = $"Processing file: {Path.GetFileName(openFileDialog.FileName)}";
                            Application.DoEvents(); // Update UI
                        }

                        // Process the selected file
                        ProcessEHMFile(openFileDialog.FileName);

                        // Display the eligible players in the ListBox
                        DisplayEligiblePlayers();

                        // Update status label
                        if (lblStatus != null)
                        {
                            lblStatus.Text = $"File loaded: {Path.GetFileName(openFileDialog.FileName)} - Found {eligiblePlayers.Count} eligible players";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProcessEHMFile(string filePath)
        {
            // Clear existing data
            eligiblePlayers.Clear();

            // Read all lines from the file
            string[] allLines = File.ReadAllLines(filePath);

            // Skip the first line as specified
            if (allLines.Length <= 1)
            {
                MessageBox.Show("The file is too short to contain any player data.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Process each player (each player takes 20 lines)
            for (int i = 1; i < allLines.Length - 19; i += 20)
            {
                try
                {
                    // Check if we have enough lines left for a complete player entry
                    if (i + 19 >= allLines.Length)
                        break;

                    // Check the condition: player has a value of 99 at the end of the third line
                    string thirdLine = allLines[i + 2];
                    string[] thirdLineValues = thirdLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (thirdLineValues.Length > 0 && thirdLineValues[thirdLineValues.Length - 1] == "99")
                    {
                        // Get the player's name from line 14 (which is at index i+13)
                        string playerName = allLines[i + 13].Trim();

                        // Add the player to our dictionary with line number as key
                        eligiblePlayers.Add(i, playerName);
                    }
                }
                catch (Exception ex)
                {
                    // Log errors but continue processing
                    Console.WriteLine($"Error processing player at line {i}: {ex.Message}");
                }
            }
        }

        private void DisplayEligiblePlayers()
        {
            ListBox listBox = this.Controls.OfType<ListBox>().FirstOrDefault(lb => lb.Name == "listBoxPlayers");

            if (listBox != null)
            {
                listBox.Items.Clear();

                if (eligiblePlayers.Count == 0)
                {
                    listBox.Items.Add("No eligible players found in the file.");
                    return;
                }

                listBox.Items.Add($"Found {eligiblePlayers.Count} eligible players:");
                listBox.Items.Add("");

                foreach (var player in eligiblePlayers)
                {
                    listBox.Items.Add($"Line: {player.Key} - Player: {player.Value}");
                }
            }
        }
    }
}