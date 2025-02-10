namespace StalcraftClanManager
{
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification()
        {
            InitializeComponent();
        }

        public string Message { get => message.Content.ToString(); set => message.Content = value; }

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
