//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EHMAssistant.ModifyRoster
//{
//    class TeamManager
//    {
//        // Dictionary of teams with their roster of players
//        private Dictionary<Teams.Team, List<Player>> teamRosters;

//        public TeamManager()
//        {
//            teamRosters = new Dictionary<Teams.Team, List<Player>>();

//            // Initialize empty rosters for all teams
//            foreach (Teams.Team team in Enum.GetValues(typeof(Teams.Team)))
//            {
//                teamRosters[team] = new List<Player>();
//            }
//        }

//        // Add a player to a team
//        public void AddPlayerToTeam(Teams.Team team, Player player)
//        {
//            if (teamRosters.ContainsKey(team))
//            {
//                teamRosters[team].Add(player);
//            }
//        }

//        // Get all players from a team
//        public List<Player> GetTeamRoster(Teams.Team team)
//        {
//            if (teamRosters.ContainsKey(team))
//            {
//                return teamRosters[team];
//            }
//            return new List<Player>();
//        }

//        // Remove a player from a team
//        public bool RemovePlayerFromTeam(Teams.Team team, Player player)
//        {
//            if (teamRosters.ContainsKey(team))
//            {
//                return teamRosters[team].Remove(player);
//            }
//            return false;
//        }

//        // Trade players between teams
//        public void TradePlayer(Teams.Team fromTeam, Teams.Team toTeam, Player player)
//        {
//            if (RemovePlayerFromTeam(fromTeam, player))
//            {
//                AddPlayerToTeam(toTeam, player);
//            }
//        }

//        // Get team name
//        public string GetTeamName(Teams.Team team)
//        {
//            return Teams.GetEnumDescription(team);
//        }
//    }
//}
