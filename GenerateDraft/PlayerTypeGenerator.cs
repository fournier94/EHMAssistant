using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace EHMAssistant
{
    class PlayerTypeGenerator
    {
        #region Variables
        private readonly RNGCryptoServiceProvider _rng;
        private readonly Dictionary<PlayerType, int> _assignedTypes;
        private int _totalAssigned;
        private readonly int _totalPlayers;
        private const int NUMBER_OF_ROLLS = 10;  // Number of times to roll for each player
        #endregion

        #region enums player types
        public enum PlayerType
        {
            // Forward types
            Sniper,
            FabricantDeJeu,
            AttaquantOffensif,
            AttaquantDePuissance,
            AttaquantPolyvalent,
            JoueurDeCaractere,

            // Defense types
            DefenseurOffensif,
            DefenseurDefensif,
            DefenseurPhysique,

            // Goalie type
            Gardien
        }
        #endregion

        #region Type odds
        private readonly Dictionary<(PositionGenerator.Position, Round), Dictionary<PlayerType, int>> _typeTargets =
            new Dictionary<(PositionGenerator.Position, Round), Dictionary<PlayerType, int>>();

        private enum Round
        {
            FirstRound,  // Rank 1-30
            SecondRound    // Rank 31-60
        }

        private void InitializeTypeTargets()
        {
            // High Rank (1-30) odds
            _typeTargets.Add((PositionGenerator.Position.Winger, Round.FirstRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.Sniper, 22 },
                { PlayerType.FabricantDeJeu, 24 },
                { PlayerType.AttaquantOffensif, 24 },
                { PlayerType.AttaquantDePuissance, 15 },
                { PlayerType.AttaquantPolyvalent, 15 }
            });

            _typeTargets.Add((PositionGenerator.Position.Center, Round.FirstRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.Sniper, 22 },
                { PlayerType.FabricantDeJeu, 24 },
                { PlayerType.AttaquantOffensif, 24 },
                { PlayerType.AttaquantDePuissance, 15 },
                { PlayerType.AttaquantPolyvalent, 15 }
            });

            _typeTargets.Add((PositionGenerator.Position.Defenseman, Round.FirstRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.DefenseurOffensif, 20 },
                { PlayerType.DefenseurDefensif, 50 },
                { PlayerType.DefenseurPhysique, 30 }
            });

            // Low Rank (31-60) odds - You can adjust these percentages as needed
            _typeTargets.Add((PositionGenerator.Position.Winger, Round.SecondRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.Sniper, 5 },
                { PlayerType.FabricantDeJeu, 5 },
                { PlayerType.AttaquantOffensif, 5 },
                { PlayerType.AttaquantDePuissance, 45 },
                { PlayerType.AttaquantPolyvalent, 18 },
                { PlayerType.JoueurDeCaractere, 22 }
            });

            _typeTargets.Add((PositionGenerator.Position.Center, Round.SecondRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.Sniper, 5 },
                { PlayerType.FabricantDeJeu, 5 },
                { PlayerType.AttaquantOffensif, 5 },
                { PlayerType.AttaquantDePuissance, 45 },
                { PlayerType.AttaquantPolyvalent, 18 },
                { PlayerType.JoueurDeCaractere, 22 }
            });

            _typeTargets.Add((PositionGenerator.Position.Defenseman, Round.SecondRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.DefenseurOffensif, 10 },
                { PlayerType.DefenseurDefensif, 40 },
                { PlayerType.DefenseurPhysique, 50 }
            });

            // Goaltender odds remain the same for both tiers
            var goalieOdds = new Dictionary<PlayerType, int> { { PlayerType.Gardien, 100 } };
            _typeTargets.Add((PositionGenerator.Position.Goaltender, Round.FirstRound), goalieOdds);
            _typeTargets.Add((PositionGenerator.Position.Goaltender, Round.SecondRound), goalieOdds);
        }
        #endregion

        #region Constructor
        public PlayerTypeGenerator(int totalPlayers = 60)
        {
            _rng = new RNGCryptoServiceProvider();
            _totalPlayers = totalPlayers;
            _totalAssigned = 0;

            // Initialize assigned types counter
            _assignedTypes = new Dictionary<PlayerType, int>();
            foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
            {
                _assignedTypes[type] = 0;
            }

            InitializeTypeTargets();
        }
        #endregion

        #region Roll a Player Type
        private PlayerType SingleRoll(PositionGenerator.Position position, Round round)
        {
            var key = (position, round);
            var possibleTypes = _typeTargets[key];

            // Create weighted list of available types based on target percentages
            List<PlayerType> weightedTypes = new List<PlayerType>();
            foreach (var kvp in possibleTypes)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    weightedTypes.Add(kvp.Key);
                }
            }

            return weightedTypes[GetSecureRandomInt(0, weightedTypes.Count)];
        }

        public PlayerType RollPlayerType(PositionGenerator.Position position, int rank)
        {
            Round round = (rank <= 30) ? Round.FirstRound : Round.SecondRound;

            if (!_typeTargets.ContainsKey((position, round)))
            {
                throw new ArgumentException($"Invalid position or rank combination: Position={position}, Rank={rank}");
            }

            PlayerType lastRoll = PlayerType.Sniper; // Default value (can be adjusted)

            for (int i = 0; i < NUMBER_OF_ROLLS; i++)
            {
                lastRoll = SingleRoll(position, round); // Perform a roll and store the result
            }

            // Update counters
            _assignedTypes[lastRoll]++;
            _totalAssigned++;

            return lastRoll; // Return the last roll (10th roll)
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

        #region Distribution Tracking
        public void PrintDistribution()
        {
            Console.WriteLine("\nCurrent Player Type Distribution:");
            foreach (var kvp in _assignedTypes.Where(x => x.Value > 0))
            {
                double percentage = (_totalAssigned > 0) ?
                    (kvp.Value * 100.0 / _totalAssigned) : 0;
                Console.WriteLine($"{kvp.Key}: {kvp.Value} players ({percentage:F1}%)");
            }
        }
        #endregion
    }
}