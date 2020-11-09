using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace QRSpace.Server.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string user, string message)
        {
            
        }
    }
}