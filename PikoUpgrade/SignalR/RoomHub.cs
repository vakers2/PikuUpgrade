using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using PikoUpgrade.Models;
using PikoUpgrade.Services;

namespace PikoUpgrade.SignalR
{
    public class RoomHub : Hub
    {
        private readonly IRoomService roomService;
        private readonly UserManager<ApplicationUser> userManager;

        public RoomHub(IRoomService roomService, UserManager<ApplicationUser> userManager)
        {
            this.roomService = roomService;
            this.userManager = userManager;
        }

        public async Task Enter(string roomId, string email)
        {
            roomService.AddUserToRoom(roomId, userManager.FindByEmailAsync(email).GetAwaiter().GetResult().Id);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Group(roomId).SendAsync("Notify", $"{email} вошел в чат");
        }

        public async Task Leave(string roomId, string email)
        {
            roomService.RemoveUserFromRoom(roomId, userManager.FindByEmailAsync(email).GetAwaiter().GetResult().Id);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendFirstVideoUrl(string roomId, string url)
        {
            await Clients.Group(roomId).SendAsync("ReceiveFirst", url);
        }

        public async Task SendSecondVideoUrl(string roomId, string url)
        {
            await Clients.Group(roomId).SendAsync("ReceiveSecond", url);
        }
    }
} 