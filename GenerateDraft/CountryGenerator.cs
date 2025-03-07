using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace EHMAssistant
{
    class CountryGenerator
    {
        #region Variables
        private readonly RNGCryptoServiceProvider _rng;

        // Keep track of assigned countries
        private readonly Dictionary<Country, int> _assignedCountries;
        private int _totalAssigned;
        private readonly int _totalPlayers;
        #endregion

        #region Enum of Countries
        public enum Country
        {
            Canada,
            UnitedStates,
            Sweden,
            Russia,
            Finland,
            CzechRepublic,
            Slovakia,
            Switzerland,
            Germany,
            Denmark,
            Latvia,
            Belarus,
            Slovenia
        }
        #endregion

        #region Odds for each Country
        // Target percentages for each country
        private readonly Dictionary<Country, int> _countryTargets = new Dictionary<Country, int>
        {
            { Country.Canada, 40 },
            { Country.UnitedStates, 27 },
            { Country.Sweden, 9 },
            { Country.Russia, 7 },
            { Country.Finland, 4 },
            { Country.CzechRepublic, 4 },
            { Country.Slovakia, 2 },
            { Country.Switzerland, 2 },
            { Country.Germany, 1 },
            { Country.Denmark, 1 },
            { Country.Latvia, 1 },
            { Country.Belarus, 1 },
            { Country.Slovenia, 1 }
        };
        #endregion


        #region Constructor
        public CountryGenerator(int totalPlayers = 60)
        {
            _rng = new RNGCryptoServiceProvider();
            _totalPlayers = totalPlayers;
            _totalAssigned = 0;

            // Initialize assigned countries counter
            _assignedCountries = new Dictionary<Country, int>();
            foreach (Country country in Enum.GetValues(typeof(Country)))
            {
                _assignedCountries[country] = 0;
            }
        }
        #endregion

        #region Roll Country
        public Country RollCountry()
        {
            // Calculate target numbers for each country
            Dictionary<Country, int> targetNumbers = new Dictionary<Country, int>();
            foreach (var kvp in _countryTargets)
            {
                targetNumbers[kvp.Key] = (int)Math.Round((_totalPlayers * kvp.Value) / 100.0);
            }

            // Create list of available countries based on current distribution
            List<Country> availableCountries = new List<Country>();
            foreach (var kvp in targetNumbers)
            {
                // If we haven't reached the target number for this country, add it to available countries
                if (_assignedCountries[kvp.Key] < kvp.Value)
                {
                    // Add country with weighted probability based on how far we are from target
                    int deficit = kvp.Value - _assignedCountries[kvp.Key];
                    for (int i = 0; i < deficit; i++)
                    {
                        availableCountries.Add(kvp.Key);
                    }
                }
            }

            // If no countries are available based on targets, fall back to original distribution
            if (!availableCountries.Any())
            {
                availableCountries = _countryTargets.Keys.ToList();
            }

            // Select random country from available countries
            Country selectedCountry = availableCountries[GetSecureRandomInt(0, availableCountries.Count)];

            // Update counters
            _assignedCountries[selectedCountry]++;
            _totalAssigned++;

            return selectedCountry;
        }

        private int GetSecureRandomInt(int minValue, int maxValue)
        {
            if (minValue >= maxValue)
                throw new ArgumentException("minValue must be less than maxValue");

            byte[] randomBytes = new byte[4];
            _rng.GetBytes(randomBytes);
            int value = BitConverter.ToInt32(randomBytes, 0);

            // We need to be careful with modulo bias here
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

        #region Print Distribution
        public void PrintDistribution()
        {
            Console.WriteLine("\nCurrent Country Distribution:");
            foreach (var kvp in _assignedCountries)
            {
                double percentage = (_totalAssigned > 0) ?
                    (kvp.Value * 100.0 / _totalAssigned) : 0;
                Console.WriteLine($"{kvp.Key}: {kvp.Value} players ({percentage:F1}%)");
            }
        }
        #endregion
    }
}