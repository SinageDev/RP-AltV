using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("hp", Arguments = [ "Кол-во" ])]
    public static Task Cmd_Hp(EPlayer player, ushort count)
    {
        player.Health += count;

        player.SendChatMessage($"Выдано {count} HP");

        return Task.CompletedTask;
    }
}