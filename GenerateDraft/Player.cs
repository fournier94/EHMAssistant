using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHMAssistant
{
    class Player
    {
        #region Scripts
        private readonly NameGenerator _nameGen;
        private static readonly SecureRandomGenerator _secureRandom = new SecureRandomGenerator();
        #endregion
        
        #region Player informations
        public string Name { get; set; }
        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }
        
        public CountryGenerator.Country PlayerCountry { get; set; }
        public int Rank { get; set; }
        public string Height { get; set; }

        public PositionGenerator.Position PlayerPosition { get; set; }
        public PlayerTypeGenerator.PlayerType PlayerType { get; set; }
        public PositionStrengthGenerator.PositionStrength PositionStrength { get; set; }
        public string Handedness { get; set; } // "Right" = 0 and "Left" = 1

        public int TargetOffense { get; set; }
        public int TargetDefense { get; set; }

        public int TeamNumber { get; set; }
        #endregion

        #region Final Attributes
        // Attributes
        public int Fighting { get; set; }

        // OFF
        public int Shooting { get; set; }
        public int Playmaking { get; set; }
        public int Stickhandling { get; set; }

        // DEF
        public int Checking { get; set; }
        public int Positioning { get; set; }
        public int Hitting { get; set; }

        // Other attributes
        public int Skating { get; set; }
        public int Endurance { get; set; }
        public int Penalty { get; set; }
        public int Faceoffs { get; set; }
        public int Leadership { get; set; }
        public int AttributeStrength { get; set; }

        // Hidden attributes
        public int Potential { get; set; }
        public int Constance { get; set; }
        public int Greed { get; set; }

        public int Offense
        {
            get
            {
                return (int)Math.Round((Shooting + Playmaking + Stickhandling) / 3.0);
            }
        }

        public int Defense
        {
            get
            {
                return (int)Math.Round((Checking + Positioning + Hitting) / 3.0);
            }
        }

        public int Overall
        {
            get
            {
                return (int)Math.Round(
            (Shooting + Playmaking + Stickhandling +
             Checking + Positioning + Hitting +
             Skating + Endurance + Penalty +
             Faceoffs + AttributeStrength + Constance + Greed + Potential) / 14.0);
            }
        }
        #endregion
        
        #region Starting attributes

        public int StartingFighting { get; set; }

        // OFF
        public int StartingShooting { get; set; }
        public int StartingPlaymaking { get; set; }
        public int StartingStickhandling { get; set; }

        // DEF
        public int StartingChecking { get; set; }
        public int StartingPositioning { get; set; }
        public int StartingHitting { get; set; }

        // Other attributes
        public int StartingSkating { get; set; }
        public int StartingEndurance { get; set; }
        public int StartingPenalty { get; set; }
        public int StartingFaceoffs { get; set; }
        public int StartingLeadership { get; set; }
        public int StartingAttributeStrength { get; set; }

        public int StartingOffense
        {
            get
            {
                return (int)Math.Round((StartingShooting + StartingPlaymaking + StartingStickhandling) / 3.0);
            }
        }

        public int StartingDefense
        {
            get
            {
                return (int)Math.Round((StartingChecking + StartingPositioning + StartingHitting) / 3.0);
            }
        }

        public int StartingOverall
        {
            get
            {
                return (int)Math.Round(
            (StartingShooting + StartingPlaymaking + StartingStickhandling +
             StartingChecking + StartingPositioning + StartingHitting +
             StartingSkating + StartingEndurance + StartingPenalty +
             StartingFaceoffs + StartingLeadership + StartingAttributeStrength + Constance + Greed + Potential) / 14.0);
            }
        }

        #endregion
        
        #region Ceilings
        public int CeilingFighting => GetCeilingValue(Fighting, Potential);
        public int CeilingShooting => GetCeilingValue(Shooting, Potential);
        public int CeilingPlaymaking => GetCeilingValue(Playmaking, Potential);
        public int CeilingStickhandling => GetCeilingValue(Stickhandling, Potential);
        public int CeilingChecking => GetCeilingValue(Checking, Potential);
        public int CeilingPositioning => GetCeilingValue(Positioning, Potential);
        public int CeilingHitting => GetCeilingValue(Hitting, Potential);
        public int CeilingSkating => GetCeilingValue(Skating, Potential);
        public int CeilingEndurance => GetCeilingValue(Endurance, Potential);
        public int CeilingPenalty => GetCeilingValue(Penalty, Potential);
        public int CeilingFaceoffs => GetCeilingValue(Faceoffs, Potential);
        public int CeilingLeadership => GetCeilingValue(Leadership, Potential);
        public int CeilingAttributeStrength => GetCeilingValue(AttributeStrength, Potential);

        private int GetCeilingValue(int value, int pot)
        {
            int ceiling = (value * 100) / pot;
            return ceiling;
        }
        #endregion

        #region Player Constructor
        public Player(int rank, PositionGenerator posGen, CountryGenerator countryGen,
    PlayerTypeGenerator typeGen, PositionStrengthGenerator strengthGen,
    HeightGenerator heightGen, GenerateDraft draftForm)
        {
            Rank = rank;
            PlayerPosition = posGen.RollPosition(rank);
            PlayerCountry = countryGen.RollCountry();
            PlayerType = typeGen.RollPlayerType(PlayerPosition, Rank);
            PositionStrength = strengthGen.RollStrength(PlayerPosition, Rank);
            _nameGen = new NameGenerator();
            Greed = 75;

            // Handle Canadian player naming with language consideration
            if (PlayerCountry == CountryGenerator.Country.Canada)
            {
                int frenchCanadianChance = _secureRandom.GetRandomValue(1, 101); // 1 to 100 inclusive
                bool isFrenchCanadian = frenchCanadianChance <= 28; // 28% chance as per requirements
                Name = _nameGen.GenerateRandomName(PlayerCountry, isFrenchCanadian);
            }
            else
            {
                // For non-Canadian players, use the standard name generation
                Name = _nameGen.GenerateRandomName(PlayerCountry);
            }

            Height = heightGen.RollHeight(PlayerType);
            BirthDateGenerator birthDateGen = new BirthDateGenerator(draftForm);
            birthDateGen.GenerateBirthDate(this);
            Handedness = _secureRandom.GetRandomValue(0, 2) == 0 ? "Right" : "Left";
        }

        public Player()
        {
            
        }
        #endregion
    }
}
