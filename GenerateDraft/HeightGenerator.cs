using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace EHMAssistant
{
    class HeightGenerator
    {
        #region Variables
        private readonly RNGCryptoServiceProvider _rng;
        private readonly Dictionary<string, int> _assignedHeights;
        private int _totalAssigned;
        private const int NUMBER_OF_ROLLS = 10;
        #endregion

        #region Height Odds Tables
        private readonly Dictionary<PlayerTypeGenerator.PlayerType, Dictionary<string, int>> _heightOdds = new Dictionary<PlayerTypeGenerator.PlayerType, Dictionary<string, int>>
{
    {
        PlayerTypeGenerator.PlayerType.Sniper, new Dictionary<string, int>
        {
            // 34% chance below 6'0"
            { "5'9\"", 6 },
            { "5'10\"", 12 },
            { "5'11\"", 16 },
            // 38% chance of being 6'0" or 6'1"
            { "6'0\"", 21 },
            { "6'1\"", 17 },
            // 28% chance of being 6'2" and above
            { "6'2\"", 10 },
            { "6'3\"", 7 },
            { "6'4\"", 5 },
            { "6'5\"", 2 },
            { "6'6\"", 2 },
            { "6'7\"", 1 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.FabricantDeJeu, new Dictionary<string, int>
        {
            // 34% chance below 6'0"
            { "5'9\"", 6 },
            { "5'10\"", 12 },
            { "5'11\"", 16 },
            // 38% chance of being 6'0" or 6'1"
            { "6'0\"", 21 },
            { "6'1\"", 17 },
            // 28% chance of being 6'2" and above
            { "6'2\"", 10 },
            { "6'3\"", 7 },
            { "6'4\"", 5 },
            { "6'5\"", 2 },
            { "6'6\"", 2 },
            { "6'7\"", 1 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.AttaquantOffensif, new Dictionary<string, int>
        {
            // 34% chance below 6'0"
            { "5'9\"", 6 },
            { "5'10\"", 12 },
            { "5'11\"", 16 },
            // 38% chance of being 6'0" or 6'1"
            { "6'0\"", 21 },
            { "6'1\"", 17 },
            // 28% chance of being 6'2" and above
            { "6'2\"", 10 },
            { "6'3\"", 7 },
            { "6'4\"", 5 },
            { "6'5\"", 2 },
            { "6'6\"", 2 },
            { "6'7\"", 1 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.AttaquantDePuissance, new Dictionary<string, int>
        {
            // Always minimum 6'2" for unrestricted hitting and SR
            { "6'2\"", 29 },
            { "6'3\"", 26 },
            { "6'4\"", 24 },
            { "6'5\"", 13 },
            { "6'6\"", 5 },
            { "6'7\"", 2 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, new Dictionary<string, int>
        {
            // 12% chance below 6'0"
            { "5'9\"", 1 },
            { "5'10\"", 4 },
            { "5'11\"", 7 },
            // 35% chance of being 6'0" or 6'1"
            { "6'0\"", 17 },
            { "6'1\"", 18 },
            // 53% chance of being 6'2" and above
            { "6'2\"", 18 },
            { "6'3\"", 14 },
            { "6'4\"", 9 },
            { "6'5\"", 5 },
            { "6'6\"", 4 },
            { "6'7\"", 2 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.JoueurDeCaractere, new Dictionary<string, int>
        {
            // 15% chance below 6'0"
            { "5'9\"", 2 },
            { "5'10\"", 4 },
            { "5'11\"", 9 },
            // 30% chance of being 6'0" or 6'1"
            { "6'0\"", 15 },
            { "6'1\"", 19 },
            // 55% chance of being 6'2" and above
            { "6'2\"", 18 },
            { "6'3\"", 14 },
            { "6'4\"", 10 },
            { "6'5\"", 6 },
            { "6'6\"", 4 },
            { "6'7\"", 2 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.DefenseurOffensif, new Dictionary<string, int>
        {
            // 20% chance below 6'0"
            { "5'9\"", 4 },
            { "5'10\"", 7 },
            { "5'11\"", 9 },
            // 45% chance of being 6'0" or 6'1"
            { "6'0\"", 20 },
            { "6'1\"", 25 },
            // 35% chance of being 6'2" and above
            { "6'2\"", 10 },
            { "6'3\"", 10 },
            { "6'4\"", 6 },
            { "6'5\"", 3 },
            { "6'6\"", 3 },
            { "6'7\"", 2 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.DefenseurDefensif, new Dictionary<string, int>
        {
            // 45% chance of being 6'0" or 6'1"
            { "6'0\"", 20 },
            { "6'1\"", 22 },
            // 55% chance of being 6'2" and above
            { "6'2\"", 20 },
            { "6'3\"", 16 },
            { "6'4\"", 10 },
            { "6'5\"", 6 },
            { "6'6\"", 4 },
            { "6'7\"", 1 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.DefenseurPhysique, new Dictionary<string, int>
        {
            // Always minimum 6'2" for unrestricted hitting and SR
            { "6'2\"", 31 },
            { "6'3\"", 30 },
            { "6'4\"", 20 },
            { "6'5\"", 13 },
            { "6'6\"", 4 },
            { "6'7\"", 1 },
            { "6'8\"", 1 }
        }
    },
    {
        PlayerTypeGenerator.PlayerType.Gardien, new Dictionary<string, int>
        {
            { "5'9\"", 2 },
            { "5'10\"", 5 },
            { "5'11\"", 8 },
            { "6'0\"", 19 },
            { "6'1\"", 19 },
            { "6'2\"", 15 },
            { "6'3\"", 15 },
            { "6'4\"", 8 },
            { "6'5\"", 4 },
            { "6'6\"", 2 },
            { "6'7\"", 2 },
            { "6'8\"", 1 }
        }
    }
};
        #endregion

        #region Constructor
        public HeightGenerator()
        {
            _rng = new RNGCryptoServiceProvider();
            _totalAssigned = 0;
            _assignedHeights = new Dictionary<string, int>();

            // Initialize heights tracking
            foreach (var typeOdds in _heightOdds.Values)
            {
                foreach (var height in typeOdds.Keys)
                {
                    if (!_assignedHeights.ContainsKey(height))
                    {
                        _assignedHeights[height] = 0;
                    }
                }
            }
        }
        #endregion

        #region Roll Height Methods
        private string SingleRoll(PlayerTypeGenerator.PlayerType playerType)
        {
            if (!_heightOdds.ContainsKey(playerType))
            {
                throw new ArgumentException($"No height odds defined for player type: {playerType}");
            }

            var heightOddsForType = _heightOdds[playerType];
            List<string> weightedHeights = new List<string>();

            // Create weighted list based on odds
            foreach (var kvp in heightOddsForType)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    weightedHeights.Add(kvp.Key);
                }
            }

            return weightedHeights[GetSecureRandomInt(0, weightedHeights.Count)];
        }

        public string RollHeight(PlayerTypeGenerator.PlayerType playerType)
        {
            string lastRoll = "6'0\""; // Default value

            // Perform multiple rolls and keep only the last one
            for (int i = 0; i < NUMBER_OF_ROLLS; i++)
            {
                lastRoll = SingleRoll(playerType);
            }

            // Update tracking
            _assignedHeights[lastRoll]++;
            _totalAssigned++;

            return lastRoll;
        }

        private int GetSecureRandomInt(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue");

            byte[] randomBytes = new byte[4];
            _rng.GetBytes(randomBytes);
            int value = BitConverter.ToInt32(randomBytes, 0);

            int range = maxValue - minValue;
            int max = int.MaxValue - (int.MaxValue % range);
            while (value >= max)
            {
                _rng.GetBytes(randomBytes);
                value = BitConverter.ToInt32(randomBytes, 0);
            }

            return minValue + (Math.Abs(value) % range);
        }
        #endregion

        #region Set height of minimum 6 feets if Generational or Elite
        public void SetMinHeightForElitePlayers(Player player)
        {
            if ((player.Height == "5'9\"" || player.Height == "5'10\"" || player.Height == "5'11\"") && 
                (player.PositionStrength == PositionStrengthGenerator.PositionStrength.Generational || player.PositionStrength == PositionStrengthGenerator.PositionStrength.Elite))
            {
                player.Height = "6'0\"";
            }
        }
        #endregion

        #region Distribution Tracking
        public void PrintDistribution()
        {
            Console.WriteLine("\nCurrent Height Distribution:");
            foreach (var kvp in _assignedHeights.Where(x => x.Value > 0))
            {
                double percentage = (_totalAssigned > 0) ?
                    (kvp.Value * 100.0 / _totalAssigned) : 0;
                Console.WriteLine($"{kvp.Key}: {kvp.Value} players ({percentage:F1}%)");
            }
        }
        #endregion
    }
}