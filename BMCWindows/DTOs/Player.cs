﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BMCWindows.DTOs
{
    public class Player
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public BitmapImage ProfilePicture { get; set; }
    }
}
