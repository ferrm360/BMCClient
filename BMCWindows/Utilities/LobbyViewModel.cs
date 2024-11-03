using BMCWindows.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Utilities
{
    public class LobbyViewModel : INotifyPropertyChanged
    {
        private string _currentPlayer; // Nombre del jugador actual
        public ObservableCollection<Player> AllPlayers { get; set; } = new ObservableCollection<Player>();

        public ObservableCollection<Player> FilteredPlayers
        {
            get
            {
                // Filtrar los jugadores para excluir al jugador actual
                return new ObservableCollection<Player>(
                    AllPlayers.Where(player => player.Username != _currentPlayer));
            }
        }

        public string CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(FilteredPlayers)); // Notificar que los jugadores filtrados han cambiado
            }
        }

        // Implementación del INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
