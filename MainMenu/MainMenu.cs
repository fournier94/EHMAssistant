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
        #region Variables
        // Store all lines from the file
        private string[] allFileLines;

        private Dictionary<int, string> playerDictionary = new Dictionary<int, string>();
        private Dictionary<string, Team> playerTeamDictionary = new Dictionary<string, Team>();
        private List<Player> players = new List<Player>();
        #endregion

        #region Constructor
        public MainMenu()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Center the window
        }
        #endregion

        #region UI
        private void MainMenu_Resize(object sender, EventArgs e)
        {
            // Center components
            label1.Left = ((this.ClientSize.Width - label1.Width) / 2) + 2;
            label2.Left = ((this.ClientSize.Width - label2.Width) / 2) + 2;
            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button2.Left = (this.ClientSize.Width - button1.Width) / 2;
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            // Center components
            label1.Left = ((this.ClientSize.Width - label1.Width) / 2) + 2;
            label2.Left = ((this.ClientSize.Width - label2.Width) / 2) + 2;
            button1.Left = (this.ClientSize.Width - button1.Width) / 2;
            button2.Left = (this.ClientSize.Width - button1.Width) / 2;
        }
        #endregion

        #region Open Draft Generator
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
        #endregion

        #region Open Roster Editor
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
                        allFileLines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));
                        
                        // Process the file content
                        ProcessEHMFile(allFileLines);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ProcessEHMFile(string[] content)
        {
            // Skip the first line as instructed
            int lineIndex = 1;

            while (lineIndex < content.Length)
            {
                // Check if we have enough lines left to process a player (20 lines per player)
                if (lineIndex + 19 >= content.Length)
                    break;

                // Store the starting line index for this player
                int playerStartLine = lineIndex;

                // Process the player - check the 20th line (playerStartLine + 19) for team info
                string lastLine = content[playerStartLine + 19];
                string[] lastLineValues = lastLine.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Get the player's name from line 14 (playerStartLine + 13)
                string playerName = content[playerStartLine + 13].Trim();

                // Check if we have at least 3 values in the last line and the 3rd value is between 1 and 60
                if (lastLineValues.Length >= 3 &&
                    int.TryParse(lastLineValues[2], out int teamNumber) &&
                    teamNumber >= 1 && teamNumber <= 60)
                {
                    // Add player to dictionary with the start line as key and name as value
                    playerDictionary[playerStartLine] = playerName;

                    // Get the team for this player
                    Team team = Teams.GetTeamByNumber(teamNumber);
                    if (team != null)
                    {
                        playerTeamDictionary[playerName] = team;
                    }

                    // Create and populate a Player object, and set the team number
                    Player player = CreatePlayerFromEHMData(content, playerStartLine);
                    player.TeamNumber = teamNumber;  // Add the teamNumber to the player
                    Console.WriteLine("name : " + player.Name + " team : " + player.TeamNumber);
                    players.Add(player);
                }

                // Move to the next player (20 lines per player)
                lineIndex += 20;
            }

            // Store the data in static properties
            Program.PlayerDictionary = playerDictionary;
            Program.PlayerTeamDictionary = playerTeamDictionary;
            Program.Players = players;

            // Option 2: If RosterMenu has constructor that accepts these parameters
            var rosterMenu = new RosterMenu();
            // Make RosterMenu access the data from static properties
            rosterMenu.Show();
        }

        private Player CreatePlayerFromEHMData(string[] content, int startLine)
        {
            Console.WriteLine("CreatePlayerFromEHMData");

            // Create a new player object using the parameterless constructor
            Player player = new Player();

            // Extract the data from the 20 lines of content
            // Line 1: Various attributes
            string[] line1Values = content[startLine].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (line1Values.Length >= 10)
            {
                player.StartingShooting = int.Parse(line1Values[0]);
                player.StartingPlaymaking = int.Parse(line1Values[1]);
                player.StartingStickhandling = int.Parse(line1Values[2]);
                player.StartingChecking = int.Parse(line1Values[3]);
                player.StartingPositioning = int.Parse(line1Values[4]);
                player.StartingHitting = int.Parse(line1Values[5]);
                player.StartingSkating = int.Parse(line1Values[6]);
                player.StartingEndurance = int.Parse(line1Values[7]);
                player.StartingPenalty = int.Parse(line1Values[8]);
                player.StartingFaceoffs = int.Parse(line1Values[9]);
            }
            
            // Line 2: More attributes
            string[] line2Values = content[startLine + 1].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (line2Values.Length >= 5)
            {
                player.StartingLeadership = int.Parse(line2Values[0]);
                player.StartingAttributeStrength = int.Parse(line2Values[1]);
                player.Potential = int.Parse(line2Values[2]);
                player.Constance = int.Parse(line2Values[3]);
                player.Greed = int.Parse(line2Values[4]);
                player.StartingFighting = int.Parse(line2Values[5]);
            }

            // Line 3: Birth year, etc.
            string[] line3Values = content[startLine + 2].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (line3Values.Length >= 3)
            {
                player.BirthYear = line3Values[0];
                player.BirthMonth = line3Values[2];
                player.BirthDay = line3Values[1];
            }

            // Line 14: Player name
            player.Name = content[startLine + 13].Trim();

            // Line 17: Birth date (formatted differently but we already have the data)

            // Line 20: Team info
            string[] line20Values = content[startLine + 19].Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (line20Values.Length >= 3)
            {
                // We already processed team info for dictionary

                // Set handedness (needs to be converted from numerical value to string)
                if (line20Values.Length >= 2 && int.TryParse(line20Values[1], out int handedness))
                {
                    player.Handedness = handedness == 0 ? "Right" : "Left";
                }
            }

            return player;
        }
        #endregion
    }
}
