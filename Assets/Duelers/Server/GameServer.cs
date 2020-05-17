﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Duelers.Common;
using Duelers.Server.Model;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

namespace Duelers.Server
{
    public class GameServer : MonoBehaviour
    {
        private readonly CancellationToken _cancellationToken = new CancellationToken();
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly Queue<string> _messagesReceived = new Queue<string>();
        private readonly Queue<string> _messagesToBeSent = new Queue<string>();
        private static Uri baseUri = new Uri("https://mechaz.org/");
        private static Uri gameAddress = new Uri("wss://mechaz.org/api/mechazorg/v1/game/");
        private static Uri resolveAddress = new Uri("https://mechaz.org/api/mechazorg/v1/resolve-target/");
        private readonly WebSocketSharp.WebSocket _clientWebSocket = new WebSocketSharp.WebSocket(gameAddress.ToString());
        private ArraySegment<byte> _buffer = new ArraySegment<byte>(new byte[2048]);
        [SerializeField] private string _deckId;

        [SerializeField] private string _password;

        // These are temporary. Deckid you can see on the main website when you go and click your deck
        [SerializeField] private string _userName;
        private string _userToken;
        public string Token { get => _userToken; }

        private void OnDestroy()
        {
            Debug.Log("Closing connection");

            CloseConnection();

            Debug.Log($"Client should be closed");
        }

        private void CloseConnection() =>
            _clientWebSocket.CloseAsync();
        
        

        public async void Connect(Func<string, bool> callback)
        {
            try
            {
                _clientWebSocket.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                _clientWebSocket.OnMessage += (sender, e) =>
                {
                    if (e.IsText)
                    {
                        callback(e.Data);
                    }
                };
                _clientWebSocket.OnOpen += (sender, e) =>
                {
                    Debug.Log("connected");
                };
                _clientWebSocket.Connect();
                var content = await Signin();
                if (content.Contains("error"))
                {
                    throw new ArgumentException(content);
                }

                StartGame(content);
                
            }
            catch (Exception e)
            {
                Debug.Log("woe " + e.Message);
                _clientWebSocket.CloseAsync();
            }
        }

        private async Task<string> Signin()
        {
            var signinType = new SigninMessage(_userName, _password, "PLAIN");

            var h = await _httpClient.PostAsync(
                baseUri + "/api/authentication/v1/signin/ ",
                new ByteArrayContent(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(signinType))));
            return await h.Content.ReadAsStringAsync();
        }

        private void StartGame(string content)
        {
            var response = JsonConvert.DeserializeObject<SigninResponse>(content);
            _userToken = response.token;
            var startGame = new StartGameMessage("START_GAME", response.token, _deckId);
            _clientWebSocket.SendAsync(JsonConvert.SerializeObject(startGame), b1 => { });
        }

        public void SendActions(string[] actions)
        {

            foreach (var a in actions)
            {
                Debug.Log("Sending message: " + a, this);
                var type = JsonConvert.DeserializeObject<TypeMessage>(a);
                switch (type.Type)
                {
                    case MessageType.NONE:
                        break;
                    case MessageType.TILE:
                        break;
                    case MessageType.CHARACTER:
                        break;
                    case MessageType.CHOICE:
                        var r = JsonConvert.DeserializeObject<UserChoicesMessage>(a);
                        r.Token = _userToken;
                        SendMessage(r);
                        break;
                    case MessageType.RESOLVE_CHOICE:


                        break;
                    case MessageType.CARD:
                        break;
                    case MessageType.PLAY:
                        var p = JsonConvert.DeserializeObject<PlayMessage>(a);
                        p.Token = _userToken;
                        SendMessage(p);

                        break;
                }
            }
        }

        private void SendMessage(object message)
        {
            _clientWebSocket.SendAsync(JsonConvert.SerializeObject(message), completed => { });
        }

        public async Task<string> GetTargets(ResolveTargetRequest resolveTargets)
        {
            if (resolveTargets == null)
                throw new ArgumentNullException(nameof(resolveTargets));

            resolveTargets.Token = _userToken;
            var content = new StringContent(JsonConvert.SerializeObject(resolveTargets), Encoding.UTF8, "application/json");
            var res = await _httpClient.PostAsync(resolveAddress, content, _cancellationToken).ConfigureAwait(false);
            return await res.Content.ReadAsStringAsync();
        }

        public async Task<string> GetJson(string path)
        {
            var res = await _httpClient.GetAsync(baseUri + path, _cancellationToken).ConfigureAwait(false);
            return await res.Content.ReadAsStringAsync();
        }
    }
}