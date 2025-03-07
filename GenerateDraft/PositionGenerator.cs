using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace EHMAssistant
{
    // Cant have goalies before rank 10
    // Cant have the same position 3 times in a row to prevent repetition

    class PositionGenerator
    {
        #region Variables
        private readonly RNGCryptoServiceProvider _rng;
        private readonly Dictionary<Position, int> _assignedPositions;
        private int _totalAssigned;
        private readonly int _totalPlayers;
        private const int NUMBER_OF_ROLLS = 10;
        private readonly Queue<Position> _previousPositions;  // Track last 2 positions
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
            _rng = new RNGCryptoServiceProvider();
            _totalPlayers = totalPlayers;
            _totalAssigned = 0;
            _previousPositions = new Queue<Position>();  // Initialize queue

            _assignedPositions = new Dictionary<Position, int>();
            foreach (Position pos in Enum.GetValues(typeof(Position)))
            {
                _assignedPositions[pos] = 0;
            }
        }
        #endregion

        #region Roll a Position
        private Position SingleRoll(int playerRank)
        {
            // Calculate target numbers for each position
            Dictionary<Position, int> targetNumbers = new Dictionary<Position, int>();
            foreach (var kvp in _positionTargets)
            {
                targetNumbers[kvp.Key] = (int)Math.Round((_totalPlayers * kvp.Value) / 100.0);
            }

            // Create list of available positions based on current distribution
            List<Position> availablePositions = new List<Position>();
            foreach (var kvp in targetNumbers)
            {
                // Skip Goaltender position for top 10 ranked players
                if (playerRank <= 10 && kvp.Key == Position.Goaltender)
                    continue;

                // Check if this position would create three in a row
                bool wouldCreateThreeInARow = _previousPositions.Count == 2 &&
                    _previousPositions.All(p => p == kvp.Key);

                // Only add position if it wouldn't create three in a row and hasn't reached target
                if (!wouldCreateThreeInARow && _assignedPositions[kvp.Key] < kvp.Value)
                {
                    int deficit = kvp.Value - _assignedPositions[kvp.Key];
                    for (int i = 0; i < deficit; i++)
                    {
                        availablePositions.Add(kvp.Key);
                    }
                }
            }

            // If no positions are available based on targets, fall back to any position that won't create three in a row
            if (!availablePositions.Any())
            {
                availablePositions = _positionTargets.Keys
                    .Where(p => (playerRank > 10 || p != Position.Goaltender) &&
                               !(_previousPositions.Count == 2 && _previousPositions.All(prev => prev == p)))
                    .ToList();
            }

            // Redistribute Goaltender probability for top 10 players
            if (playerRank <= 10)
            {
                double totalWeight = _positionTargets.Where(x => x.Key != Position.Goaltender)
                                                   .Sum(x => x.Value);

                foreach (var pos in availablePositions.ToList())
                {
                    int additionalSpots = (int)Math.Round((_positionTargets[Position.Goaltender] * _positionTargets[pos]) / totalWeight);
                    for (int i = 0; i < additionalSpots; i++)
                    {
                        availablePositions.Add(pos);
                    }
                }
            }

            // If somehow we end up with no available positions (shouldn't happen), reset previous positions
            if (!availablePositions.Any())
            {
                _previousPositions.Clear();
                return SingleRoll(playerRank);
            }

            return availablePositions[GetSecureRandomInt(0, availablePositions.Count)];
        }

        public Position RollPosition(int playerRank)
        {
            Position lastRoll = Position.Winger; // Default value

            // Perform multiple rolls and keep only the last one
            for (int i = 0; i < NUMBER_OF_ROLLS; i++)
            {
                lastRoll = SingleRoll(playerRank);
            }

            // Update previous positions queue
            if (_previousPositions.Count >= 2)
            {
                _previousPositions.Dequeue();
            }
            _previousPositions.Enqueue(lastRoll);

            // Update counters for the final roll
            _assignedPositions[lastRoll]++;
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

        #region Distribution Tracking
        public void PrintDistribution()
        {
            Console.WriteLine("\nCurrent Position Distribution:");
            foreach (var kvp in _assignedPositions)
            {
                double percentage = (_totalAssigned > 0) ?
                    (kvp.Value * 100.0 / _totalAssigned) : 0;
                Console.WriteLine($"{kvp.Key}: {kvp.Value} players ({percentage:F1}%)");
            }
        }
        #endregion
    }
}