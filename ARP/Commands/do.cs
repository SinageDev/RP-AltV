using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("do", Arguments = new[] { "Действие" }, IsMessage = true)]
    public static Task Cmd_Do(EPlayer player, string[] messages)
    {
        foreach (var target in Server.GetPlayersInRange(player.Position, 15))
        {
            target.SendChatMessage(
              new CustomString(
                new StyleText(ChatHandler.PackMessage(messages), "font-weight:500"),
                $" ({player.Character!.GetNameForId(target.Character!.Id)}",
                new StyleText($" #{player.Character!.Id}", "opacity: 0.7"), ")"
              ).SetColor("#FF30DE").SetFontWeight(700));
        }

        return Task.CompletedTask;
    }
}