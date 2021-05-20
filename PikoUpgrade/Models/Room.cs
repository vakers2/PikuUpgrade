using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PikoUpgrade.Models
{
    public class Room
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int NumberOfParticipants => Participators.Count;

        public string HostId { get; set; }

        public ApplicationUser Host { get; set; }

        public List<ApplicationUser> Participators { get; set; }
    }
}