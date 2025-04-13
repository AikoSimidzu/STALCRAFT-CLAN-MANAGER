using System.Windows;
using System.Windows.Interop;

namespace StalcraftClanManager
{
    /// <summary>
    /// Логика взаимодействия для UserChange.xaml
    /// </summary>
    public partial class UserChange : Window
    {
        public UserChange()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            MainWindow.SendMessage(helper.Handle, 161, 2, 0);
            LoadData();
        }

        private void savePlayer_Click(object sender, RoutedEventArgs e)
        {
            List<string> daysList = new List<string>();

            if (Monday.IsActivated)
            {
                daysList.Add("Monday");
            }
            if (Tuesday.IsActivated)
            {
                daysList.Add("Tuesday");
            }
            if (Wednesday.IsActivated)
            {
                daysList.Add("Wednesday");
            }
            if (Thursday.IsActivated)
            {
                daysList.Add("Thursday");
            }
            if (Friday.IsActivated)
            {
                daysList.Add("Friday");
            }
            if (Saturday.IsActivated)
            {
                daysList.Add("Saturday");
            }
            if (Sunday.IsActivated)
            {
                daysList.Add("Sunday");
            }

            Player player = new Player
            {
                Name = pName.Text,
                Armor = pArmor.Text,
                Weapon = pWeapon.Text,
                Rifle = pRifle.Text,
                GS = int.Parse(pGS.Text),
                HaveBio = (bool)pBio.IsChecked,
                HaveSpeed = (bool)pSpeed.IsChecked,
                HaveFight = (bool)pFight.IsChecked,
                SocialRating = int.Parse(pSR.Text),
                WhenCantPlay = daysList,
                SquadLeader = (bool)pSquadLeader.IsChecked
            };
            
            Manager.changePlayerData(Manager.PlayerDataForChange, player);

            Notification notification = new Notification();
            notification.Message = "Успешно сохранено!";
            notification.Show();

            Close();
        }

        private void LoadData()
        {
            Player pl = Manager.PlayerDataForChange;

            pName.Text = pl.Name;
            pArmor.Text = pl.Armor;
            pWeapon.Text = pl.Weapon;
            pRifle.Text = pl.Rifle;
            pGS.Text = pl.GS.ToString();
            pBio.IsChecked = pl.HaveBio;
            pSpeed.IsChecked = pl.HaveSpeed;
            pFight.IsChecked = pl.HaveFight;
            pSR.Text = pl.SocialRating.ToString();
            pSquadLeader.IsChecked = pl.SquadLeader;
        }

        private void clsoe_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            MainWindow.SendMessage(helper.Handle, 161, 2, 0);
        }
    }
}
