namespace StalcraftClanManager
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.Json;

    internal class Manager
    {        
        private static ObservableCollection<Player> Players = new();
        private static ObservableCollection<string> WhiteList = new();

        public static ObservableCollection<Player> PlayersInClan = new();
        public static Team _Team = new Team() { Squads = new() };
        public static Player PlayerDataForChange = new Player();

        #region DB
        public static void removePlayerFromClan(Player player)
        {
            PlayersInClan.Remove(player); 
            Players.Remove(player);
            SaveDB();
        }

        public static void addPlayerToClan(Player player)
        {
            PlayersInClan.Add(player);
            SaveDB();
        }

        public static void changePlayerData(Player oldPlayerData, Player newPlayerData)
        {
            for (int i = 0; i < PlayersInClan.Count; i++)
            {
                if (PlayersInClan[i].Name == oldPlayerData.Name)
                {
                    PlayersInClan[i] = newPlayerData;
                    SaveDB();
                    break;
                }
            }
        }

        private static void SaveDB()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Players.cfg")))
            {
                File.Delete(Path.Combine(Environment.CurrentDirectory, "Players.cfg"));
            }
            foreach (Player player in PlayersInClan) 
            {
                Player temp = player;
                temp.doNotPlay = false;
                temp.haveCause = false;
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "Players.cfg"), JsonSerializer.Serialize(temp) + "\n");
            }
        }

        public static void getPlayers() 
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "Players.cfg")))
            {
                foreach (string s in File.ReadLines(Path.Combine(Environment.CurrentDirectory, "Players.cfg")))
                {
                    Player player = JsonSerializer.Deserialize<Player>(s);
                    PlayersInClan.Add(player);
                }
            }
        }

        public static void InitPlayers(ObservableCollection<Player> players, ObservableCollection<string> whiteList)
        {
            Players = players;
            WhiteList = whiteList;
        }
        #endregion

        #region Калькулятор
        public static string Calculate(bool withBio, bool withSpeed)
        {
            int squadsCount = (int)Math.Ceiling((double)(Players.Count / 5));
            for (int i = 0; i <= squadsCount; i++)
            {
                if (withBio && i == 4)
                {
                    _Team.Squads.Add(new Squad() { Name = "`БИО`", Players = new() });
                    continue;
                }
                if (withSpeed && i == 5)
                {
                    _Team.Squads.Add(new Squad() { Name = "`РАЛИК`", Players = new() });
                    continue;
                }

                _Team.Squads.Add(new Squad() { Name = $"`БОЁВКА {i + 1}`", Players = new() });
            }

            var sortedPlayers = Players.OrderByDescending(p => p.GS).ToList();
            int index = 0;

            foreach (var player in sortedPlayers)
            {
                if (WhiteList.Contains(player.Name))
                { continue; }

                if (withBio && player.HaveBio)
                {
                    if (_Team.Squads[4].Players.Count < 5)
                    {
                        _Team.Squads[4].Players.Add(player);
                        continue;
                    }
                }
                if (withSpeed && player.HaveSpeed)
                {
                    if (_Team.Squads[5].Players.Count < 5)
                    {
                        _Team.Squads[5].Players.Add(player);
                        continue;
                    }
                }

                if (index < squadsCount)
                {
                    index++;
                }
                else { index = 0; }

                if (_Team.Squads[index].Players.Count < 5)
                {
                    _Team.Squads[index].Players.Add(player);
                }
            }

            calculateSR();
            return NormalizeList();
        }

        private static void calculateSR()
        {
            ObservableCollection<Player> temp = new(); 
            string today = DateTime.Today.DayOfWeek.ToString();
            foreach (Player player in PlayersInClan)
            {
                if (!Players.Contains(player))
                {
                    Player p = player;
                    if (!player.WhenCantPlay.Contains(today) && !WhiteList.Contains(player.Name))
                    {
                        p.SocialRating -= 40;
                        temp.Add(p);
                    }
                    else { temp.Add(p); }
                }
                else 
                {
                    Player p = player;
                    if ((player.SocialRating + 15) <= 120)
                    {
                        p.SocialRating += 15;
                    }
                    temp.Add(p);
                }
            }
            PlayersInClan = temp;
            SaveDB();
        }

        private static string NormalizeList()
        {
            string result = "";

            foreach (Squad squad in _Team.Squads)
            {
                result += string.Concat(squad.Name, "\n```\n");

                foreach (Player pl in squad.Players)
                {
                    result += string.Concat(pl.Name, " | ", pl.GS, "\n");
                }
                result += "\n```\n";
            }

            return result;
        }
        #endregion
    }
}
