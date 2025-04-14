using System;
using System.Collections.Generic;
using System.Linq;

namespace EHMAssistant
{
    class PositionGenerator
    {
        #region Variables
        private readonly SecureRandomGenerator _secureRandom;
        private readonly Dictionary<Position, int> _assignedPositions;
        private int _totalAssigned;
        private readonly int _totalPlayers;

        // For tracking top 8 picks
        private Dictionary<Position, int> _topPicksAssigned;
        private Dictionary<Position, int> _topPicksTargets;
        private int _topPicksRemaining;
        private Position? _rank1Position;

        // For tracking ranks 9-16
        private Dictionary<Position, int> _midPicksAssigned;
        private Dictionary<Position, int> _midPicksTargets;
        private int _midPicksRemaining;
        private bool _rank9IsDefenseman = false;
        private bool _rank10IsDefenseman = false;

        // For tracking ranks 17-26
        private Dictionary<Position, int> _lowerPicksAssigned;
        private Dictionary<Position, int> _lowerPicksTargets;
        private int _lowerPicksRemaining;

        // For tracking ranks 27-33
        private Dictionary<Position, int> _bracket27to33Assigned;
        private Dictionary<Position, int> _bracket27to33Targets;
        private int _bracket27to33Remaining;

        // For tracking ranks 34-43
        private Dictionary<Position, int> _bracket34to43Assigned;
        private Dictionary<Position, int> _bracket34to43Targets;
        private int _bracket34to43Remaining;

        // For tracking ranks 44-60
        private Dictionary<Position, int> _bracket44to60Assigned;
        private Dictionary<Position, int> _bracket44to60Targets;
        private int _bracket44to60Remaining;
        #endregion

        #region enum Positions
        public enum Position
        {
            Winger,
            Center,
            Defenseman,
            Goaltender
        }
        #endregion

        #region Odds of rolling a Position
        private readonly Dictionary<Position, int> _positionTargets = new Dictionary<Position, int>
        {
            { Position.Winger, 38 },
            { Position.Center, 24 },
            { Position.Defenseman, 33 },
            { Position.Goaltender, 5 }
        };
        #endregion

        #region Constructor
        public PositionGenerator(int totalPlayers = 60)
        {
            _secureRandom = new SecureRandomGenerator();
            _totalPlayers = totalPlayers;
            _totalAssigned = 0;

            _assignedPositions = new Dictionary<Position, int>();
            foreach (Position pos in Enum.GetValues(typeof(Position)))
            {
                _assignedPositions[pos] = 0;
            }
        }
        #endregion

        #region Roll a Position
        public Position RollPosition(int playerRank)
        {
            Position lastRoll = Position.Winger; // Default initialization

            // Special handling for ranks 1-8
            if (playerRank >= 1 && playerRank <= 8)
            {
                // Initialize on first call
                if (_topPicksAssigned == null)
                {
                    _topPicksAssigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 } // Included for completeness but won't be used
                    };

                    _topPicksTargets = new Dictionary<Position, int>
                    {
                        { Position.Center, 3 },
                        { Position.Winger, 2 },
                        { Position.Defenseman, 3 },
                        { Position.Goaltender, 0 } // No goalies in top 8
                    };

                    _topPicksRemaining = 8;
                    _rank1Position = null; // Will be set when rank 1 is processed
                }

                // Special constraint for rank 2
                if (playerRank == 2 && _rank1Position.HasValue)
                {
                    var availablePositions = new List<Position>();

                    if (_rank1Position == Position.Defenseman)
                    {
                        // If rank 1 is Defenseman, rank 2 must be Center or Winger
                        if (_topPicksAssigned[Position.Center] < _topPicksTargets[Position.Center])
                            availablePositions.Add(Position.Center);
                        if (_topPicksAssigned[Position.Winger] < _topPicksTargets[Position.Winger])
                            availablePositions.Add(Position.Winger);
                    }
                    else
                    {
                        // If rank 1 is Center or Winger, rank 2 must be Defenseman
                        if (_topPicksAssigned[Position.Defenseman] < _topPicksTargets[Position.Defenseman])
                            availablePositions.Add(Position.Defenseman);
                    }

                    // If no positions satisfy all constraints, prioritize the rank 1-2 constraint
                    if (!availablePositions.Any())
                    {
                        if (_rank1Position == Position.Defenseman)
                        {
                            // Choose between Center and Winger based on which has more remaining slots
                            if (_topPicksTargets[Position.Center] - _topPicksAssigned[Position.Center] >=
                                _topPicksTargets[Position.Winger] - _topPicksAssigned[Position.Winger])
                                availablePositions.Add(Position.Center);
                            else
                                availablePositions.Add(Position.Winger);
                        }
                        else
                        {
                            availablePositions.Add(Position.Defenseman);
                        }
                    }

                    // Select position
                    lastRoll = availablePositions[_secureRandom.GetRandomValue(0, availablePositions.Count)];
                }
                else
                {
                    // Standard handling for other ranks (1, 3-8)
                    // Get available positions that haven't reached their target
                    var availablePositions = _topPicksTargets
                        .Where(kvp => _topPicksAssigned[kvp.Key] < kvp.Value)
                        .Select(kvp => kvp.Key)
                        .ToList();

                    // Create weighted list based on remaining positions
                    List<Position> weightedPositions = new List<Position>();
                    foreach (var pos in availablePositions)
                    {
                        int remaining = _topPicksTargets[pos] - _topPicksAssigned[pos];
                        for (int i = 0; i < remaining; i++)
                        {
                            weightedPositions.Add(pos);
                        }
                    }

                    // Choose from weighted list (ensure there's at least one option)
                    if (weightedPositions.Count > 0)
                    {
                        lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                    }
                    // Fallback (shouldn't happen with correct math)
                    else
                    {
                        // Find any unfilled position
                        foreach (var kvp in _topPicksTargets)
                        {
                            if (_topPicksAssigned[kvp.Key] < kvp.Value)
                            {
                                lastRoll = kvp.Key;
                                break;
                            }
                        }
                    }

                    // Store rank 1 position for use with rank 2
                    if (playerRank == 1)
                    {
                        _rank1Position = lastRoll;
                    }
                }

                // Update tracking for top 8 picks
                _topPicksAssigned[lastRoll]++;
                _topPicksRemaining--;
            }
            // Special handling for ranks 9-16
            else if (playerRank >= 9 && playerRank <= 16)
            {
                // Initialize on first call
                if (_midPicksAssigned == null)
                {
                    _midPicksAssigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 }
                    };

                    _midPicksTargets = new Dictionary<Position, int>
                    {
                        { Position.Center, 2 },
                        { Position.Winger, 3 },
                        { Position.Defenseman, 2 },
                        { Position.Goaltender, 1 }
                    };

                    _midPicksRemaining = 8;
                }

                // Handle the specific case for rank 11
                if (playerRank == 11 && !_rank9IsDefenseman && !_rank10IsDefenseman &&
                    _midPicksAssigned[Position.Defenseman] < _midPicksTargets[Position.Defenseman])
                {
                    // Force rank 11 to be a defenseman
                    lastRoll = Position.Defenseman;
                }
                else
                {
                    // Get available positions that haven't reached their target
                    var availablePositions = _midPicksTargets
                        .Where(kvp => _midPicksAssigned[kvp.Key] < kvp.Value)
                        .Select(kvp => kvp.Key)
                        .ToList();

                    // Create weighted list based on remaining positions
                    List<Position> weightedPositions = new List<Position>();
                    foreach (var pos in availablePositions)
                    {
                        int remaining = _midPicksTargets[pos] - _midPicksAssigned[pos];
                        for (int i = 0; i < remaining; i++)
                        {
                            weightedPositions.Add(pos);
                        }
                    }

                    // Choose from weighted list (ensure there's at least one option)
                    if (weightedPositions.Count > 0)
                    {
                        lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                    }
                    // Fallback (shouldn't happen with correct math)
                    else
                    {
                        // Find any unfilled position
                        foreach (var kvp in _midPicksTargets)
                        {
                            if (_midPicksAssigned[kvp.Key] < kvp.Value)
                            {
                                lastRoll = kvp.Key;
                                break;
                            }
                        }
                    }
                }

                // Store information about ranks 9 and 10 for later use
                if (playerRank == 9)
                {
                    _rank9IsDefenseman = (lastRoll == Position.Defenseman);
                }
                else if (playerRank == 10)
                {
                    _rank10IsDefenseman = (lastRoll == Position.Defenseman);
                }

                // Update tracking for ranks 9-16 picks
                _midPicksAssigned[lastRoll]++;
                _midPicksRemaining--;
            }
            // Special handling for ranks 17-26
            else if (playerRank >= 17 && playerRank <= 26)
            {
                // Initialize on first call
                if (_lowerPicksAssigned == null)
                {
                    _lowerPicksAssigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 }
                    };

                    _lowerPicksTargets = new Dictionary<Position, int>
                    {
                        { Position.Center, 2 },
                        { Position.Winger, 3 },
                        { Position.Defenseman, 3 },
                        { Position.Goaltender, 1 }
                    };

                    _lowerPicksRemaining = 9;
                }

                // Get available positions that haven't reached their target
                var availablePositions = _lowerPicksTargets
                    .Where(kvp => _lowerPicksAssigned[kvp.Key] < kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Create weighted list based on remaining positions
                List<Position> weightedPositions = new List<Position>();
                foreach (var pos in availablePositions)
                {
                    int remaining = _lowerPicksTargets[pos] - _lowerPicksAssigned[pos];
                    for (int i = 0; i < remaining; i++)
                    {
                        weightedPositions.Add(pos);
                    }
                }

                // Choose from weighted list (ensure there's at least one option)
                if (weightedPositions.Count > 0)
                {
                    lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                }
                // Fallback (shouldn't happen with correct math)
                else
                {
                    // Find any unfilled position
                    foreach (var kvp in _lowerPicksTargets)
                    {
                        if (_lowerPicksAssigned[kvp.Key] < kvp.Value)
                        {
                            lastRoll = kvp.Key;
                            break;
                        }
                    }
                }

                // Update tracking for ranks 17-26 picks
                _lowerPicksAssigned[lastRoll]++;
                _lowerPicksRemaining--;
            }
            // NEW: Special handling for ranks 27-33
            else if (playerRank >= 27 && playerRank <= 33)
            {
                // Initialize on first call
                if (_bracket27to33Assigned == null)
                {
                    _bracket27to33Assigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 }
                    };

                    _bracket27to33Targets = new Dictionary<Position, int>
                    {
                        { Position.Center, 2 },
                        { Position.Winger, 3 },
                        { Position.Defenseman, 2 },
                        { Position.Goaltender, 0 } // No goaltenders in this bracket
                    };

                    _bracket27to33Remaining = 7; // 33 - 27 + 1 = 7 picks
                }

                // Get available positions that haven't reached their target
                var availablePositions = _bracket27to33Targets
                    .Where(kvp => _bracket27to33Assigned[kvp.Key] < kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Create weighted list based on remaining positions
                List<Position> weightedPositions = new List<Position>();
                foreach (var pos in availablePositions)
                {
                    int remaining = _bracket27to33Targets[pos] - _bracket27to33Assigned[pos];
                    for (int i = 0; i < remaining; i++)
                    {
                        weightedPositions.Add(pos);
                    }
                }

                // Choose from weighted list (ensure there's at least one option)
                if (weightedPositions.Count > 0)
                {
                    lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                }
                // Fallback (shouldn't happen with correct math)
                else
                {
                    // Find any unfilled position
                    foreach (var kvp in _bracket27to33Targets)
                    {
                        if (_bracket27to33Assigned[kvp.Key] < kvp.Value)
                        {
                            lastRoll = kvp.Key;
                            break;
                        }
                    }
                }

                // Update tracking for ranks 27-33 picks
                _bracket27to33Assigned[lastRoll]++;
                _bracket27to33Remaining--;
            }
            // NEW: Special handling for ranks 34-43
            else if (playerRank >= 34 && playerRank <= 43)
            {
                // Initialize on first call
                if (_bracket34to43Assigned == null)
                {
                    _bracket34to43Assigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 }
                    };

                    _bracket34to43Targets = new Dictionary<Position, int>
                    {
                        { Position.Center, 2 },
                        { Position.Winger, 4 },
                        { Position.Defenseman, 3 },
                        { Position.Goaltender, 1 }
                    };

                    _bracket34to43Remaining = 10; // 43 - 34 + 1 = 10 picks
                }

                // Get available positions that haven't reached their target
                var availablePositions = _bracket34to43Targets
                    .Where(kvp => _bracket34to43Assigned[kvp.Key] < kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Create weighted list based on remaining positions
                List<Position> weightedPositions = new List<Position>();
                foreach (var pos in availablePositions)
                {
                    int remaining = _bracket34to43Targets[pos] - _bracket34to43Assigned[pos];
                    for (int i = 0; i < remaining; i++)
                    {
                        weightedPositions.Add(pos);
                    }
                }

                // Choose from weighted list (ensure there's at least one option)
                if (weightedPositions.Count > 0)
                {
                    lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                }
                // Fallback (shouldn't happen with correct math)
                else
                {
                    // Find any unfilled position
                    foreach (var kvp in _bracket34to43Targets)
                    {
                        if (_bracket34to43Assigned[kvp.Key] < kvp.Value)
                        {
                            lastRoll = kvp.Key;
                            break;
                        }
                    }
                }

                // Update tracking for ranks 34-43 picks
                _bracket34to43Assigned[lastRoll]++;
                _bracket34to43Remaining--;
            }
            // NEW: Special handling for ranks 44-60
            else if (playerRank >= 44 && playerRank <= 60)
            {
                // Initialize on first call
                if (_bracket44to60Assigned == null)
                {
                    _bracket44to60Assigned = new Dictionary<Position, int>
                    {
                        { Position.Center, 0 },
                        { Position.Winger, 0 },
                        { Position.Defenseman, 0 },
                        { Position.Goaltender, 0 }
                    };

                    _bracket44to60Targets = new Dictionary<Position, int>
                    {
                        { Position.Center, 4 },
                        { Position.Winger, 6 },
                        { Position.Defenseman, 6 },
                        { Position.Goaltender, 1 }
                    };

                    _bracket44to60Remaining = 17; // 60 - 44 + 1 = 17 picks
                }

                // Get available positions that haven't reached their target
                var availablePositions = _bracket44to60Targets
                    .Where(kvp => _bracket44to60Assigned[kvp.Key] < kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // Create weighted list based on remaining positions
                List<Position> weightedPositions = new List<Position>();
                foreach (var pos in availablePositions)
                {
                    int remaining = _bracket44to60Targets[pos] - _bracket44to60Assigned[pos];
                    for (int i = 0; i < remaining; i++)
                    {
                        weightedPositions.Add(pos);
                    }
                }

                // Choose from weighted list (ensure there's at least one option)
                if (weightedPositions.Count > 0)
                {
                    lastRoll = weightedPositions[_secureRandom.GetRandomValue(0, weightedPositions.Count)];
                }
                // Fallback (shouldn't happen with correct math)
                else
                {
                    // Find any unfilled position
                    foreach (var kvp in _bracket44to60Targets)
                    {
                        if (_bracket44to60Assigned[kvp.Key] < kvp.Value)
                        {
                            lastRoll = kvp.Key;
                            break;
                        }
                    }
                }

                // Update tracking for ranks 44-60 picks
                _bracket44to60Assigned[lastRoll]++;
                _bracket44to60Remaining--;
            }

            // Update counters for the final roll
            _assignedPositions[lastRoll]++;
            _totalAssigned++;

            return lastRoll;
        }

        // Dispose method to clean up SecureRandomGenerator
        public void Dispose()
        {
            _secureRandom?.Dispose();
        }
        #endregion
    }
}