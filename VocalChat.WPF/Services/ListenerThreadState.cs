using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VocalChat.WPF.Codecs;

namespace VocalChat.WPF.Services
{
    internal class ListenerThreadState
    {
        public IPEndPoint EndPoint { get; set; } = default!;
        public INetworkChatCodec Codec { get; set; }
    }
}
