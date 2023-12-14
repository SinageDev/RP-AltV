using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("bench")]
    public static Task Cmd_Bench(EPlayer player)
    {
        player.Dimension = (int)player.Id;
        player.ChangeRoute("bench");
        return Task.CompletedTask;
    }
}