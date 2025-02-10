using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StalcraftClanManager.FrontEnd.CControls
{
    /// <summary>
    /// Логика взаимодействия для PlateButton.xaml
    /// </summary>
    public partial class PlateButton : UserControl
    {
        public PlateButton()
        {
            InitializeComponent();
        }

        public bool IsActivated { get; set; }

        private Brush iColor { get; set; }
        public Brush inactiveColor 
        {
            get => iColor;
            set => iColor = value;
        }

        private Brush aColor { get; set; }
        public Brush activatedColor
        {
            get => aColor;
            set => aColor = value;
        }

        public string Text
        {
            get => lText.Content.ToString();
            set => lText.Content = value;
        }

        private void TBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsActivated)
            {
                TBackground.Background = iColor;
                IsActivated = !IsActivated;
            }
            else
            {
                TBackground.Background = aColor;
                IsActivated = !IsActivated;
            }
        }
    }
}
