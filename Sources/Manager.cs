namespace StalcraftClanManager
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Unicode;

    internal class Manager : IDisposable
    {        
        private ObservableCollection<Player> Players = new();
        private ObservableCollection<string> WhiteList = new();

        public static ObservableCollection<Player> PlayersInClan = new();
        public static Player PlayerDataForChange = new();

        public Manager(ObservableCollection<Player> players, ObservableCollection<string> whiteList)
        {
            Players = players;
            WhiteList = whiteList;
        }

        #region Калькулятор
        public string Calculate(bool withBio, bool withSpeed)
        {
            Team team = new()
            { Squads = new() };
            int squadsCount = (int)Math.Ceiling((double)(Players.Count / 5));

            for (int i = 0; i <= squadsCount; i++)
            {
                if (withBio && i == (squadsCount - 1))
                {
                    team.Squads.Add(new Squad() { Name = "`БИО`", Players = new() });
                    continue;
                }

                if (withSpeed && i == squadsCount)
                {
                    team.Squads.Add(new Squad() { Name = "`РАЛИК`", Players = new() });
                    continue;
                }

                team.Squads.Add(new Squad() { Name = $"`БОЁВКА {i + 1}`", Players = new() });
            }

            var leaders = Players.Where(p => p.SquadLeader && !WhiteList.Contains(p.Name)).ToList();
            var regularPlayers = Players.Where(p => !p.SquadLeader && !WhiteList.Contains(p.Name)).OrderByDescending(p => p.GS).ToList();

            AddToSquad(leaders);
            AddToSquad(regularPlayers);

            void AddToSquad(List<Player> players)
            {
                int index = 0;

                foreach (Player player in players)
                {
                    if (withBio && player.HaveBio)
                    {
                        if (team.Squads[squadsCount - 1].Players.Count < 5)
                        {
                            team.Squads[squadsCount - 1].Players.Add(player);
                            continue;
                        }
                    }
                    if (withSpeed && player.HaveSpeed)
                    {
                        if (team.Squads[squadsCount].Players.Count < 5)
                        {
                            team.Squads[squadsCount].Players.Add(player);
                            continue;
                        }
                    }

                    if (index < squadsCount)
                    {
                        index++;
                    }
                    else { index = 0; }

                    if (team.Squads[index].Players.Count < 5)
                    {
                        team.Squads[index].Players.Add(player);
                    }
                }
            }

            calculateSR();
            return NormalizeList(team);
        }

        private void calculateSR()
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

        private string NormalizeList(Team team)
        {
            StringBuilder result = new StringBuilder();

            foreach (Squad squad in team.Squads)
            {
                result.Append(string.Concat(squad.Name, "\n```\n"));

                foreach (Player pl in squad.Players)
                {
                    result.Append(string.Concat(pl.Name, " | ", pl.GS, "\n"));
                }
                result.Append("\n```\n");
            }

            return result.ToString();
        }
        #endregion

        #region DB
        public static void removePlayerFromClan(Player player)
        {
            PlayersInClan.Remove(player);
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
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Player player in PlayersInClan)
            {
                Player temp = player;
                temp.doNotPlay = false;
                temp.haveCause = false;
                stringBuilder.Append(JsonSerializer.Serialize(temp, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = false }) + "\n");
            }
            File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "Players.cfg"), stringBuilder.ToString());
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
        #endregion

        public void Dispose()
        {
            Players.Clear();
            WhiteList.Clear();
        }
    }
}
