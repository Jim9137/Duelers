using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duelers.Common;
using Duelers.Server.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Duelers.Server
{
    public class GameServer : MonoBehaviour
    {
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private readonly string _deckId = "8";
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Queue<string> _messagesReceived = new Queue<string>();
        private readonly Queue<string> _messagesToBeSent = new Queue<string>();
        private readonly Uri gameAddress = new Uri("ws://duelers.olegmaslennikov.com/api/mechazorg/v1/game/");
        private ArraySegment<byte> _buffer = new ArraySegment<byte>(new byte[2048]);
        private string _userToken;

        public void AddMessageToQueue(string message) => _messagesToBeSent.Enqueue(message);
        public string PopMessageFromQueue() => _messagesReceived.Count > 0 ? _messagesReceived.Dequeue() : "";

        private void Awake()
        {
            Connect();
        }

        private void OnDestroy()
        {
            Debug.Log("Closing connection");

            Task.Run(CloseConnection, _cancellationToken)
                .Wait(_cancellationToken);

            Debug.Log($"Client should be closed {_clientWebSocket.State}");
        }

        private async Task CloseConnection() =>
            await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client destroyed",
                _cancellationToken);

        private async void Connect()
        {
            try
            {
                await _clientWebSocket.ConnectAsync(gameAddress, _cancellationToken);
                if (_clientWebSocket.State == WebSocketState.Open) Debug.Log("connected");
                var content = await Signin();

                StartGame(content);
                ReceiveMessages();
            }
            catch (Exception e)
            {
                Debug.Log("woe " + e.Message);
            }
        }


        private async Task<string> Signin()
        {
            var signinType = new SigninMessage("Birb9136", "913789", "PLAIN");

            var h = await _httpClient.PostAsync(
                " http://duelers.olegmaslennikov.com/api/authentication/v1/signin/ ",
                new ByteArrayContent(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(signinType))));
            return await h.Content.ReadAsStringAsync();
        }


        private async void StartGame(string content)
        {
            var response = JsonConvert.DeserializeObject<SigninResponse>(content);
            _userToken = response.token;
            var startGame = new StartGameMessage("START_GAME", response.token, _deckId);
            var b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(startGame)));
            await _clientWebSocket.SendAsync(b, WebSocketMessageType.Text, true, _cancellationToken);
        }

        private async void ReceiveMessages()
        {
            var str = new StringBuilder();
            while (true)
            {
                var buffer = new ArraySegment<byte>(new byte[2048]);
                if (_clientWebSocket.State != WebSocketState.Open) break;

                var r = await _clientWebSocket.ReceiveAsync(buffer, _cancellationToken);

                if (buffer.Array != null) str.Append(Encoding.UTF8.GetString(buffer.Array));

                if (!r.EndOfMessage)
                {
                    Debug.Log($"Continuing message, thus far: {str}");
                    continue;
                }

                _messagesReceived.Enqueue(str.ToString());
                Debug.Log("Got: " + str);
                str.Clear();
            }
        }

        public async Task SendActions(string[] actions)
        {
            if (_clientWebSocket.State != WebSocketState.Open) return;

            foreach (var a in actions)
            {
                var messages = JsonConvert.DeserializeObject<UserChoicesMessage>(a);
                messages.Token = _userToken;
                var b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messages)));
                await _clientWebSocket.SendAsync(b, WebSocketMessageType.Text, true, _cancellationToken);
            }
        }
    }
}