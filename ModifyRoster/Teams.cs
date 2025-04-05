using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EHMAssistant.ModifyRoster
{
    public class Team
    {
        public string Name { get; }
        public int Number { get; }
        public bool IsNHL { get; }
        public Team AHLTeam { get; set; }

        public Team(string name, int number, bool isNHL)
        {
            Name = name;
            Number = number;
            IsNHL = isNHL;
        }
    }

    public static class Teams
    {
        public static readonly Dictionary<int, Team> TeamList = new Dictionary<int, Team>();

        static Teams()
        {
            // Define NHL teams
            var nhlTeams = new List<Team>
            {
                new Team("Anaheim Ducks", 1, true),
                new Team("Boston Bruins", 3, true),
                new Team("Buffalo Sabres", 4, true),
                new Team("Carolina Hurricanes", 6, true),
                new Team("Chicago Blackhawks", 7, true),
                new Team("Colorado Avalanche", 8, true),
                new Team("Columbus Blue Jackets", 9, true),
                new Team("Dallas Stars", 10, true),
                new Team("Detroit Red Wings", 11, true),
                new Team("Edmonton Oilers", 12, true),
                new Team("Florida Panthers", 13, true),
                new Team("Los Angeles Kings", 14, true),
                new Team("Minnesota Wild", 15, true),
                new Team("Montreal Canadiens", 16, true),
                new Team("Nashville Predators", 19, true),
                new Team("New Jersey Devils", 20, true),
                new Team("New York Islanders", 17, true),
                new Team("New York Rangers", 18, true),
                new Team("Ottawa Senators", 21, true),
                new Team("Philadelphia Flyers", 22, true),
                new Team("San Jose Sharks", 25, true),
                new Team("Seattle Kraken", 5, true),
                new Team("St. Louis Blues", 26, true),
                new Team("Tampa Bay Lightning", 27, true),
                new Team("Toronto Maple Leafs", 28, true),
                new Team("Utah HC", 23, true),
                new Team("Vancouver Canucks", 29, true),
                new Team("Vegas Golden Knights", 24, true),
                new Team("Washington Capitals", 30, true),
                new Team("Winnipeg Jets", 2, true),
            };

            // Define AHL teams
            foreach (var nhlTeam in nhlTeams)
            {
                int ahlNumber = nhlTeam.Number + 30;
                var ahlTeam = new Team($"{nhlTeam.Name} AHL", ahlNumber, false);
                nhlTeam.AHLTeam = ahlTeam;
                TeamList[nhlTeam.Number] = nhlTeam;
                TeamList[ahlNumber] = ahlTeam;
            }
        }

        public static Team GetTeamByNumber(int number) => TeamList.TryGetValue(number, out var team) ? team : null;
    }
}
