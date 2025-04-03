using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StalcraftClanManager.Panels
{
    /// <summary>
    /// Логика взаимодействия для HomePanel.xaml
    /// </summary>
    public partial class HomePanel : UserControl
    {
        public HomePanel()
        {
            InitializeComponent();
        }

        private static DiscordData dsData = Discord.LoadData();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            users.ItemsSource = Manager.PlayersInClan;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer SV = sender as ScrollViewer;
            if (e.Delta > 0)
            {
                SV.LineUp();
            }
            else
            {
                SV.LineDown();
            }
            e.Handled = true;
        }

        private void removeFromClan_Click(object sender, RoutedEventArgs e)
        {
            foreach (Player player in Manager.PlayersInClan)
            {
                if (((Player)users.SelectedItem).Name == player.Name)
                {
                    Manager.removePlayerFromClan(player);
                    break;
                }
            }
        }

        private void userEdit_Click(object sender, RoutedEventArgs e)
        {
            UserChange userChange = new UserChange();
            Manager.PlayerDataForChange = (Player)users.SelectedItem;
            userChange.Show();
        }

        private void sendToDiscord_Click(object sender, RoutedEventArgs e)
        {
            if (users.Items.Count > 5)
            {
                ObservableCollection<string> white = new();
                ObservableCollection<Player> players = new();

                foreach (var items in (users.ItemsSource as IEnumerable<Player>))
                {
                    if (items.haveCause)
                    {
                        white.Add(items.Name);
                    }
                    if (!items.doNotPlay)
                    {
                        Player player = items;
                        players.Add(player);
                    }
                }

                using (Manager manager = new Manager(players, white))
                {
                    Discord.Send(dsData.WebHook, manager.Calculate((bool)withBio.IsChecked, (bool)withSpeed.IsChecked));
                    if (dsData.WebHookForTable.Length > 0)
                    {
                        Discord.SendFile(dsData.WebHookForTable);
                    }
                }

                Notification notification = new Notification();
                notification.Message = "Отряды успешно сформированы \nи отправлены в дискорд!";
                notification.Show();
            }
        }

        private void formate_Click(object sender, RoutedEventArgs e)
        {
            if (users.Items.Count > 5)
            {
                ObservableCollection<string> white = new();
                ObservableCollection<Player> players = new();

                foreach (var items in (users.ItemsSource as IEnumerable<Player>))
                {
                    if (items.haveCause)
                    {
                        white.Add(items.Name);
                        continue;
                    }
                    if (!items.doNotPlay)
                    {
                        Player player = items;
                        players.Add(player);
                    }
                }

                using (Manager manager = new Manager(players, white))
                {
                    Clipboard.SetText(manager.Calculate((bool)withBio.IsChecked, (bool)withSpeed.IsChecked));
                }

                Notification notification = new Notification();
                notification.Message = "Отряды успешно сформированы \nи скопированы в буффер!";
                notification.Show();
            }
        }

        private void sendConfig_Click(object sender, RoutedEventArgs e)
        {
            if (dsData.WebHookForTable.Length > 0)
            {
                Discord.SendFile(dsData.WebHookForTable);
            }
            Notification notification = new Notification();
            notification.Message = "Конфиг отправлен в дискорд!";
            notification.Show();
        }
    }
}
