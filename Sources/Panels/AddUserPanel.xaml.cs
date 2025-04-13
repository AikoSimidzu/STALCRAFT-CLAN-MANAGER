using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StalcraftClanManager.Panels
{
    /// <summary>
    /// Логика взаимодействия для AddUserPanel.xaml
    /// </summary>
    public partial class AddUserPanel : UserControl
    {
        public AddUserPanel()
        {
            InitializeComponent();
        }

        private void addPlayer_Click(object sender, RoutedEventArgs e)
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
                SocialRating = 100,
                WhenCantPlay = daysList,
                SquadLeader = (bool)pSquadLeader.IsChecked
            };
            Manager.addPlayerToClan(player);
            Visibility = Visibility.Hidden;
        }
    }
}
