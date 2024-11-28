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
    public class PlayerLoader
    {
        private readonly ProfileServer.ProfileServiceClient _profileProxy;

        public PlayerLoader()
        {
            _profileProxy = new ProfileServer.ProfileServiceClient();
        }

        public ObservableCollection<Player> LoadPlayers(IEnumerable<string> usernames, string currentUsername)
        {
            var players = new ObservableCollection<Player>();
            foreach (var username in usernames)
            {
                if (username == currentUsername) continue;
                var playerImage = _profileProxy.GetProfileImage(username);
                if (playerImage?.ImageData?.Length > 0)
                {
                    var image = ConvertByteArrayToImage(playerImage.ImageData);
                    players.Add(new Player { Username = username, ProfilePicture = image });
                }
            }
            return players;
        }

        public ObservableCollection<Player> LoadSinglePlayer(string username)
        {
            var players = new ObservableCollection<Player>();
            var playerImage = _profileProxy.GetProfileImage(username);
            if (playerImage?.ImageData?.Length > 0)
            {
                var image = ConvertByteArrayToImage(playerImage.ImageData);
                players.Add(new Player { Username = username, ProfilePicture = image });
            }
            return players;
        }

        private BitmapImage ConvertByteArrayToImage(byte[] imageData)
        {
            using (var stream = new MemoryStream(imageData))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}
