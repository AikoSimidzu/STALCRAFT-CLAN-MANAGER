namespace StalcraftClanManager
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.Json;

    internal class Manager : IDisposable
    {        
        private ObservableCollection<Player> Players = new();
        private ObservableCollection<string> WhiteList = new();

        public static ObservableCollection<Player> PlayersInClan = new();
        public static Team _Team = new Team() { Squads = new() };
        public static Player PlayerDataForChange = new();

        public Manager(ObservableCollection<Player> players, ObservableCollection<string> whiteList)
        {
            Players = players;
            WhiteList = whiteList;
        }

        #region Калькулятор
        public string Calculate(bool withBio, bool withSpeed)
        {
            const int squadSize = 5;
            var squads = _Team.Squads;
            squads.Clear();

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

            var whiteListSet = new HashSet<string>(WhiteList);
            var leaders = Players
                .Where(p => p.SquadLeader && !whiteListSet.Contains(p.Name))
                .ToList();

            var regularPlayers = Players
                .Where(p => !p.SquadLeader && !whiteListSet.Contains(p.Name))
                .OrderByDescending(p => p.GS)
                .ToList();

            void ProcessPlayer(Player player, bool isLeader)
            {
                if (TryAddToSpecialSquad(player, 4, p => p.HaveBio) ||
                    TryAddToSpecialSquad(player, 5, p => p.HaveSpeed))
                    return;

                if (!player.HaveFight) return;

                var targetSquad = squads
                    .Take(squadsCount)
                    .FirstOrDefault(s => s.Players.Count < squadSize);

                if (targetSquad != null)
                {
                    if (isLeader)
                        targetSquad.Players.Insert(0, player);
                    else
                        targetSquad.Players.Add(player);
                }
            }

            leaders.ForEach(p => ProcessPlayer(p, isLeader: true));
            regularPlayers.ForEach(p => ProcessPlayer(p, isLeader: false));

            calculateSR();
            return NormalizeList();

            bool TryAddToSpecialSquad(Player p, int index, Func<Player, bool> condition)
            {
                if (index < 0 || !condition(p)) return false;

                var squad = squads[index];
                if (squad.Players.Count >= squadSize) return false;

                if (p.SquadLeader)
                    squad.Players.Insert(0, p);
                else
                    squad.Players.Add(p);

                return true;
            }
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

        private string NormalizeList()
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
        #endregion

        public void Dispose()
        {
            Players.Clear();
            WhiteList.Clear();
            _Team.Squads.Clear();
        }
    }
}
