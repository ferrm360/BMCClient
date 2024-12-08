using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BMCWindows.DTOs
{
    public class Message
    {

        public string Sender { get; set; }

        public string Messages { get; set; }

        public DateTime TimeSent { get; set; }
        public HorizontalAlignment Alignment { get; set; }
    }
}
