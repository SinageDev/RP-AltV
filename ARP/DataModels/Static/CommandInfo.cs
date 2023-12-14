using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ARP.DataModels.Static
{
    public enum CommandAccessType : byte
    {
        Player = 0,
        Helper = 1,
        Admin = 2,
    }

    public class CommandInfo
    {
        [Key]
        public uint Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = "TODO";
        public CommandAccessType Access { get; set; } = CommandAccessType.Player;
        public int AccessLevel { get; set; } = 0;
        public string[] Aliases { get; set; } = [];

        [NotMapped]
        public bool IsMessage { get; set; }
        [NotMapped]
        public MethodInfo Method { get; set; }
        [NotMapped]
        public string[]? Arguments { get; set; }
    }
}
