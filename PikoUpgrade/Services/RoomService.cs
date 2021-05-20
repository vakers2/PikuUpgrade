using System.Collections.Generic;
using System.Linq;
using PikoUpgrade.DataAccess;
using PikoUpgrade.Models;
using PikoUpgrade.ViewModels;

namespace PikoUpgrade.Services
{
    public interface IRoomService
    {
        Room CreateRoom(string name, ApplicationUser creator);
        RoomViewModel GetRoom(string roomId);
        List<Room> GetAllRooms();
        void AddUserToRoom(string roomId, string userId);
        void RemoveUserFromRoom(string roomId, string userId);
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomRepository roomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }

        public Room CreateRoom(string name, ApplicationUser creator)
        {
            var room = new Room
            {
                Name = name,
                HostId = creator.Id
            };
            roomRepository.Create(room);
            return room;
        }

        public RoomViewModel GetRoom(string roomId)
        {
            var room = roomRepository.Get(roomId);
            return new RoomViewModel(roomId, room.Host.Email, room.Name, room.Participators.Select(x => new UserViewModel(x.Email)).ToList());
        }

        public List<Room> GetAllRooms()
        {
            return roomRepository.GetAll();
        }

        public void AddUserToRoom(string roomId, string userId)
        {
            roomRepository.AddUserToRoom(roomId, userId);
        }

        public void RemoveUserFromRoom(string roomId, string userId)
        {
            roomRepository.RemoveUserFromRoom(roomId, userId);
        }
    }
}