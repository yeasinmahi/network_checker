using System;
using System.Net;
using System.Net.Sockets;

namespace LogService
{
    public class Network
    {
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "'No IP Found'";
        }
    }
}