using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EHMAssistant
{
    class ModifyPlayers
    {
        // Dictionary to store eligible players with line numbers as keys and player names as values
        private Dictionary<int, string> eligiblePlayers = new Dictionary<int, string>();
        // Reference to the generated players list
        private List<Player> generatedPlayers;
        // Store the file path for later use
        private string currentFilePath;
        // Store all lines from the file
        private string[] allFileLines;

        public ModifyPlayers(List<Player> players = null)
        {
            // Store reference to the generated players if provided
            this.generatedPlayers = players;
        }

        public Dictionary<int, string> GetEligiblePlayers()
        {
            return eligiblePlayers;
        }

        public void SetGeneratedPlayers(List<Player> players)
        {
            this.generatedPlayers = players;
        }

        public int ProcessEHMFile(string filePath)
        {
            // Store the file path
            currentFilePath = filePath;

            // Clear existing data
            eligiblePlayers.Clear();

            try
            {
                // Read all lines from the file
                allFileLines = File.ReadAllLines(filePath, Encoding.GetEncoding(1252));

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

                        // Check the condition: player has a value of 99 at the end of the third line
                        string thirdLine = allFileLines[i + 2];
                        string[] thirdLineValues = thirdLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (thirdLineValues.Length > 0 && thirdLineValues[thirdLineValues.Length - 1] == "99")
                        {
                            // Get the player's name from line 14 (which is at index i+13)
                            string playerName = allFileLines[i + 13].Trim();

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

                return eligiblePlayers.Count;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing file: {ex.Message}");
            }
        }

        public void UpdatePlayerStats()
        {
            if (eligiblePlayers.Count == 0 || generatedPlayers == null || generatedPlayers.Count == 0 || string.IsNullOrEmpty(currentFilePath))
            {
                throw new Exception("Please ensure that both the EHM file and generated players are available.");
            }

            try
            {
                // Update player stats in the file
                UpdatePlayersInFile();

                // Write the updated file
                File.WriteAllLines(currentFilePath, allFileLines, Encoding.GetEncoding(1252));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating player stats: {ex.Message}");
            }
        }

        private void UpdatePlayersInFile()
        {
            // Only update as many players as we have in both collections
            int playersToUpdate = Math.Min(eligiblePlayers.Count, generatedPlayers.Count);

            // Get the eligible players sorted by line number
            var eligiblePlayersSorted = eligiblePlayers.OrderBy(p => p.Key).ToList();

            for (int i = 0; i < playersToUpdate; i++)
            {
                var lineNumber = eligiblePlayersSorted[i].Key;
                var generatedPlayer = generatedPlayers[i];

                // Update first line (starting attributes)
                UpdateFirstLine(lineNumber, generatedPlayer);
                UpdateSecondLine(lineNumber + 1, generatedPlayer);
                UpdateThirdLine(lineNumber + 2, generatedPlayer);
                UpdateEleventhLine(lineNumber + 10, generatedPlayer);
                UpdatePlayerName(lineNumber + 13, generatedPlayer);
                UpdateSeventeenthLine(lineNumber + 16, generatedPlayer);
                UpdateTwentiethLine(lineNumber + 19, generatedPlayer);
            }
        }

        private void UpdateFirstLine(int lineNumber, Player player)
        {
            string updatedLine = string.Format("{0,3} {1,3} {2,3} {3,3} {4,3} {5,3} {6,3} {7,3} {8,3} {9,3}",
                player.StartingShooting,
                player.StartingPlaymaking,
                player.StartingStickhandling,
                player.StartingChecking,
                player.StartingPositioning,
                player.StartingHitting,
                player.StartingSkating,
                player.StartingEndurance,
                player.StartingPenalty,
                player.StartingFaceoffs);

            // Add a single trailing space after the 10th number
            updatedLine += " ";

            allFileLines[lineNumber] = updatedLine;
        }

        private void UpdateSecondLine(int lineNumber, Player player)
        {
            // Get the original line to preserve certain values
            string originalLine = allFileLines[lineNumber];
            string[] originalValues = originalLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Extract values that need to be preserved
            string seventhValue = originalValues.Length >= 7 ? originalValues[6] : "0";
            string eighthValue = originalValues.Length >= 8 ? originalValues[7] : "0";
            string tenthValue = originalValues.Length >= 10 ? originalValues[9] : "0";
            int position = GetPositionValue(player);

            int handedness;

            if (player.Handedness == "Right")
                handedness = 0;
            else
                handedness = 1;

            string country = GetCountryValue(player).ToString();

            // Create the updated line with exact spacing: one space at beginning, two spaces between values
            string updatedLine = string.Format(" {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}  {8}  {9}  {10} ",
                player.StartingLeadership,
                player.StartingAttributeStrength,
                player.Potential,
                player.Constance,
                player.Greed,
                player.StartingFighting,
                seventhValue,
                eighthValue,
                position,
                country,
                handedness);
            Console.WriteLine("updatedLine : " + updatedLine);
            allFileLines[lineNumber] = updatedLine;
        }

        private int GetPositionValue(Player player)
        {
            int positionValue;

            if (player.PlayerPosition == PositionGenerator.Position.Center)
                positionValue = 4;
            else if (player.PlayerPosition == PositionGenerator.Position.Winger)
            {
                if (player.Handedness == "Right")
                    positionValue = 5;
                else
                    positionValue = 3;
            }
            else if (player.PlayerPosition == PositionGenerator.Position.Defenseman)
                positionValue = 2;
            else
                positionValue = 1;

            return positionValue;
        }

        private int GetSecondaryPositionValue(Player player)
        {
            int positionValue;

            if (player.PlayerPosition == PositionGenerator.Position.Center)
            {
                if (player.Handedness == "Right")
                    positionValue = 5;
                else
                    positionValue = 3;
            }
            else if (player.PlayerPosition == PositionGenerator.Position.Winger)
            {
                if (player.Handedness == "Right")
                    positionValue = 3;
                else
                    positionValue = 5;
            }
            else
                positionValue = 0;

            return positionValue;
        }

        private int GetCountryValue(Player player)
        {
            int countryValue;

            if (player.PlayerCountry == CountryGenerator.Country.Canada)
                countryValue = 1;
            else if (player.PlayerCountry == CountryGenerator.Country.UnitedStates)
                countryValue = 2;
            else if (player.PlayerCountry == CountryGenerator.Country.Sweden)
                countryValue = 5;
            else if (player.PlayerCountry == CountryGenerator.Country.Russia)
                countryValue = 3;
            else if (player.PlayerCountry == CountryGenerator.Country.Finland)
                countryValue = 6;
            else if (player.PlayerCountry == CountryGenerator.Country.CzechRepublic)
                countryValue = 4;
            else if (player.PlayerCountry == CountryGenerator.Country.Slovakia)
                countryValue = 8;
            else if (player.PlayerCountry == CountryGenerator.Country.Switzerland)
                countryValue = 17;
            else if (player.PlayerCountry == CountryGenerator.Country.Germany)
                countryValue = 10;
            else if (player.PlayerCountry == CountryGenerator.Country.Denmark)
                countryValue = 19;
            else if (player.PlayerCountry == CountryGenerator.Country.Latvia)
                countryValue = 14;
            else if (player.PlayerCountry == CountryGenerator.Country.Belarus)
                countryValue = 7;
            else if (player.PlayerCountry == CountryGenerator.Country.Slovenia)
                countryValue = 20;
            else
                countryValue = 1;

            return countryValue;
        }

        private void UpdateThirdLine(int lineNumber, Player player)
        {
            // Get the original line to preserve other values
            string originalLine = allFileLines[lineNumber];
            string[] originalValues = originalLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (originalValues.Length < 9)
            {
                throw new Exception("Third line does not have enough values.");
            }

            // Update the first three values with player's birth information
            originalValues[0] = player.BirthYear.ToString();
            originalValues[1] = player.BirthDay.ToString();
            originalValues[2] = player.BirthMonth.ToString();

            StringBuilder updatedLine = new StringBuilder(" ");

            for (int i = 0; i < originalValues.Length; i++)
            {
                updatedLine.Append(originalValues[i]);

                // Add two spaces between values
                if (i < originalValues.Length - 1)
                {
                    updatedLine.Append("  ");
                }
                // Add one space at the end
                else
                {
                    updatedLine.Append(" ");
                }
            }

            // Update the line in the file
            allFileLines[lineNumber] = updatedLine.ToString();
        }

        private void UpdateEleventhLine(int lineNumber, Player player)
        {
            // Get the original line to preserve all other values
            string originalLine = allFileLines[lineNumber];
            string[] originalValues = originalLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (originalValues.Length < 8)
            {
                throw new Exception("Line does not have at least 8 values.");
            }

            int height = GetHeightValue(player);

            // Update the seventh value (index 6) with player's height value
            originalValues[6] = height.ToString();

            // Rebuild the formatted line with exact spacing
            string updatedLine = $" {originalValues[0]}  {originalValues[1]}  {originalValues[2]}  {originalValues[3]}  {originalValues[4]}  {originalValues[5]}  {originalValues[6]}  {originalValues[7]} ";

            // Update the line in the file
            allFileLines[lineNumber] = updatedLine;
        }


        private int GetHeightValue(Player player)
        {
            int height;

            if (player.Height == "5'9\"")
                height = 4;
            else if (player.Height == "5'10\"")
                height = 5;
            else if (player.Height == "5'11\"")
                height = 6;
            else if (player.Height == "6'0\"")
                height = 7;
            else if (player.Height == "6'1\"")
                height = 8;
            else if (player.Height == "6'2\"")
                height = 9;
            else if (player.Height == "6'3\"")
                height = 10;
            else if (player.Height == "6'4\"")
                height = 11;
            else if (player.Height == "6'5\"")
                height = 12;
            else if (player.Height == "6'6\"")
                height = 13;
            else if (player.Height == "6'7\"")
                height = 14;
            else if (player.Height == "6'8\"")
                height = 15;
            else
                height = 7;

            return height;
        }

        private void UpdatePlayerName(int lineNumber, Player player)
        {
            // Update the player name line
            allFileLines[lineNumber] = player.Name;
        }

        private void UpdateSeventeenthLine(int lineNumber, Player player)
        {
            // Calculate the ceilings value
            string ceilings = string.Concat(
                player.CeilingFighting.ToString("D3"),
                player.CeilingShooting.ToString("D3"),
                player.CeilingPlaymaking.ToString("D3"),
                player.CeilingStickhandling.ToString("D3"),
                player.CeilingChecking.ToString("D3"),
                player.CeilingPositioning.ToString("D3"),
                player.CeilingHitting.ToString("D3"),
                player.CeilingSkating.ToString("D3"),
                player.CeilingEndurance.ToString("D3"),
                player.CeilingPenalty.ToString("D3"),
                player.CeilingFaceoffs.ToString("D3"),
                player.CeilingLeadership.ToString("D3"),
                player.CeilingAttributeStrength.ToString("D3")
            );

            // Get the original line to preserve all other values
            string originalLine = allFileLines[lineNumber];
            string[] originalValues = originalLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Create a new array to store the updated values
            string[] updatedValues = new string[originalValues.Length];

            // Copy all original values first
            for (int i = 0; i < originalValues.Length; i++)
            {
                updatedValues[i] = originalValues[i];
            }

            // Update the first value with the ceilings calculation
            if (updatedValues.Length > 0)
            {
                updatedValues[0] = ceilings;
            }

            // Join the array back into a space-separated string with proper formatting
            string updatedLine = string.Join(" ", updatedValues.Select((val, index) => string.Format("{0,3}", val)));

            // Update the line in the file
            allFileLines[lineNumber] = updatedLine;
        }

        private void UpdateTwentiethLine(int lineNumber, Player player)
        {
            // Get the original line to preserve most values
            string originalLine = allFileLines[lineNumber];
            string[] originalValues = originalLine.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (originalValues.Length < 5)
            {
                throw new Exception("Line does not have at least 5 values.");
            }

            int secondaryPosition = GetSecondaryPositionValue(player);

            // Update only the second value (index 1) with player's secondary position
            originalValues[1] = secondaryPosition.ToString();

            // Rebuild the formatted line with exact spacing
            string updatedLine = $" {originalValues[0]}  {originalValues[1]}  {originalValues[2]}  {originalValues[3]}  {originalValues[4]} ";

            // Update the line in the file
            allFileLines[lineNumber] = updatedLine;
        }
    }
}