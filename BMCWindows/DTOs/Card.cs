using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.DTOs
{
    public class Card
    {
        public string Name { get; set; }
        public int Life { get; set; }
        public BitmapImage CardImage { get; set; }
        public bool IsClicked { get; set; }
    }
}
