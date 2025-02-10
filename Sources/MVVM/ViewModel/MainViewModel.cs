using StalcraftClanManager.MVVM.Core;

namespace StalcraftClanManager.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand AddUserViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public SettingsViewModel SettingsVM { get; set; }
        public AddUserViewModel AddUserVM { get; set; }

        private object? _currentView;

        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged("CurrentView"); }
        }

        public MainViewModel() 
        {
            HomeVM = new HomeViewModel();
            SettingsVM = new SettingsViewModel();
            AddUserVM = new AddUserViewModel();

            CurrentView = new HomeViewModel();

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVM; });
            SettingsViewCommand = new RelayCommand(o => { CurrentView = SettingsVM; });
            AddUserViewCommand = new RelayCommand(o => { CurrentView = AddUserVM; });
        }
    }
}
