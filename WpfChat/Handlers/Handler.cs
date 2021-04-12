using ChatServer_1.Core.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfChat.Handlers
{
    public static class Handler
    {
        public static bool Logining(ref string data)
        {
            if (data.Contains("SERVER_MESSAGE::LOGGED"))
            {
                data.Remove(0, "SERVER_MESSAGE::LOGGED".Length);
                return true;
            }
            return false;
        }
    }
}
