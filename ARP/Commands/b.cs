using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("b", Arguments = [ "Текст" ], IsMessage = true)]
    public static Task Cmd_B(EPlayer player, string[] messages)
    {
        foreach (var target in Server.GetPlayersInRange(player.Position, 15))
        {
            target.SendChatMessage(
              new CustomString("color:rgba(197,197,197,0.7)") +
              "Не ролевой " +
              new StyleText(player.Character!.GetNameForId(target.Character!.Id), "font-weight:700;color:rgba(197,197,197,1)") +
              new StyleText(" #" + player.Character!.Id + ":", "opacity: 0.7"),
              new CustomString("color:rgb(197,197,197)") + ChatHandler.PackMessage(messages));
        }

        return Task.CompletedTask;
    }
}