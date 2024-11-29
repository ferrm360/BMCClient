using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.DTOs
{
    public class AttackCard
    {
        public string Name { get; set; }
        public int AttackLevel { get; set; }
        public BitmapImage CardImage { get; set; }
        public bool IsClicked { get; set; }

    }
}
