using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("s", Arguments = new[] { "Текст" }, IsMessage = true)]
    public static Task Cmd_S(EPlayer player, string[] messages)
    {
        foreach (var target in Server.GetPlayersInRange(player.Position, 25))
        {
            target.SendChatMessage(
              new CustomString("color:rgba(255,255,255,0.7)") +
              "Крикнул " +
              new StyleText(player.Character!.GetNameForId(target.Character!.Id), "font-weight:700;color:rgba(255,255,255,1)") +
              (" #" + player.Character!.Id + ":"),
              ChatHandler.PackMessage(messages));
        }

        return Task.CompletedTask;
    }
}