using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("me", Arguments = new[] { "Действие" }, IsMessage = true)]
    public static Task Cmd_Me(EPlayer player, string[] messages)
    {
        foreach (var target in Server.GetPlayersInRange(player.Position, 15))
        {
            target.SendChatMessage(
              new CustomString(
                player.Character!.GetNameForId(target.Character!.Id),
                new StyleText($" #{player.Character!.Id} ", "opacity:0.7"),
                new StyleText(ChatHandler.PackMessage(messages), "font-weight:500")).SetColor("#FF30DE").SetFontWeight(700)
            );
        }

        return Task.CompletedTask;
    }
}