﻿namespace StalcraftClanManager.Panels
{
    using System.IO;
    using System.Text.Json;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;

    /// <summary>
    /// Логика взаимодействия для SettingsPanel.xaml
    /// </summary>
    public partial class SettingsPanel : UserControl
    {
        public SettingsPanel()
        {
            InitializeComponent();
        }

        private void disSave_Click(object sender, RoutedEventArgs e)
        {
            DiscordData ds = new DiscordData { WebHook = webHook.Text, WebHookForTable = webHookFT.Text };
            File.WriteAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "Discord.cfg"), JsonSerializer.Serialize(ds));

            Notification notification = new Notification();
            notification.Message = "Успешно сохранено!";
            notification.Show();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DiscordData dsData = Discord.LoadData();
            webHook.Text = dsData.WebHook;
            webHookFT.Text = dsData.WebHookForTable;
        }
    }
}
