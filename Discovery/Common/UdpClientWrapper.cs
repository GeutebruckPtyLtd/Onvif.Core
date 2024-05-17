﻿using Onvif.Core.Discovery.Interfaces;

using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Onvif.Core.Discovery.Common
{
    /// <summary>
    /// A simple <see cref="System.Net.Sockets.UdpClient"/> wrapper
    /// </summary>
    public class UdpClientWrapper : IUdpClient
    {
        private readonly UdpClient client;
        private bool _disposedValue;

        public UdpClientWrapper()
        {
            client = new UdpClient
            {
                EnableBroadcast = true
            };
        }

        public UdpClientWrapper(string ipAddress, int port)
        {
            client = new UdpClient(new IPEndPoint(IPAddress.Parse(ipAddress), port))
            {
                EnableBroadcast = true
            };
        }

        public async Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint endPoint)
        {
            return await client.SendAsync(datagram, bytes, endPoint).ConfigureAwait(false);
        }

        public async Task<UdpReceiveResult> ReceiveAsync()
        {
            return await client.ReceiveAsync().ConfigureAwait(false);
        }

        public void Close()
        {
            client.Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    client?.Dispose();
                }
                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
