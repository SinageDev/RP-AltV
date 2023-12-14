using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("dl")]
    public static Task Cmd_Dl(EPlayer player)
    {
        player.Admin!.DlEnabled = !player.Admin!.DlEnabled;
        player.Emit("Dl:Enabled", player.Admin!.DlEnabled);
        return Task.CompletedTask;
    }
}