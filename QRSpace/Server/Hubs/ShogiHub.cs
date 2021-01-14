using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using QRSpace.Server.Entities;

namespace QRSpace.Server.Hubs
{
    public class ShogiHub : Hub
    {
        private static readonly Dictionary<string, string> UserConnectionId = new();
        private static readonly Dictionary<string, string> ConnectionIdUser = new();

        private static readonly List<GameSession> GameSessions = new();

        public async Task Invite(string userName)
        {
            if (UserConnectionId.ContainsKey(userName))
            {
                await Clients.Client(UserConnectionId[userName]).SendAsync("ReceiveInvitation");
            }
        }
        
        public async Task GetConfirmation(string inviterUserName)
        {
            var name = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            var session = new GameSession(inviterUserName, name);
            GameSessions.Add(session);
            var isControl = DateTime.Now.Ticks % 2 == 0;
            await Clients.Client("1").SendAsync("GameConfirmed", isControl);
            await Clients.Client("2").SendAsync("GameConfirmed", !isControl);
        }

        public async Task SendStep((int, int) from, (int, int) to)
        {
            await Clients.Client("opponent").SendAsync("ReceiveStep", from, to);
        }

        public async Task GameOver(Guid sessionId)
        {
            var session = GameSessions.FirstOrDefault(s => s.SessionId == sessionId);
            if (session != null)
            {
                await Clients.Client(session.InviterName).SendAsync("GameOverConfirmed");
                await Clients.Client(session.ReceiverName).SendAsync("GameOverConfirmed");
                GameSessions.Remove(session);
            }
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(userName))
            {
                UserConnectionId.Add(userName, Context.ConnectionId);
                ConnectionIdUser.Add(Context.ConnectionId, userName);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var result = ConnectionIdUser.TryGetValue(Context.ConnectionId, out var name);
            if (result)
            {
                ConnectionIdUser.Remove(Context.ConnectionId);
                UserConnectionId.Remove(name);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}