using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace websocket
{
    public class WebSocketConnection
    {
        ClientWebSocket _socket = null;
        private async Task<string> Receive()
        {
            byte[] buffer = new byte[1024];
            string response = string.Empty;

            while (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationToken(false));

                if (result.MessageType == WebSocketMessageType.Close)
                    await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                else
                {
                    response = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

                }
            }
            return response;
        }

        public async Task Connect(string url)
        {
            Thread.Sleep(1000);

            try
            {
                _socket = new ClientWebSocket();
                await _socket.ConnectAsync(new Uri(url), CancellationToken.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void OnConnect(string url)
        {
            Task.Run(() => Connect(url)).Wait();
        }

        public void Close()
        {
            _socket?.Dispose();
            Console.WriteLine("Websocket closed");
        }

        private async Task Send(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            await _socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text,
                true, CancellationToken.None);
            Console.WriteLine("Sent: {0} ", message);
            await Task.Delay(1000);
        }

        public async Task<string> OnMessageReceive()
        {
            return await Task.Run(()=>Receive().Result);
        }

        public async void OnMessageSend(string message)
        {
            await Task.Run(() => Send(message));
        }
    }
}