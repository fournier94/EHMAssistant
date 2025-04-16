using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHMAssistant
{
    class PlayerTargetAttributes : IDisposable
    {
        private readonly SecureRandomGenerator _secureRandom = new SecureRandomGenerator();
        private bool _disposed;

        public struct AttributeTargets
        {
            public int TargetOffense { get; }
            public int TargetDefense { get; }
            public int DistributionValue { get; }

            public AttributeTargets(int off, int def, int distributionValue)
            {
                TargetOffense = off;
                TargetDefense = def;
                DistributionValue = distributionValue;
            }
        }

        private readonly Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), AttributeTargets> _targetAttributes;

        public PlayerTargetAttributes()
        {
            _targetAttributes = InitializeTargetAttributes();
        }

        public (int targetOff, int targetDef) GetTargets(PlayerTypeGenerator.PlayerType playerType,
            PositionStrengthGenerator.PositionStrength strength)
        {
            var key = (playerType, strength);
            if (_targetAttributes.TryGetValue(key, out AttributeTargets targets))
            {
                // Roll for offensive boost (0 to DistributionValue)
                int offenseBoost = _secureRandom.GetRandomValue(0, targets.DistributionValue + 1);

                // Calculate defensive boost (remaining points)
                int defenseBoost = targets.DistributionValue - offenseBoost;

                return (targets.TargetOffense + offenseBoost,
                       targets.TargetDefense + defenseBoost);
            }
            return (0, 0);
        }


        private Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), AttributeTargets> InitializeTargetAttributes()
        {
            var targets = new Dictionary<(PlayerTypeGenerator.PlayerType, PositionStrengthGenerator.PositionStrength), AttributeTargets>();

            #region Attaquants
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(94, 79, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(89, 76, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(85, 75, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(81, 70, 7));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(79, 68, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(75, 65, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.Sniper, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(94, 79, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(89, 76, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(85, 75, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(81, 70, 7));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(79, 68, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(75, 65, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.FabricantDeJeu, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(94, 79, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(89, 76, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(85, 75, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(81, 70, 7));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(79, 68, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(75, 65, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantOffensif, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(85, 82, 11));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(83, 80, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(80, 78, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(77, 76, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(70, 77, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(60, 75, 8));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantDePuissance, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(85, 82, 11));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(83, 81, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(81, 79, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(79, 77, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(70, 77, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(60, 75, 9));
            targets.Add((PlayerTypeGenerator.PlayerType.AttaquantPolyvalent, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(86, 80, 10));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(85, 76, 7));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.FirstLine),
                new AttributeTargets(81, 74, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.SecondLine),
                new AttributeTargets(78, 71, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.ThirdLine),
                new AttributeTargets(66, 77, 7));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.FourthLine),
                new AttributeTargets(58, 75, 8));
            targets.Add((PlayerTypeGenerator.PlayerType.JoueurDeCaractere, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));
            #endregion

            #region Defenseurs
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(91, 80, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(87, 78, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.FirstPair),
                new AttributeTargets(86, 76, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.SecondPair),
                new AttributeTargets(83, 74, 4));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.ThirdPair),
                new AttributeTargets(80, 69, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurOffensif, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(82, 86, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(78, 84, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.FirstPair),
                new AttributeTargets(75, 81, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.SecondPair),
                new AttributeTargets(68, 81, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.ThirdPair),
                new AttributeTargets(60, 78, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurDefensif, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));

            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.Generational),
                new AttributeTargets(82, 86, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.Elite),
                new AttributeTargets(78, 84, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.FirstPair),
                new AttributeTargets(75, 81, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.SecondPair),
                new AttributeTargets(68, 81, 5));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.ThirdPair),
                new AttributeTargets(60, 78, 6));
            targets.Add((PlayerTypeGenerator.PlayerType.DefenseurPhysique, PositionStrengthGenerator.PositionStrength.AHL),
                new AttributeTargets(69, 69, 4));
            #endregion

            return targets;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _secureRandom.Dispose();
                _disposed = true;
            }
        }
    }
}