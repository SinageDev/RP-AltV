using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using System.Text;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("ahelp")]
    public static Task Cmd_AdminCommands(EPlayer player)
    {
        if (player.Admin == null) return Task.CompletedTask;

        StringBuilder commandsString = new("");
        List<string> commandList = ChatHandler.GetAdminCommands(player.Admin.Level);
        foreach (string command in commandList) commandsString.Append($"/{command} ");
        player.SendChatMessage("Команды администратора: ", commandsString.ToString());
        return Task.CompletedTask;
    }
}