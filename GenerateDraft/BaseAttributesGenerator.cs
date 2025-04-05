using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHMAssistant
{
    class BaseAttributesGenerator
    {
        private readonly Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), PlayerAttributes> _baseAttributes;

        private class PlayerAttributes
        {
            public int Fighting { get; set; }
            public int Shooting { get; set; }
            public int Playmaking { get; set; }
            public int Stickhandling { get; set; }
            public int Checking { get; set; }
            public int Positioning { get; set; }
            public int Hitting { get; set; }
            public int Skating { get; set; }
            public int Endurance { get; set; }
            public int Penalty { get; set; }
            public int Faceoffs { get; set; }
            public int Leadership { get; set; }
            public int AttributeStrength { get; set; }
            public int Potential { get; set; }
            public int Constance { get; set; }
        }

        public BaseAttributesGenerator()
        {
            _baseAttributes = InitializeBaseAttributes();
        }

        public void SetPlayerBaseAttributes(Player player)
        {
            var key = (player.PlayerType, player.PositionStrength);

            if (_baseAttributes.TryGetValue(key, out PlayerAttributes baseStats))
            {
                // Set base attributes
                player.Fighting = baseStats.Fighting;
                player.Shooting = baseStats.Shooting;
                player.Playmaking = baseStats.Playmaking;
                player.Stickhandling = baseStats.Stickhandling;
                player.Checking = baseStats.Checking;
                player.Positioning = baseStats.Positioning;
                player.Hitting = baseStats.Hitting;
                player.Skating = baseStats.Skating;
                player.Endurance = baseStats.Endurance;
                player.Penalty = baseStats.Penalty;
                player.Leadership = baseStats.Leadership;
                player.AttributeStrength = baseStats.AttributeStrength;
                player.Potential = baseStats.Potential;
                player.Constance = baseStats.Constance;

                // Set faceoffs based on position
                if (player.PlayerPosition == PositionGenerator.Position.Center)
                    player.Faceoffs = 75;
                else if (player.PlayerPosition == PositionGenerator.Position.Winger || player.PlayerPosition == PositionGenerator.Position.Defenseman)
                    player.Faceoffs = 65;
                else if (player.PlayerPosition == PositionGenerator.Position.Goaltender)
                    player.Faceoffs = 85;
            }
        }

        private Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), PlayerAttributes> InitializeBaseAttributes()
        {
            var attributes = new Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), PlayerAttributes>();

            #region Attaquants

            #region Sniper
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 99,
                Playmaking = 76,
                Stickhandling = 80,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 87,
                Endurance = 90,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 85,
                Playmaking = 74,
                Stickhandling = 78,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 83,
                Endurance = 85,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 80,
                Playmaking = 70,
                Stickhandling = 74,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 79,
                Endurance = 80,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 78
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 77,
                Playmaking = 65,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 75,
                Endurance = 75,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 75
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 75,
                Playmaking = 60,
                Stickhandling = 65,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 71,
                Endurance = 71,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 73,
                Playmaking = 58,
                Stickhandling = 63,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 68,
                Endurance = 66,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 81,
                Constance = 65
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 60,
                Stickhandling = 60,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 62,
                Endurance = 62,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 71,
                Constance = 65
            });
            #endregion

            #region FabricantDeJeu
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 76,
                Playmaking = 99,
                Stickhandling = 80,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 87,
                Endurance = 90,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 74,
                Playmaking = 85,
                Stickhandling = 78,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 83,
                Endurance = 85,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 70,
                Playmaking = 80,
                Stickhandling = 74,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 79,
                Endurance = 80,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 78
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 65,
                Playmaking = 77,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 75,
                Endurance = 75,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 75
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 60,
                Playmaking = 75,
                Stickhandling = 65,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 71,
                Endurance = 71,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 91,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 58,
                Playmaking = 73,
                Stickhandling = 63,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 68,
                Endurance = 66,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 81,
                Constance = 65
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 60,
                Stickhandling = 60,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 62,
                Endurance = 62,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 71,
                Constance = 65
            });
            #endregion

            #region AttaquantOffensif
            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 80,
                Playmaking = 80,
                Stickhandling = 80,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 87,
                Endurance = 90,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 93,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 80,
                Playmaking = 80,
                Stickhandling = 80,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 83,
                Endurance = 85,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 93,
                Constance = 85
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 75,
                Playmaking = 75,
                Stickhandling = 75,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 79,
                Endurance = 80,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 93,
                Constance = 78
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 70,
                Playmaking = 70,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 75,
                Endurance = 75,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 93,
                Constance = 75
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 70,
                Playmaking = 70,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 71,
                Endurance = 71,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 93,
                Constance = 70
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 65,
                Playmaking = 65,
                Stickhandling = 65,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 68,
                Endurance = 66,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 83,
                Constance = 65
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 62,
                Stickhandling = 62,
                Checking = 60,
                Positioning = 60,
                Hitting = 53,
                Skating = 62,
                Endurance = 62,
                Penalty = 55,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 73,
                Constance = 65
            });
            #endregion

            #region AttaquantDePuissance
            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 75,
                Playmaking = 75,
                Stickhandling = 75,
                Checking = 76,
                Positioning = 72,
                Hitting = 85,
                Skating = 84,
                Endurance = 90,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 82,
                Potential = 94,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 73,
                Playmaking = 73,
                Stickhandling = 73,
                Checking = 73,
                Positioning = 69,
                Hitting = 83,
                Skating = 81,
                Endurance = 85,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 80,
                Potential = 94,
                Constance = 85
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 70,
                Playmaking = 70,
                Stickhandling = 70,
                Checking = 70,
                Positioning = 66,
                Hitting = 80,
                Skating = 78,
                Endurance = 80,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 78,
                Potential = 94,
                Constance = 78
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 67,
                Playmaking = 67,
                Stickhandling = 67,
                Checking = 67,
                Positioning = 63,
                Hitting = 80,
                Skating = 75,
                Endurance = 75,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 76,
                Potential = 94,
                Constance = 75
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 60,
                Playmaking = 60,
                Stickhandling = 60,
                Checking = 67,
                Positioning = 63,
                Hitting = 80,
                Skating = 73,
                Endurance = 71,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 94,
                Constance = 70
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 58,
                Playmaking = 58,
                Stickhandling = 58,
                Checking = 67,
                Positioning = 63,
                Hitting = 80,
                Skating = 68,
                Endurance = 66,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 84,
                Constance = 65
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 62,
                Playmaking = 62,
                Stickhandling = 62,
                Checking = 58,
                Positioning = 55,
                Hitting = 62,
                Skating = 62,
                Endurance = 62,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 74,
                Constance = 65
            });
            #endregion

            #region AttaquantPolyvalent
            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 80,
                Playmaking = 80,
                Stickhandling = 83,
                Checking = 76,
                Positioning = 76,
                Hitting = 76,
                Skating = 87,
                Endurance = 90,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 78,
                Playmaking = 78,
                Stickhandling = 82,
                Checking = 74,
                Positioning = 74,
                Hitting = 74,
                Skating = 83,
                Endurance = 85,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 85
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 75,
                Playmaking = 75,
                Stickhandling = 78,
                Checking = 72,
                Positioning = 72,
                Hitting = 72,
                Skating = 79,
                Endurance = 80,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 78
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 72,
                Playmaking = 72,
                Stickhandling = 75,
                Checking = 67,
                Positioning = 67,
                Hitting = 67,
                Skating = 75,
                Endurance = 75,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 75
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 65,
                Playmaking = 65,
                Stickhandling = 70,
                Checking = 66,
                Positioning = 66,
                Hitting = 66,
                Skating = 71,
                Endurance = 71,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 70
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 62,
                Stickhandling = 67,
                Checking = 65,
                Positioning = 65,
                Hitting = 65,
                Skating = 68,
                Endurance = 66,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 85,
                Constance = 65
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 62,
                Stickhandling = 62,
                Checking = 62,
                Positioning = 62,
                Hitting = 62,
                Skating = 62,
                Endurance = 62,
                Penalty = 58,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 75,
                Constance = 65
            });
            #endregion

            #region JoueurDeCaractere
            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 77,
                Playmaking = 77,
                Stickhandling = 77,
                Checking = 68,
                Positioning = 68,
                Hitting = 68,
                Skating = 89,
                Endurance = 90,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 99,
                Constance = 100
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 75,
                Playmaking = 75,
                Stickhandling = 75,
                Checking = 68,
                Positioning = 68,
                Hitting = 68,
                Skating = 85,
                Endurance = 87,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 99,
                Constance = 90
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.FirstLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 70,
                Playmaking = 70,
                Stickhandling = 70,
                Checking = 68,
                Positioning = 68,
                Hitting = 68,
                Skating = 81,
                Endurance = 83,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 99,
                Constance = 83
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.SecondLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 68,
                Playmaking = 68,
                Stickhandling = 68,
                Checking = 65,
                Positioning = 65,
                Hitting = 65,
                Skating = 77,
                Endurance = 78,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 99,
                Constance = 78
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.ThirdLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 63,
                Playmaking = 63,
                Stickhandling = 63,
                Checking = 65,
                Positioning = 65,
                Hitting = 65,
                Skating = 74,
                Endurance = 75,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 89,
                Constance = 75
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.FourthLine), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 55,
                Playmaking = 55,
                Stickhandling = 55,
                Checking = 65,
                Positioning = 65,
                Hitting = 65,
                Skating = 71,
                Endurance = 72,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 89,
                Constance = 72
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 62,
                Playmaking = 62,
                Stickhandling = 62,
                Checking = 62,
                Positioning = 62,
                Hitting = 65,
                Skating = 64,
                Endurance = 66,
                Penalty = 52,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 79,
                Constance = 65
            });
            #endregion

            #endregion

            #region Defenseurs

            #region DefenseurOffensif
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 83,
                Playmaking = 83,
                Stickhandling = 83,
                Checking = 65,
                Positioning = 65,
                Hitting = 60,
                Skating = 89,
                Endurance = 90,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 75,
                Potential = 98,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 77,
                Playmaking = 77,
                Stickhandling = 77,
                Checking = 65,
                Positioning = 65,
                Hitting = 60,
                Skating = 85,
                Endurance = 85,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 75,
                Potential = 98,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.FirstPair), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 75,
                Playmaking = 75,
                Stickhandling = 75,
                Checking = 64,
                Positioning = 64,
                Hitting = 59,
                Skating = 81,
                Endurance = 80,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 98,
                Constance = 78
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.SecondPair), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 73,
                Playmaking = 73,
                Stickhandling = 73,
                Checking = 62,
                Positioning = 62,
                Hitting = 57,
                Skating = 77,
                Endurance = 75,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 88,
                Constance = 75
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.ThirdPair), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 70,
                Playmaking = 70,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 60,
                Hitting = 55,
                Skating = 73,
                Endurance = 68,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 88,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 20,
                Shooting = 67,
                Playmaking = 67,
                Stickhandling = 67,
                Checking = 60,
                Positioning = 60,
                Hitting = 65,
                Skating = 65,
                Endurance = 65,
                Penalty = 56,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 78,
                Constance = 65
            });
            #endregion

            #region DefenseurDefensif
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 73,
                Playmaking = 73,
                Stickhandling = 78,
                Checking = 68,
                Positioning = 70,
                Hitting = 65,
                Skating = 85,
                Endurance = 90,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 68,
                Playmaking = 68,
                Stickhandling = 73,
                Checking = 68,
                Positioning = 70,
                Hitting = 65,
                Skating = 81,
                Endurance = 85,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.FirstPair), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 60,
                Playmaking = 60,
                Stickhandling = 65,
                Checking = 66,
                Positioning = 68,
                Hitting = 63,
                Skating = 77,
                Endurance = 80,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 95,
                Constance = 78
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.SecondPair), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 55,
                Playmaking = 55,
                Stickhandling = 60,
                Checking = 64,
                Positioning = 66,
                Hitting = 61,
                Skating = 73,
                Endurance = 75,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 85,
                Constance = 75
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.ThirdPair), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 50,
                Playmaking = 50,
                Stickhandling = 55,
                Checking = 63,
                Positioning = 65,
                Hitting = 60,
                Skating = 69,
                Endurance = 68,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 85,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 40,
                Shooting = 60,
                Playmaking = 60,
                Stickhandling = 60,
                Checking = 68,
                Positioning = 68,
                Hitting = 65,
                Skating = 65,
                Endurance = 65,
                Penalty = 53,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 75,
                Constance = 65
            });
            #endregion

            #region DefenseurPhysique
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 73,
                Playmaking = 68,
                Stickhandling = 68,
                Checking = 86,
                Positioning = 77,
                Hitting = 92,
                Skating = 83,
                Endurance = 90,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 82,
                Potential = 97,
                Constance = 95
            });

            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 68,
                Playmaking = 63,
                Stickhandling = 63,
                Checking = 83,
                Positioning = 76,
                Hitting = 87,
                Skating = 79,
                Endurance = 85,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 85,
                AttributeStrength = 79,
                Potential = 97,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.FirstPair), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 65,
                Playmaking = 60,
                Stickhandling = 60,
                Checking = 79,
                Positioning = 75,
                Hitting = 85,
                Skating = 75,
                Endurance = 80,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 77,
                Potential = 97,
                Constance = 78
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.SecondPair), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 60,
                Playmaking = 55,
                Stickhandling = 55,
                Checking = 77,
                Positioning = 73,
                Hitting = 85,
                Skating = 71,
                Endurance = 75,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 97,
                Constance = 75
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.ThirdPair), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 55,
                Playmaking = 50,
                Stickhandling = 50,
                Checking = 73,
                Positioning = 66,
                Hitting = 84,
                Skating = 68,
                Endurance = 68,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 87,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 50,
                Shooting = 70,
                Playmaking = 65,
                Stickhandling = 70,
                Checking = 60,
                Positioning = 55,
                Hitting = 70,
                Skating = 65,
                Endurance = 65,
                Penalty = 50,
                Faceoffs = 55,
                Leadership = 87,
                AttributeStrength = 75,
                Potential = 77,
                Constance = 65
            });
            #endregion

            #endregion

            #region Gardiens
            attributes.Add((PlayerTypeGenerator.PlayerType.Gardien, PositionStrengthGenerator.PositionStrength.Generational), new PlayerAttributes
            {
                Fighting = 92,
                Shooting = 92,
                Playmaking = 92,
                Stickhandling = 92,
                Checking = 92,
                Positioning = 92,
                Hitting = 92,
                Skating = 92,
                Endurance = 92,
                Penalty = 92,
                Faceoffs = 92,
                Leadership = 92,
                AttributeStrength = 92,
                Potential = 92,
                Constance = 95
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Gardien, PositionStrengthGenerator.PositionStrength.Elite), new PlayerAttributes
            {
                Fighting = 87,
                Shooting = 87,
                Playmaking = 87,
                Stickhandling = 87,
                Checking = 87,
                Positioning = 87,
                Hitting = 87,
                Skating = 87,
                Endurance = 87,
                Penalty = 87,
                Faceoffs = 87,
                Leadership = 87,
                AttributeStrength = 87,
                Potential = 87,
                Constance = 85
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Gardien, PositionStrengthGenerator.PositionStrength.Starter), new PlayerAttributes
            {
                Fighting = 83,
                Shooting = 83,
                Playmaking = 83,
                Stickhandling = 83,
                Checking = 83,
                Positioning = 83,
                Hitting = 83,
                Skating = 83,
                Endurance = 83,
                Penalty = 83,
                Faceoffs = 83,
                Leadership = 83,
                AttributeStrength = 83,
                Potential = 83,
                Constance = 80
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Gardien, PositionStrengthGenerator.PositionStrength.Backup), new PlayerAttributes
            {
                Fighting = 77,
                Shooting = 77,
                Playmaking = 77,
                Stickhandling = 77,
                Checking = 77,
                Positioning = 77,
                Hitting = 77,
                Skating = 77,
                Endurance = 77,
                Penalty = 77,
                Faceoffs = 77,
                Leadership = 77,
                AttributeStrength = 77,
                Potential = 77,
                Constance = 70
            });
            attributes.Add((PlayerTypeGenerator.PlayerType.Gardien, PositionStrengthGenerator.PositionStrength.AHL), new PlayerAttributes
            {
                Fighting = 69,
                Shooting = 69,
                Playmaking = 69,
                Stickhandling = 69,
                Checking = 69,
                Positioning = 69,
                Hitting = 69,
                Skating = 69,
                Endurance = 69,
                Penalty = 69,
                Faceoffs = 69,
                Leadership = 69,
                AttributeStrength = 69,
                Potential = 69,
                Constance = 70
            });
            #endregion

            return attributes;
        }
    }
}
