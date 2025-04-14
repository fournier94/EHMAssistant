using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace EHMAssistant
{
    class PlayerTypeGenerator
    {
        #region Variables
        private readonly SecureRandomGenerator _randomGenerator;
        private readonly Dictionary<PlayerType, int> _assignedTypes;
        private int _totalAssigned;
        private readonly int _totalPlayers;
        private const int NUMBER_OF_ROLLS = 10;  // Number of times to roll for each player

        // Dictionaries to track the assigned types per rank bracket
        private Dictionary<PlayerType, int> _rank1to8Assigned;
        private Dictionary<PlayerType, int> _rank9to16Assigned;
        private Dictionary<PlayerType, int> _rank17to26Assigned;
        private Dictionary<PlayerType, int> _rank27to33Assigned;

        // Target percentages for ranks 34-60
        private readonly Dictionary<(PositionGenerator.Position, Round), Dictionary<PlayerType, int>> _typeTargets =
            new Dictionary<(PositionGenerator.Position, Round), Dictionary<PlayerType, int>>();
        #endregion

        #region PlayerType and Round Enums
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

        private enum Round
        {
            FirstRound,  // Rank 1-30
            SecondRound  // Rank 31-60
        }
        #endregion

        #region Player Types by category
        private readonly List<PlayerType> _forwardTypes = new List<PlayerType>
        {
            PlayerType.Sniper,
            PlayerType.FabricantDeJeu,
            PlayerType.AttaquantOffensif,
            PlayerType.AttaquantDePuissance,
            PlayerType.AttaquantPolyvalent
        };

        private readonly List<PlayerType> _defensemenTypes = new List<PlayerType>
        {
            PlayerType.DefenseurOffensif,
            PlayerType.DefenseurDefensif,
            PlayerType.DefenseurPhysique
        };

        private readonly List<PlayerType> _rank34to60ForwardTypes = new List<PlayerType>
        {
            PlayerType.Sniper,
            PlayerType.FabricantDeJeu,
            PlayerType.AttaquantOffensif,
            PlayerType.AttaquantDePuissance,
            PlayerType.AttaquantPolyvalent,
            PlayerType.JoueurDeCaractere
        };

        private readonly List<PlayerType> _rank27to33ForwardTypes = new List<PlayerType>
        {
            PlayerType.Sniper,
            PlayerType.FabricantDeJeu,
            PlayerType.AttaquantOffensif,
            PlayerType.AttaquantDePuissance,
            PlayerType.AttaquantPolyvalent
        };

        private readonly List<PlayerType> _rank27to33DefenseTypes = new List<PlayerType>
        {
            PlayerType.DefenseurDefensif,
            PlayerType.DefenseurPhysique
        };
        #endregion

        #region Initialize Player Type Odds
        private void InitializeTypeTargets()
        {
            // Low Rank (34-60) odds - These will be used for random distributions
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
                { PlayerType.Sniper, 4 },
                { PlayerType.FabricantDeJeu, 4 },
                { PlayerType.AttaquantOffensif, 4 },
                { PlayerType.AttaquantDePuissance, 48 },
                { PlayerType.AttaquantPolyvalent, 18 },
                { PlayerType.JoueurDeCaractere, 22 }
            });

            _typeTargets.Add((PositionGenerator.Position.Defenseman, Round.SecondRound), new Dictionary<PlayerType, int>
            {
                { PlayerType.DefenseurOffensif, 10 },
                { PlayerType.DefenseurDefensif, 40 },
                { PlayerType.DefenseurPhysique, 50 }
            });

            // For completeness, adding goalie option
            var goalieOdds = new Dictionary<PlayerType, int> { { PlayerType.Gardien, 100 } };
            _typeTargets.Add((PositionGenerator.Position.Goaltender, Round.FirstRound), goalieOdds);
            _typeTargets.Add((PositionGenerator.Position.Goaltender, Round.SecondRound), goalieOdds);
        }
        #endregion

        #region Constructor
        public PlayerTypeGenerator(int totalPlayers = 60)
        {
            _randomGenerator = new SecureRandomGenerator();
            _totalPlayers = totalPlayers;
            _totalAssigned = 0;
            _assignedTypes = new Dictionary<PlayerType, int>();
            foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
            {
                _assignedTypes[type] = 0;
            }
            InitializeTypeTargets();
        }
        #endregion

        #region Roll Player Type
        public PlayerType RollPlayerType(PositionGenerator.Position position, int rank)
        {
            // Always return Gardien for goaltender
            if (position == PositionGenerator.Position.Goaltender)
            {
                _assignedTypes[PlayerType.Gardien]++;
                _totalAssigned++;
                return PlayerType.Gardien;
            }

            // Check which rank bracket we're in and handle accordingly
            if (rank >= 1 && rank <= 8)
            {
                return RollRank1to8Type(position);
            }
            else if (rank >= 9 && rank <= 16)
            {
                return RollRank9to16Type(position);
            }
            else if (rank >= 17 && rank <= 26)
            {
                return RollRank17to26Type(position);
            }
            else if (rank >= 27 && rank <= 33)
            {
                return RollRank27to33Type(position);
            }
            else if (rank >= 34 && rank <= 60)
            {
                // Using distribution table for these ranks
                Round round = Round.SecondRound;
                return RollLowerRankType(position, round);
            }

            // This should never happen given our input constraints
            throw new ArgumentException($"Invalid rank: {rank}");
        }
        #endregion
        
        #region Roll rank 1 to 33
        private PlayerType RollRank1to8Type(PositionGenerator.Position position)
        {
            // Initialize assigned types dictionary if it doesn't exist
            if (_rank1to8Assigned == null)
            {
                _rank1to8Assigned = new Dictionary<PlayerType, int>();
                foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
                {
                    _rank1to8Assigned[type] = 0;
                }
            }

            List<PlayerType> availableTypes = new List<PlayerType>();

            // Determine available types based on position
            if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
            {
                // For forwards, add all forward types that haven't been assigned yet
                foreach (var type in _forwardTypes)
                {
                    if (_rank1to8Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }
            else if (position == PositionGenerator.Position.Defenseman)
            {
                // For defensemen, add all defenseman types that haven't been assigned yet
                foreach (var type in _defensemenTypes)
                {
                    if (_rank1to8Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }

            // If no types are available (shouldn't happen but just in case),
            // we'll use all possible types for the position
            if (availableTypes.Count == 0)
            {
                if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
                {
                    availableTypes = _forwardTypes.ToList();
                }
                else if (position == PositionGenerator.Position.Defenseman)
                {
                    availableTypes = _defensemenTypes.ToList();
                }
            }

            // Roll for a type using SecureRandomGenerator
            PlayerType selectedType = availableTypes[_randomGenerator.GetRandomValue(0, availableTypes.Count)];

            // Mark it as assigned
            _rank1to8Assigned[selectedType]++;
            _assignedTypes[selectedType]++;
            _totalAssigned++;

            return selectedType;
        }

        private PlayerType RollRank9to16Type(PositionGenerator.Position position)
        {
            // Initialize assigned types dictionary if it doesn't exist
            if (_rank9to16Assigned == null)
            {
                _rank9to16Assigned = new Dictionary<PlayerType, int>();
                foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
                {
                    _rank9to16Assigned[type] = 0;
                }
            }

            List<PlayerType> availableTypes = new List<PlayerType>();

            // Determine available types based on position
            if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
            {
                foreach (var type in _forwardTypes)
                {
                    if (_rank9to16Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }
            else if (position == PositionGenerator.Position.Defenseman)
            {
                foreach (var type in _defensemenTypes)
                {
                    if (_rank9to16Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }

            // If no types are available, use all possible types for the position
            if (availableTypes.Count == 0)
            {
                if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
                {
                    availableTypes = _forwardTypes.ToList();
                }
                else if (position == PositionGenerator.Position.Defenseman)
                {
                    availableTypes = _defensemenTypes.ToList();
                }
            }

            // Roll for a type using SecureRandomGenerator
            PlayerType selectedType = availableTypes[_randomGenerator.GetRandomValue(0, availableTypes.Count)];

            // Mark it as assigned
            _rank9to16Assigned[selectedType]++;
            _assignedTypes[selectedType]++;
            _totalAssigned++;

            return selectedType;
        }

        private PlayerType RollRank17to26Type(PositionGenerator.Position position)
        {
            // Initialize assigned types dictionary if it doesn't exist
            if (_rank17to26Assigned == null)
            {
                _rank17to26Assigned = new Dictionary<PlayerType, int>();
                foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
                {
                    _rank17to26Assigned[type] = 0;
                }
            }

            List<PlayerType> availableTypes = new List<PlayerType>();

            // Determine available types based on position
            if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
            {
                foreach (var type in _forwardTypes)
                {
                    if (_rank17to26Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }
            else if (position == PositionGenerator.Position.Defenseman)
            {
                foreach (var type in _defensemenTypes)
                {
                    if (_rank17to26Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }

            // If no types are available, use all possible types for the position
            if (availableTypes.Count == 0)
            {
                if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
                {
                    availableTypes = _forwardTypes.ToList();
                }
                else if (position == PositionGenerator.Position.Defenseman)
                {
                    availableTypes = _defensemenTypes.ToList();
                }
            }

            // Roll for a type using SecureRandomGenerator
            PlayerType selectedType = availableTypes[_randomGenerator.GetRandomValue(0, availableTypes.Count)];

            // Mark it as assigned
            _rank17to26Assigned[selectedType]++;
            _assignedTypes[selectedType]++;
            _totalAssigned++;

            return selectedType;
        }

        private PlayerType RollRank27to33Type(PositionGenerator.Position position)
        {
            // Initialize assigned types dictionary if it doesn't exist
            if (_rank27to33Assigned == null)
            {
                _rank27to33Assigned = new Dictionary<PlayerType, int>();
                foreach (PlayerType type in Enum.GetValues(typeof(PlayerType)))
                {
                    _rank27to33Assigned[type] = 0;
                }
            }

            List<PlayerType> availableTypes = new List<PlayerType>();

            // For ranks 27-33, there's a different set of available player types
            if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
            {
                foreach (var type in _rank27to33ForwardTypes)
                {
                    if (_rank27to33Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }
            else if (position == PositionGenerator.Position.Defenseman)
            {
                // In rank 27-33, only DefenseurDefensif and DefenseurPhysique are available for defensemen
                foreach (var type in _rank27to33DefenseTypes)
                {
                    if (_rank27to33Assigned[type] == 0)
                    {
                        availableTypes.Add(type);
                    }
                }
            }

            // If no types are available, use all possible types for the position
            if (availableTypes.Count == 0)
            {
                if (position == PositionGenerator.Position.Winger || position == PositionGenerator.Position.Center)
                {
                    availableTypes = _rank27to33ForwardTypes.ToList();
                }
                else if (position == PositionGenerator.Position.Defenseman)
                {
                    availableTypes = _rank27to33DefenseTypes.ToList();
                }
            }

            // Roll for a type using SecureRandomGenerator
            PlayerType selectedType = availableTypes[_randomGenerator.GetRandomValue(0, availableTypes.Count)];

            // Mark it as assigned
            _rank27to33Assigned[selectedType]++;
            _assignedTypes[selectedType]++;
            _totalAssigned++;

            return selectedType;
        }
        #endregion

        #region Roll ranks 34 to 60
        private PlayerType RollLowerRankType(PositionGenerator.Position position, Round round)
        {
            // For ranks 34-60, we use the predefined odds tables
            var key = (position, round);

            if (!_typeTargets.ContainsKey(key))
            {
                throw new ArgumentException($"Invalid position or round combination: Position={position}, Round={round}");
            }

            var possibleTypes = _typeTargets[key];
            List<PlayerType> weightedTypes = new List<PlayerType>();

            foreach (var kvp in possibleTypes)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    weightedTypes.Add(kvp.Key);
                }
            }

            // Use SecureRandomGenerator for random selection
            PlayerType selectedType = weightedTypes[_randomGenerator.GetRandomValue(0, weightedTypes.Count)];

            // For ranks 34-60, we don't need to track uniqueness constraints
            // Just update global counters
            _assignedTypes[selectedType]++;
            _totalAssigned++;

            return selectedType;
        }
        #endregion

        #region Print Distribution
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

        #region IDisposable Implementation
        public void Dispose()
        {
            _randomGenerator?.Dispose();
        }
        #endregion
    }
}