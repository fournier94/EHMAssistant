using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace EHMAssistant
{
    class PlayerReader
    {
        // Dictionary to store players with line numbers as keys and player objects as values
        private Dictionary<int, PlayerData> playerDictionary = new Dictionary<int, PlayerData>();

        // Store the file path for later use
        private string currentFilePath;

        // Store all lines from the file
        private string[] allFileLines;

        public Dictionary<int, PlayerData> GetPlayerDictionary()
        {
            return playerDictionary;
        }

        public int ReadEHMFile(string filePath)
        {
            // Store the file path
            currentFilePath = filePath;

            // Clear existing data
            playerDictionary.Clear();

            try
            {
                // Read all lines from the file
                allFileLines = File.ReadAllLines(filePath);

                // Skip the first line as specified
                if (allFileLines.Length <= 1)
                {
                    throw new Exception("The file is too short to contain any player data.");
                }

                // Process each player (each player takes 20 lines)
                for (int i = 1; i < allFileLines.Length - 19; i += 20)
                {
                    try
                    {
                        // Check if we have enough lines left for a complete player entry
                        if (i + 19 >= allFileLines.Length)
                            break;

                        // Check if this is a valid player entry (has 99 at the end of third line)
                        string thirdLine = allFileLines[i + 2];
                        string[] thirdLineValues = thirdLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (thirdLineValues.Length > 0 && thirdLineValues[thirdLineValues.Length - 1] == "99")
                        {
                            // Create a new player data object
                            PlayerData player = new PlayerData();

                            // Read player name from line 14 (index i+13)
                            player.Name = allFileLines[i + 13].Trim();

                            // Read and parse first line (starting attributes)
                            ParseFirstLine(allFileLines[i], player);

                            // Read and parse second line
                            ParseSecondLine(allFileLines[i + 1], player);

                            // Read and parse third line (birth info)
                            ParseThirdLine(allFileLines[i + 2], player);

                            // Read and parse eleventh line (height)
                            ParseEleventhLine(allFileLines[i + 10], player);

                            // Read and parse seventeenth line (ceilings)
                            ParseSeventeenthLine(allFileLines[i + 16], player);

                            // Read and parse twentieth line (secondary position)
                            ParseTwentiethLine(allFileLines[i + 19], player);

                            // Add player to dictionary
                            playerDictionary.Add(i, player);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log errors but continue processing
                        Console.WriteLine($"Error processing player at line {i}: {ex.Message}");
                    }
                }

                return playerDictionary.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing file: {ex.Message}");
            }
        }

        private void ParseFirstLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length >= 10)
            {
                player.StartingShooting = int.Parse(values[0]);
                player.StartingPlaymaking = int.Parse(values[1]);
                player.StartingStickhandling = int.Parse(values[2]);
                player.StartingChecking = int.Parse(values[3]);
                player.StartingPositioning = int.Parse(values[4]);
                player.StartingHitting = int.Parse(values[5]);
                player.StartingSkating = int.Parse(values[6]);
                player.StartingEndurance = int.Parse(values[7]);
                player.StartingPenalty = int.Parse(values[8]);
                player.StartingFaceoffs = int.Parse(values[9]);
            }
        }

        private void ParseSecondLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length >= 11)
            {
                player.StartingLeadership = int.Parse(values[0]);
                player.StartingAttributeStrength = int.Parse(values[1]);
                player.Potential = int.Parse(values[2]);
                player.Constance = int.Parse(values[3]);
                player.Greed = int.Parse(values[4]);
                player.StartingFighting = int.Parse(values[5]);

                // Position (9th value, index 8)
                int positionValue = int.Parse(values[8]);
                player.Position = GetPositionFromValue(positionValue);

                // Country (10th value, index 9)
                int countryValue = int.Parse(values[9]);
                player.Country = GetCountryFromValue(countryValue);

                // Handedness (11th value, index 10)
                player.Handedness = int.Parse(values[10]) == 0 ? "Right" : "Left";
            }
        }

        private void ParseThirdLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length >= 3)
            {
                player.BirthYear = values[0];
                player.BirthDay = values[1];
                player.BirthMonth = values[2];
            }
        }

        private void ParseEleventhLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length >= 7)
            {
                int heightValue = int.Parse(values[6]);
                player.Height = GetHeightFromValue(heightValue);
            }
        }

        private void ParseSeventeenthLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length > 0)
            {
                // Parse the ceiling string (it's a long concatenated string)
                string ceilingString = values[0];

                if (ceilingString.Length >= 39) // 13 attributes * 3 digits each
                {
                    // Extract each 3-digit ceiling value
                    player.CeilingFighting = int.Parse(ceilingString.Substring(0, 3));
                    player.CeilingShooting = int.Parse(ceilingString.Substring(3, 3));
                    player.CeilingPlaymaking = int.Parse(ceilingString.Substring(6, 3));
                    player.CeilingStickhandling = int.Parse(ceilingString.Substring(9, 3));
                    player.CeilingChecking = int.Parse(ceilingString.Substring(12, 3));
                    player.CeilingPositioning = int.Parse(ceilingString.Substring(15, 3));
                    player.CeilingHitting = int.Parse(ceilingString.Substring(18, 3));
                    player.CeilingSkating = int.Parse(ceilingString.Substring(21, 3));
                    player.CeilingEndurance = int.Parse(ceilingString.Substring(24, 3));
                    player.CeilingPenalty = int.Parse(ceilingString.Substring(27, 3));
                    player.CeilingFaceoffs = int.Parse(ceilingString.Substring(30, 3));
                    player.CeilingLeadership = int.Parse(ceilingString.Substring(33, 3));
                    player.CeilingAttributeStrength = int.Parse(ceilingString.Substring(36, 3));
                }
            }
        }

        private void ParseTwentiethLine(string line, PlayerData player)
        {
            string[] values = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (values.Length >= 2)
            {
                int secondaryPositionValue = int.Parse(values[1]);
                player.SecondaryPosition = GetPositionFromValue(secondaryPositionValue);
            }
        }

        private string GetPositionFromValue(int positionValue)
        {
            switch (positionValue)
            {
                case 1: return "Goalie";
                case 2: return "Defenseman";
                case 3: return "Left Wing";
                case 4: return "Center";
                case 5: return "Right Wing";
                default: return "Unknown";
            }
        }

        private string GetCountryFromValue(int countryValue)
        {
            switch (countryValue)
            {
                case 1: return "Canada";
                case 2: return "United States";
                case 3: return "Russia";
                case 4: return "Czech Republic";
                case 5: return "Sweden";
                case 6: return "Finland";
                case 7: return "Belarus";
                case 8: return "Slovakia";
                case 10: return "Germany";
                case 14: return "Latvia";
                case 17: return "Switzerland";
                case 19: return "Denmark";
                case 20: return "Slovenia";
                default: return "Unknown";
            }
        }

        private string GetHeightFromValue(int heightValue)
        {
            switch (heightValue)
            {
                case 4: return "5'9\"";
                case 5: return "5'10\"";
                case 6: return "5'11\"";
                case 7: return "6'0\"";
                case 8: return "6'1\"";
                case 9: return "6'2\"";
                case 10: return "6'3\"";
                case 11: return "6'4\"";
                case 12: return "6'5\"";
                case 13: return "6'6\"";
                case 14: return "6'7\"";
                case 15: return "6'8\"";
                default: return "Unknown";
            }
        }

        public void DisplayPlayersInDataGridView(DataGridView dgv)
        {
            if (playerDictionary.Count == 0)
            {
                MessageBox.Show("No players found in the file. Please load a valid EHM file first.");
                return;
            }

            // Clear existing data
            dgv.DataSource = null;

            // Convert dictionary to a list for binding
            List<PlayerData> playerList = new List<PlayerData>();
            foreach (var player in playerDictionary.Values)
            {
                playerList.Add(player);
            }

            // Bind list to DataGridView
            dgv.DataSource = playerList;

            // Customize column headers if needed
            if (dgv.Columns.Count > 0)
            {
                dgv.Columns["Name"].HeaderText = "Player Name";
                dgv.Columns["Position"].HeaderText = "Position";
                dgv.Columns["SecondaryPosition"].HeaderText = "Secondary Position";
                dgv.Columns["Country"].HeaderText = "Country";
                dgv.Columns["BirthYear"].HeaderText = "Birth Year";
                dgv.Columns["BirthMonth"].HeaderText = "Birth Month";
                dgv.Columns["BirthDay"].HeaderText = "Birth Day";
                dgv.Columns["Height"].HeaderText = "Height";
                dgv.Columns["Handedness"].HeaderText = "Handedness";
                dgv.Columns["Potential"].HeaderText = "Potential";
                dgv.Columns["Constance"].HeaderText = "Constance";
                dgv.Columns["Greed"].HeaderText = "Greed";

                // Starting attributes
                dgv.Columns["StartingShooting"].HeaderText = "Shooting";
                dgv.Columns["StartingPlaymaking"].HeaderText = "Playmaking";
                dgv.Columns["StartingStickhandling"].HeaderText = "Stickhandling";
                dgv.Columns["StartingChecking"].HeaderText = "Checking";
                dgv.Columns["StartingPositioning"].HeaderText = "Positioning";
                dgv.Columns["StartingHitting"].HeaderText = "Hitting";
                dgv.Columns["StartingSkating"].HeaderText = "Skating";
                dgv.Columns["StartingEndurance"].HeaderText = "Endurance";
                dgv.Columns["StartingPenalty"].HeaderText = "Penalty";
                dgv.Columns["StartingFaceoffs"].HeaderText = "Faceoffs";
                dgv.Columns["StartingLeadership"].HeaderText = "Leadership";
                dgv.Columns["StartingAttributeStrength"].HeaderText = "Strength";
                dgv.Columns["StartingFighting"].HeaderText = "Fighting";

                // You can optionally hide ceiling columns if they make the grid too cluttered
                dgv.Columns["CeilingFighting"].Visible = false;
                dgv.Columns["CeilingShooting"].Visible = false;
                dgv.Columns["CeilingPlaymaking"].Visible = false;
                dgv.Columns["CeilingStickhandling"].Visible = false;
                dgv.Columns["CeilingChecking"].Visible = false;
                dgv.Columns["CeilingPositioning"].Visible = false;
                dgv.Columns["CeilingHitting"].Visible = false;
                dgv.Columns["CeilingSkating"].Visible = false;
                dgv.Columns["CeilingEndurance"].Visible = false;
                dgv.Columns["CeilingPenalty"].Visible = false;
                dgv.Columns["CeilingFaceoffs"].Visible = false;
                dgv.Columns["CeilingLeadership"].Visible = false;
                dgv.Columns["CeilingAttributeStrength"].Visible = false;
            }

            // Auto-resize columns for better display
            dgv.AutoResizeColumns();
        }
    }

    // Class to hold player data
    public class PlayerData
    {
        // Player information
        public string Name { get; set; }
        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }
        public string Country { get; set; }
        public string Height { get; set; }
        public string Position { get; set; }
        public string SecondaryPosition { get; set; }
        public string Handedness { get; set; }

        // Hidden attributes
        public int Potential { get; set; }
        public int Constance { get; set; }
        public int Greed { get; set; }

        // Starting attributes
        public int StartingFighting { get; set; }
        public int StartingShooting { get; set; }
        public int StartingPlaymaking { get; set; }
        public int StartingStickhandling { get; set; }
        public int StartingChecking { get; set; }
        public int StartingPositioning { get; set; }
        public int StartingHitting { get; set; }
        public int StartingSkating { get; set; }
        public int StartingEndurance { get; set; }
        public int StartingPenalty { get; set; }
        public int StartingFaceoffs { get; set; }
        public int StartingLeadership { get; set; }
        public int StartingAttributeStrength { get; set; }

        // Ceiling attributes
        public int CeilingFighting { get; set; }
        public int CeilingShooting { get; set; }
        public int CeilingPlaymaking { get; set; }
        public int CeilingStickhandling { get; set; }
        public int CeilingChecking { get; set; }
        public int CeilingPositioning { get; set; }
        public int CeilingHitting { get; set; }
        public int CeilingSkating { get; set; }
        public int CeilingEndurance { get; set; }
        public int CeilingPenalty { get; set; }
        public int CeilingFaceoffs { get; set; }
        public int CeilingLeadership { get; set; }
        public int CeilingAttributeStrength { get; set; }

        // Calculated properties
        public int Offense
        {
            get { return (int)Math.Round((StartingShooting + StartingPlaymaking + StartingStickhandling) / 3.0); }
        }

        public int Defense
        {
            get { return (int)Math.Round((StartingChecking + StartingPositioning + StartingHitting) / 3.0); }
        }

        public int Overall
        {
            get
            {
                return (int)Math.Round(
                    (StartingShooting + StartingPlaymaking + StartingStickhandling +
                     StartingChecking + StartingPositioning + StartingHitting +
                     StartingSkating + StartingEndurance + StartingPenalty +
                     StartingFaceoffs + StartingAttributeStrength + Constance + Greed + Potential) / 14.0);
            }
        }
    }
}