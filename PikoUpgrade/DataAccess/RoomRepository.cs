using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PikoUpgrade.Data;
using PikoUpgrade.Models;

namespace PikoUpgrade.DataAccess
{
    public interface IRoomRepository
    {
        Room Create(Room room);
        Room Update(Room room);
        Room Get(string roomId);
        List<Room> GetAll();
        void AddUserToRoom(string roomId, string userId);
        void RemoveUserFromRoom(string roomId, string userId);
    }

    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext context;

        public RoomRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Room Create(Room room)
        {
            context.Rooms.Add(room);
            context.SaveChanges();
            return room;
        }

        public Room Update(Room room)
        {
            context.Rooms.Update(room);
            context.SaveChanges();
            return room;
        }

        public Room Get(string roomId)
        {
            return context.Rooms.Include(x => x.Host).Include(x => x.Participators).SingleOrDefault(x => x.Id == roomId);
        }

        public List<Room> GetAll()
        {
            return context.Rooms.Include(x => x.Host).Include(x => x.Participators).ToList();
        }

        public void AddUserToRoom(string roomId, string userId)
        {
            var room = Get(roomId);
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user != null && !room.Participators.Contains(user))
            {
                room.Participators.Add(user);
            }
        }

        public void RemoveUserFromRoom(string roomId, string userId)
        {
            var room = Get(roomId);
            var user = context.Users.SingleOrDefault(x => x.Id == userId);
            if (user != null && room.Participators.Contains(user))
            {
                room.Participators.Remove(user);
            }
        }
    }
}