namespace StalcraftClanManager
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public struct Team
    {
        public ObservableCollection<Squad> Squads { get; set; }
    }

    public class Squad
    {
        public bool HaveLeader {  get; set; }
        public string Name { get; set; }
        public List<Player> Players { get; set; }
    }

    public struct DiscordData
    {
        public string WebHook { get; set; }
        public string WebHookForTable { get; set; }
    }

    public class Player : INotifyPropertyChanged
    {
        private bool _doNotPlay;
        public bool doNotPlay
        {
            get => _doNotPlay;
            set
            {
                if (_doNotPlay != value)
                {
                    _doNotPlay = value;
                    OnPropertyChanged(nameof(doNotPlay));
                }
            }
        }

        private bool _haveCause;
        public bool haveCause
        {
            get => _haveCause;
            set
            {
                if (_haveCause != value)
                {
                    _haveCause = value;
                    OnPropertyChanged(nameof(haveCause));
                }
            }
        }

        public bool SquadLeader { get; set; }
        public string Name { get; set; }
        public string Armor { get; set; }
        public string Weapon { get; set; }
        public string? Rifle { get; set; }
        public int GS { get; set; }
        public int SocialRating { get; set; }
        public bool HaveBio { get; set; }
        public bool HaveSpeed { get; set; }
        public bool HaveFight { get; set; }
        public List<string>? WhenCantPlay { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
