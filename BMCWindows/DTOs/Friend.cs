using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.DTOs
{
    public class Friend
    {
        public string UserName { get; set; }
        public DateTime LastVisit { get; set; }
        public Byte[] ProfileImage { get; set; }
        public int RequestId { get; set; }
        public BitmapImage FriendPicture { get; set; }
    }
}
