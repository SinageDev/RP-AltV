using System.ComponentModel.DataAnnotations;

namespace ARP.DataModels
{
    public class Account
    {
        [Key] public uint Id { get; set; }

        public ulong Social { get; set; }

        public string RegIp { get; set; } = null!;
        public DateTime RegDate { get; set; }
    }
}