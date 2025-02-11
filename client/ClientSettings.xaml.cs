using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace client
{
    /// <summary>
    /// Interaction logic for ClientSettings.xaml
    /// </summary>
    public partial class ClientSettings : UserControl
    {
        public ClientSettings()
        {
            InitializeComponent();
        }

        public delegate void SettingsClosedHandler();
        public event SettingsClosedHandler? SettingsClosed;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetControlPositions();
        }

        private void SetControlPositions()
        {
            // Position ServerTitle and ServerCotitle relative to ExitSettingsIcon
            PositionTitleAndSubtitle();

            // Position the RemoteIpLabel and RemoteIpTextbox
            PositionRemoteIpControls();

            // Position the RemotePortLabel and RemotePortTextbox
            PositionRemotePortControls();

            // Position the RemoteUsernameLabel and RemoteUsernameTextBox
            PositionRemoteUsernameControls();

            // Position the RemotePasswdLabel and RemotePasswordTextbox
            PositionRemotePasswordControls();

            // Position the Logo relative to other controls
            PositionLogo();

            PositionFirstSeparator();
        }

        private void PositionFirstSeparator()
        {
            FirstSeparator.Y1 = 0;
            FirstSeparator.Y2 = 0;

            FirstSeparator.X1 = Canvas.GetLeft(ServerTitle);
            FirstSeparator.X2 = Canvas.GetLeft(RemotePasswordTextbox) + RemotePasswordTextbox.Width;

           
            Canvas.SetTop(FirstSeparator, Canvas.GetTop(RemoteUsernameLabel) + 100);

        }

        private void PositionTitleAndSubtitle()
        {
            Canvas.SetLeft(ServerTitle, Canvas.GetLeft(ExitSettingsIcon) + 10);
            Canvas.SetLeft(ServerCotitle, Canvas.GetLeft(ServerTitle));
            Canvas.SetTop(ServerTitle, Canvas.GetTop(ExitSettingsIcon) + 40);
            Canvas.SetTop(ServerCotitle, Canvas.GetTop(ServerTitle) + 30);
        }

        private void PositionRemoteIpControls()
        {
            Canvas.SetTop(RemoteIpLabel, Canvas.GetTop(ServerTitle) + (ServerTitle.FontSize / 2) - (RemoteIpLabel.FontSize / 2));
            Canvas.SetLeft(RemoteIpLabel, Canvas.GetLeft(ServerTitle) + 300);
            Canvas.SetTop(RemoteIpTextbox, Canvas.GetTop(RemoteIpLabel) + 30);
            Canvas.SetLeft(RemoteIpTextbox, Canvas.GetLeft(RemoteIpLabel));
        }

        private void PositionRemotePortControls()
        {
            Canvas.SetLeft(RemotePortLabel, Canvas.GetLeft(RemoteIpLabel) + 200);
            Canvas.SetTop(RemotePortLabel, Canvas.GetTop(RemoteIpLabel));
            Canvas.SetLeft(RemotePortTextbox, Canvas.GetLeft(RemotePortLabel));
            Canvas.SetTop(RemotePortTextbox, Canvas.GetTop(RemotePortLabel) + 30);
        }

        private void PositionRemoteUsernameControls()
        {
            Canvas.SetTop(RemoteUsernameLabel, Canvas.GetTop(RemoteIpTextbox) + 50);
            Canvas.SetLeft(RemoteUsernameLabel, Canvas.GetLeft(RemoteIpTextbox));
            Canvas.SetTop(RemoteUsernameTextBox, Canvas.GetTop(RemoteUsernameLabel) + 30);
            Canvas.SetLeft(RemoteUsernameTextBox, Canvas.GetLeft(RemoteUsernameLabel));
        }

        private void PositionRemotePasswordControls()
        {
            Canvas.SetLeft(RemotePasswdLabel, Canvas.GetLeft(RemotePortLabel));
            Canvas.SetTop(RemotePasswdLabel, Canvas.GetTop(RemoteUsernameLabel));
            Canvas.SetLeft(RemotePasswordTextbox, Canvas.GetLeft(RemotePasswdLabel));
            Canvas.SetTop(RemotePasswordTextbox, Canvas.GetTop(RemotePasswdLabel) + 30);
        }

        private void PositionLogo()
        {
            Canvas.SetTop(Logo, Canvas.GetTop(RemoteIpLabel) + 30);
            Canvas.SetLeft(Logo, Canvas.GetLeft(RemotePortTextbox) + 220);
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }

        private void ExitSettingsIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SettingsClosed?.Invoke();
        }
    }
}
