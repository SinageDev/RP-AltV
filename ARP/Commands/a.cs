using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("a", Arguments = [ "Сообщение" ])]
    public static Task Cmd_A(EPlayer player, string[] messages)
    {
        ChatHandler.SendAdminMessage(new CustomString(
            new StyleText($"Администратор [{player.Admin!.Level}] ", "opacity:0.7"),
            new StyleText($"{player.Character!.GetName()} ", "font-weight:700"),
            new StyleText($"#{player.Character.Id}:", "opacity:0.7")
            ).SetColor(219, 255, 0),
          new CustomString(ChatHandler.PackMessage(messages)).SetColor(219, 255, 0));

        return Task.CompletedTask;
    }
}