using BMCWindows.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.GameplayBoard
{
    public class PlayerManager
    {
        private ProfileServer.ProfileServiceClient _profileProxy;

        public PlayerManager()
        {
            _profileProxy = new ProfileServer.ProfileServiceClient();
        }

        public ObservableCollection<Player> LoadPlayers(ObservableCollection<string> playerNames, string currentUsername)
        {
            var players = new ObservableCollection<Player>();
            foreach (var playerName in playerNames)
            {
                if (playerName != currentUsername)
                {
                    var profile = _profileProxy.GetProfileImage(playerName);
                    if (profile.ImageData?.Length > 0)
                    {
                        var image = ConvertToBitmapImage(profile.ImageData);
                        players.Add(new Player { Username = playerName, ProfilePicture = image });
                    }
                }
            }
            return players;
        }

        private BitmapImage ConvertToBitmapImage(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                return image;
            }
        }
    }
}
