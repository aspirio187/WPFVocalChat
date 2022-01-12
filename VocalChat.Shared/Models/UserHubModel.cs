using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocalChat.Shared.Models
{
    public class UserHubModel
    {
        public string Username { get; set; }
        public string Ip { get; set; }
        public string Port { get; set; }

        public override string ToString()
        {
            return $"{Username} - {Ip}:{Port}";
        }
    }
}
