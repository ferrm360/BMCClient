using BMCWindows.LobbyServer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.DTOs
{
    public class Lobby
    {
        public string LobbyId { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public int CurrentPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string Host { get; set; }
        public ObservableCollection<String> Players { get; set; }
    }
}
