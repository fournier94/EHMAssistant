﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EHMAssistant
{
    class PositionStrengthGenerator : IDisposable
    {
        #region Variables

        private static readonly SecureRandomGenerator _secureRandom = new SecureRandomGenerator();
        private readonly Dictionary<PositionGenerator.Position, Dictionary<int, RankOdds>> _positionStrengthOdds;
        private bool _disposed = false; // Track whether the object has been disposed

        public delegate string StrengthTranslator(PositionStrength strength);

        public class RankOdds
        {
            public Dictionary<PositionStrength, int> StrengthProbabilities { get; set; }
        }

        #endregion

        #region Enum Positions Strength
        public enum PositionStrength
        {
            Generational, // Available to every position
            Elite, // Available to every position
            FirstLine, // Available to center and winger position
            SecondLine, // Available to center and winger position
            ThirdLine, // Available to center and winger position
            FourthLine, // Available to center and winger position
            AHL, // Available to every position
            FirstPair, // Available to defensemen
            SecondPair, // Available to defensemen
            ThirdPair, // Available to defensemen
            Starter, // Available to goaltender
            Backup // Available to goaltender
        }
        #endregion
        
        #region Constructor

        public PositionStrengthGenerator()
        {
            _positionStrengthOdds = InitializeStrengthOdds();
        }

        #endregion

        #region Get Strength Odds
        public string GetStrengthOdds(PositionGenerator.Position playerPosition, int playerRank)
        {
            // Check if disposed
            ThrowIfDisposed();

            // Initialize the odds dictionary
            var odds = InitializeStrengthOdds();

            // Check if the player's position exists in the dictionary
            if (odds.ContainsKey(playerPosition) && odds[playerPosition].ContainsKey(playerRank))
            {
                // Get the RankOdds for the player's position and rank
                var rankOdds = odds[playerPosition][playerRank];

                // Get the strength probabilities and format them
                var strengthStrings = rankOdds.StrengthProbabilities
                    .Select(kv => $"{kv.Key}: {kv.Value}%")
                    .ToArray();

                return string.Join(", ", strengthStrings);
            }

            return string.Empty; // Return empty string if no odds are found for the position/rank
        }

        public string GetStrengthOddsTranslated(PositionGenerator.Position playerPosition, int playerRank, StrengthTranslator translator)
        {
            // Check if disposed
            ThrowIfDisposed();

            // Initialize the odds dictionary
            var odds = InitializeStrengthOdds();

            // Check if the player's position exists in the dictionary
            if (odds.ContainsKey(playerPosition) && odds[playerPosition].ContainsKey(playerRank))
            {
                // Get the RankOdds for the player's position and rank
                var rankOdds = odds[playerPosition][playerRank];

                // Get the strength probabilities and format them with translated strings
                var strengthStrings = rankOdds.StrengthProbabilities
                    .Select(kv => $"{translator(kv.Key)}: {kv.Value}%")
                    .ToArray();

                return string.Join(", ", strengthStrings);
            }

            return string.Empty;
        }
        #endregion

        #region Odds table
        private Dictionary<PositionGenerator.Position, Dictionary<int, RankOdds>> InitializeStrengthOdds()
        {
            var odds = new Dictionary<PositionGenerator.Position, Dictionary<int, RankOdds>>();

            #region Ailiers
            // Initialize Winger odds
            var wingerOdds = new Dictionary<int, RankOdds>
                        {
                {
                    1, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 35 },
                            { PositionStrength.Elite, 65 }
                        }
                    }
                },
                {
                    2, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 10 },
                            { PositionStrength.Elite, 90 }
                        }
                    }
                },
                {
                    3, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 60 },
                            { PositionStrength.FirstLine, 40 }
                        }
                    }
                },
                {
                    4, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 40 },
                            { PositionStrength.FirstLine, 60}
                        }
                    }
                },
                {
                    5, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 25 },
                            { PositionStrength.FirstLine, 75 }
                        }
                    }
                },
                {
                    6, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstLine, 90 }
                        }
                    }
                },
                {
                    7, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstLine, 70 },
                            { PositionStrength.SecondLine, 20 }
                        }
                    }
                },
                {
                    8, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 7 },
                            { PositionStrength.FirstLine, 45 },
                            { PositionStrength.SecondLine, 48 }
                        }
                    }
                },
                {
                    9, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 6 },
                            { PositionStrength.FirstLine, 40 },
                            { PositionStrength.SecondLine, 54 }
                        }
                    }
                },
                {
                    10, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 5 },
                            { PositionStrength.FirstLine, 23 },
                            { PositionStrength.SecondLine, 72 }
                        }
                    }
                },
                {
                    11, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 75 },
                            { PositionStrength.ThirdLine, 10 }
                        }
                    }
                },
                {
                    12, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 65 },
                            { PositionStrength.ThirdLine, 20 }
                        }
                    }
                },
                {
                    13, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 50 },
                            { PositionStrength.ThirdLine, 35 }
                        }
                    }
                },
                {
                    14, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 45 },
                            { PositionStrength.ThirdLine, 40 }
                        }
                    }
                },
                {
                    15, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 40 },
                            { PositionStrength.ThirdLine, 45 }
                        }
                    }
                },
                {
                    16, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 28 },
                            { PositionStrength.ThirdLine, 62 }
                        }
                    }
                },
                {
                    17, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 25 },
                            { PositionStrength.ThirdLine, 65 }
                        }
                    }
                },
                {
                    18, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 25 },
                            { PositionStrength.ThirdLine, 65 }
                        }
                    }
                },
                {
                    19, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 20 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 5 }
                        }
                    }
                },
                {
                    20, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    21, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    22, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    23, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 60 },
                            { PositionStrength.FourthLine, 15 }
                        }
                    }
                },
                {
                    24, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 60 },
                            { PositionStrength.FourthLine, 15 }
                        }
                    }
                },
                {
                    25, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 55 },
                            { PositionStrength.FourthLine, 25 }
                        }
                    }
                },
                {
                    26, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 55 },
                            { PositionStrength.FourthLine, 25 }
                        }
                    }
                },
                {
                    27, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 50 },
                            { PositionStrength.FourthLine, 30 }
                        }
                    }
                },
                {
                    28, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 45 },
                            { PositionStrength.FourthLine, 35 }
                        }
                    }
                },
                {
                    29, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 45 },
                            { PositionStrength.FourthLine, 35 }
                        }
                    }
                },
                {
                    30, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 40 }
                        }
                    }
                },
                {
                    31, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    32, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    33, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    34, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    35, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    36, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    37, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    38, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    39, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 30 },
                            { PositionStrength.FourthLine, 38 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    40, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 8 },
                            { PositionStrength.ThirdLine, 30 },
                            { PositionStrength.FourthLine, 38 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    41, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    42, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    43, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    44, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    45, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    46, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    47, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    48, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    49, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    50, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    51, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    52, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 55 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    53, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 55 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    54, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    55, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    56, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    57, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    58, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    59, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    60, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                }
            };

            odds[PositionGenerator.Position.Winger] = wingerOdds;
            #endregion

            #region Centres
            var centerOdds = new Dictionary<int, RankOdds>
            {
                {
                    1, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 35 },
                            { PositionStrength.Elite, 65 }
                        }
                    }
                },
                {
                    2, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 10 },
                            { PositionStrength.Elite, 90 }
                        }
                    }
                },
                {
                    3, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 60 },
                            { PositionStrength.FirstLine, 40 }
                        }
                    }
                },
                {
                    4, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 40 },
                            { PositionStrength.FirstLine, 60}
                        }
                    }
                },
                {
                    5, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 25 },
                            { PositionStrength.FirstLine, 75 }
                        }
                    }
                },
                {
                    6, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstLine, 90 }
                        }
                    }
                },
                {
                    7, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstLine, 70 },
                            { PositionStrength.SecondLine, 20 }
                        }
                    }
                },
                {
                    8, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 7 },
                            { PositionStrength.FirstLine, 45 },
                            { PositionStrength.SecondLine, 48 }
                        }
                    }
                },
                {
                    9, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 6 },
                            { PositionStrength.FirstLine, 40 },
                            { PositionStrength.SecondLine, 54 }
                        }
                    }
                },
                {
                    10, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 5 },
                            { PositionStrength.FirstLine, 23 },
                            { PositionStrength.SecondLine, 72 }
                        }
                    }
                },
                {
                    11, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 75 },
                            { PositionStrength.ThirdLine, 10 }
                        }
                    }
                },
                {
                    12, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 65 },
                            { PositionStrength.ThirdLine, 20 }
                        }
                    }
                },
                {
                    13, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 50 },
                            { PositionStrength.ThirdLine, 35 }
                        }
                    }
                },
                {
                    14, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 45 },
                            { PositionStrength.ThirdLine, 40 }
                        }
                    }
                },
                {
                    15, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstLine, 12 },
                            { PositionStrength.SecondLine, 40 },
                            { PositionStrength.ThirdLine, 45 }
                        }
                    }
                },
                {
                    16, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 28 },
                            { PositionStrength.ThirdLine, 62 }
                        }
                    }
                },
                {
                    17, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 25 },
                            { PositionStrength.ThirdLine, 65 }
                        }
                    }
                },
                {
                    18, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 25 },
                            { PositionStrength.ThirdLine, 65 }
                        }
                    }
                },
                {
                    19, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 20 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 5 }
                        }
                    }
                },
                {
                    20, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    21, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    22, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 65 },
                            { PositionStrength.FourthLine, 10 }
                        }
                    }
                },
                {
                    23, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 60 },
                            { PositionStrength.FourthLine, 15 }
                        }
                    }
                },
                {
                    24, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 8 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 60 },
                            { PositionStrength.FourthLine, 15 }
                        }
                    }
                },
                {
                    25, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 55 },
                            { PositionStrength.FourthLine, 25 }
                        }
                    }
                },
                {
                    26, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 55 },
                            { PositionStrength.FourthLine, 25 }
                        }
                    }
                },
                {
                    27, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 50 },
                            { PositionStrength.FourthLine, 30 }
                        }
                    }
                },
                {
                    28, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 45 },
                            { PositionStrength.FourthLine, 35 }
                        }
                    }
                },
                {
                    29, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 45 },
                            { PositionStrength.FourthLine, 35 }
                        }
                    }
                },
                {
                    30, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstLine, 5 },
                            { PositionStrength.SecondLine, 13 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 40 }
                        }
                    }
                },
                {
                    31, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    32, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    33, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    34, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    35, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 4 },
                            { PositionStrength.SecondLine, 15 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    36, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    37, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 40 },
                            { PositionStrength.FourthLine, 30 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    38, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 35 },
                            { PositionStrength.FourthLine, 35 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    39, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 11 },
                            { PositionStrength.ThirdLine, 30 },
                            { PositionStrength.FourthLine, 38 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    40, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 8 },
                            { PositionStrength.ThirdLine, 30 },
                            { PositionStrength.FourthLine, 38 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    41, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    42, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    43, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 25 },
                            { PositionStrength.FourthLine, 40 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    44, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    45, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    46, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 20 },
                            { PositionStrength.FourthLine, 45 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    47, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    48, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    49, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    50, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    51, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 15 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    52, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 55 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    53, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 55 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    54, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    55, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    56, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    57, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    58, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    59, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    60, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstLine, 3 },
                            { PositionStrength.SecondLine, 6 },
                            { PositionStrength.ThirdLine, 10 },
                            { PositionStrength.FourthLine, 50 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                }
            };

            odds[PositionGenerator.Position.Center] = centerOdds;
            #endregion
            
            #region Defenseurs
            var defenseurOdds = new Dictionary<int, RankOdds>
            {
                {
                    1, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 35 },
                            { PositionStrength.Elite, 65 }
                        }
                    }
                },
                {
                    2, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 10 },
                            { PositionStrength.Elite, 90 }
                        }
                    }
                },
                {
                    3, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 60 },
                            { PositionStrength.FirstPair, 40 }
                        }
                    }
                },
                {
                    4, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 40 },
                            { PositionStrength.FirstPair, 60}
                        }
                    }
                },
                {
                    5, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 25 },
                            { PositionStrength.FirstPair, 75 }
                        }
                    }
                },
                {
                    6, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstPair, 90 }
                        }
                    }
                },
                {
                    7, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 10 },
                            { PositionStrength.FirstPair, 65 },
                            { PositionStrength.SecondPair, 25 }
                        }
                    }
                },
                {
                    8, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 7 },
                            { PositionStrength.FirstPair, 40 },
                            { PositionStrength.SecondPair, 53 }
                        }
                    }
                },
                {
                    9, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 6 },
                            { PositionStrength.FirstPair, 35 },
                            { PositionStrength.SecondPair, 59 }
                        }
                    }
                },
                {
                    10, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstPair, 22 },
                            { PositionStrength.SecondPair, 75 }
                        }
                    }
                },
                {
                    11, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstPair, 12 },
                            { PositionStrength.SecondPair, 75 },
                            { PositionStrength.ThirdPair, 10 }
                        }
                    }
                },
                {
                    12, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstPair, 12 },
                            { PositionStrength.SecondPair, 65 },
                            { PositionStrength.ThirdPair, 20 }
                        }
                    }
                },
                {
                    13, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 3 },
                            { PositionStrength.FirstPair, 12 },
                            { PositionStrength.SecondPair, 60 },
                            { PositionStrength.ThirdPair, 25 }
                        }
                    }
                },
                {
                    14, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 13 },
                            { PositionStrength.SecondPair, 55 },
                            { PositionStrength.ThirdPair, 30 }
                        }
                    }
                },
                {
                    15, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 13 },
                            { PositionStrength.SecondPair, 50 },
                            { PositionStrength.ThirdPair, 35 }
                        }
                    }
                },
                {
                    16, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 13 },
                            { PositionStrength.SecondPair, 40 },
                            { PositionStrength.ThirdPair, 45 }
                        }
                    }
                },
                {
                    17, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 13 },
                            { PositionStrength.SecondPair, 35 },
                            { PositionStrength.ThirdPair, 50 }
                        }
                    }
                },
                {
                    18, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 10 },
                            { PositionStrength.SecondPair, 33 },
                            { PositionStrength.ThirdPair, 55 }
                        }
                    }
                },
                {
                    19, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 10 },
                            { PositionStrength.SecondPair, 33 },
                            { PositionStrength.ThirdPair, 55 }
                        }
                    }
                },
                {
                    20, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 10 },
                            { PositionStrength.SecondPair, 25 },
                            { PositionStrength.ThirdPair, 63 }
                        }
                    }
                },
                {
                    21, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 10 },
                            { PositionStrength.SecondPair, 18 },
                            { PositionStrength.ThirdPair, 70 }
                        }
                    }
                },
                {
                    22, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 10 },
                            { PositionStrength.SecondPair, 18 },
                            { PositionStrength.ThirdPair, 70 }
                        }
                    }
                },
                {
                    23, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    24, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    25, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    26, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    27, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    28, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 75 }
                        }
                    }
                },
                {
                    29, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    30, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 2 },
                            { PositionStrength.FirstPair, 8 },
                            { PositionStrength.SecondPair, 15 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 10 }
                        }
                    }
                },
                {
                    31, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 14 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    32, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 14 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    33, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 14 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 15 }
                        }
                    }
                },
                {
                    34, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 14 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    35, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 14 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    36, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 9 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    37, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 9 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    38, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 9 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    39, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 5 },
                            { PositionStrength.SecondPair, 9 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 20 }
                        }
                    }
                },
                {
                    40, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    41, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    42, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    43, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    44, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    45, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 3 },
                            { PositionStrength.SecondPair, 11 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    46, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    47, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    48, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    49, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    50, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    51, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    52, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    53, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 65 },
                            { PositionStrength.AHL, 25 }
                        }
                    }
                },
                {
                    54, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    55, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    56, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    57, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    58, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    59, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                },
                {
                    60, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Elite, 1 },
                            { PositionStrength.FirstPair, 2 },
                            { PositionStrength.SecondPair, 7 },
                            { PositionStrength.ThirdPair, 60 },
                            { PositionStrength.AHL, 30 }
                        }
                    }
                }
            };

            odds[PositionGenerator.Position.Defenseman] = defenseurOdds;
            #endregion

            #region Goaltenders
            var goaltenderOdds = new Dictionary<int, RankOdds>
            {
                {
                    1, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    2, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    3, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    4, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    5, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    6, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    7, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    8, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    9, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 20 },
                            { PositionStrength.Elite, 70 },
                            { PositionStrength.Starter, 10 }
                        }
                    }
                },
                {
                    10, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 30 },
                            { PositionStrength.Starter, 65 }
                        }
                    }
                },
                {
                    11, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 30 },
                            { PositionStrength.Starter, 65 }
                        }
                    }
                },
                {
                    12, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 30 },
                            { PositionStrength.Starter, 65 }
                        }
                    }
                },
                {
                    13, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 30 },
                            { PositionStrength.Starter, 65 }
                        }
                    }
                },
                {
                    14, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 20 },
                            { PositionStrength.Starter, 75 }
                        }
                    }
                },
                {
                    15, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 5 },
                            { PositionStrength.Elite, 20 },
                            { PositionStrength.Starter, 75 }
                        }
                    }
                },
                {
                    16, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 85 }
                        }
                    }
                },
                {
                    17, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 85 }
                        }
                    }
                },
                {
                    18, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    19, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    20, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    21, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    22, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    23, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    24, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 65 },
                            { PositionStrength.Backup, 20 }
                        }
                    }
                },
                {
                    25, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    26, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    27, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    28, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    29, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    30, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 3 },
                            { PositionStrength.Elite, 12 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 25 }
                        }
                    }
                },
                {
                    31, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 30 }
                        }
                    }
                },
                {
                    32, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 30 }
                        }
                    }
                },
                {
                    33, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 30 }
                        }
                    }
                },
                {
                    34, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 30 }
                        }
                    }
                },
                {
                    35, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 60 },
                            { PositionStrength.Backup, 30 }
                        }
                    }
                },
                {
                    36, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    37, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    38, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    39, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    40, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    41, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    42, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    43, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    44, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    45, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 45 },
                            { PositionStrength.Backup, 45 }
                        }
                    }
                },
                {
                    46, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    47, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    48, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    49, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    50, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    51, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    52, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    53, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    54, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    55, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    56, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    57, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    58, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    59, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                },
                {
                    60, new RankOdds
                    {
                        StrengthProbabilities = new Dictionary<PositionStrength, int>
                        {
                            { PositionStrength.Generational, 2 },
                            { PositionStrength.Elite, 8 },
                            { PositionStrength.Starter, 30 },
                            { PositionStrength.Backup, 60 }
                        }
                    }
                }
            };

            odds[PositionGenerator.Position.Goaltender] = goaltenderOdds;
            #endregion

            return odds;
        }
        #endregion

        #region Roll Strength of player (Elite/1st line, etc...)
        internal PositionStrength RollStrength(PositionGenerator.Position position, int rank)
        {
            // Check if disposed
            ThrowIfDisposed();

            // Find the closest rank in odds table that's less than or equal to the player's rank
            var positionOdds = _positionStrengthOdds[position];
            int closestRank = positionOdds.Keys
                .Where(r => r <= rank)
                .OrderByDescending(r => r)
                .FirstOrDefault();
            if (closestRank == 0)
            {
                closestRank = positionOdds.Keys.Max();
            }
            var strengthOdds = positionOdds[closestRank].StrengthProbabilities;

            // Create weighted list based on probabilities
            List<PositionStrength> weightedStrengths = new List<PositionStrength>();
            foreach (var kvp in strengthOdds)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    weightedStrengths.Add(kvp.Key);
                }
            }

            // Use SecureRandomGenerator to get a random index
            int randomIndex = _secureRandom.GetRandomValue(0, weightedStrengths.Count);
            return weightedStrengths[randomIndex];
        }
        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // No need to dispose _secureRandom here as it's now static
                // and will be handled by the application lifecycle
            }

            _disposed = true;
        }

        // Finalizer
        ~PositionStrengthGenerator()
        {
            Dispose(false);
        }

        // Helper method to throw if this object has been disposed
        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(PositionStrengthGenerator));
            }
        }

        #endregion
    }
}