using System.Collections.Generic;

namespace PikoUpgrade.ViewModels
{
    public class RoomViewModel
    {
        public string Id { get; }
        public string Host { get; }
        public string Name { get; }
        public int NumberOfPlayers { get; }
        public List<UserViewModel> Players { get; }

        public RoomViewModel(string id, string host, string name, int players)
        {
            Id = id;
            Host = host;
            Name = name;
            NumberOfPlayers = players;
        }

        public RoomViewModel(string id, string host, string name, List<UserViewModel> players)
        {
            Id = id;
            Host = host;
            Name = name;
            Players = players;
            NumberOfPlayers = players.Count;
        }
    }
}