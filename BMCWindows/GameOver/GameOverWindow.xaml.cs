using BMCWindows.GameOver.ImagePath;
using BMCWindows.LobbyServer;
using BMCWindows.Patterns.Singleton;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BMCWindows.GameOver
{
    /// <summary>
    /// Interaction logic for GameOverWindow.xaml
    /// </summary>
    public partial class GameOverWindow : Page
    {
        private LobbyDTO _lobby;
        private string _loser;
        private string _name = UserSessionManager.getInstance().GetPlayerUserData().Username;

        public string PlayerImageLeft { get; set; }
        public string PlayerTextLeft { get; set; }
        public string PlayerImageRight { get; set; }
        public string PlayerTextRight { get; set; }

        public GameOverWindow(LobbyDTO lobby, string loser)
        {
            _lobby = lobby;
            _loser = loser;
            InitializeComponent();
            this.DataContext = this;
            UpdatePlayerInfo();
        }

        private void UpdatePlayerInfo()
        {
            bool isHost = IsHost();
            bool isCurrentPlayerWinner = (_name != _loser);

            UpdatePlayerDetails(isHost, isCurrentPlayerWinner);
        }

        private bool IsHost() => _lobby.Host == _name;

        private void UpdatePlayerDetails(bool isHost, bool isWinner)
        {
            string winnerImageLeft, winnerTextLeft, winnerImageRight, winnerTextRight;

            if (isHost)
            {
                winnerImageLeft = isWinner ? ImageGameOverPath.DogWin : ImageGameOverPath.DogLose;
                winnerTextLeft = isWinner ? $"¡Has ganado {_name}!" : "¡Has perdido!";

                winnerImageRight = isWinner ? ImageGameOverPath.CatLose : ImageGameOverPath.CatWin;
                winnerTextRight = isWinner ? "¡Has perdido!" : $"¡Has ganado {_name}!";
            }
            else
            {
                winnerImageLeft = isWinner ? ImageGameOverPath.CatWin : ImageGameOverPath.CatLose;
                winnerTextLeft = isWinner ? $"¡Has ganado {_name}!" : "¡Has perdido!";

                winnerImageRight = isWinner ? ImageGameOverPath.DogLose : ImageGameOverPath.DogWin;
                winnerTextRight = isWinner ? "¡Has perdido!" : $"¡Has ganado {_name}!";
            }

            PlayerImageLeft = winnerImageLeft;
            PlayerTextLeft = winnerTextLeft;
            PlayerImageRight = winnerImageRight;
            PlayerTextRight = winnerTextRight;
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }
    }
}
