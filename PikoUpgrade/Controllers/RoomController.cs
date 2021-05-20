using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PikoUpgrade.Services;
using PikoUpgrade.ViewModels;

namespace PikoUpgrade.Controllers
{
    [Authorize]
    [ApiController]
    public class RoomController : Controller
    {
        private readonly IRoomService roomService;

        public RoomController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [Route("rooms")]
        public IActionResult Index()
        {
            return Ok(roomService.GetAllRooms().Select(x => new RoomViewModel(x.Id, x.Host.Email, x.Name, x.NumberOfParticipants)));
        }

        [Route("room/get/{roomId}")]
        public IActionResult GetData(string roomId)
        {
            return Ok(roomService.GetRoom(roomId));
        }
    }
}
