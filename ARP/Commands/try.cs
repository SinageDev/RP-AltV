using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Managers;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    private const string ChatTryTrue =
      "background: radial-gradient(59.46% 59.46% at 50% 15.54%, rgba(255, 255, 255, 0.2) 0%, rgba(255, 255, 255, 0) 100%), linear-gradient(203.88deg, #1C832C 15.34%, #167620 36.08%, #177B1B 68.89%, #1B881F 84.96%);";

    [Command("try", Arguments = new[] { "Действие" }, IsMessage = true)]
    public static Task Cmd_Try(EPlayer player, string[] messages)
    {
        var message = ChatHandler.PackMessage(messages);
        var random = new Random();
        var tryResult = random.Next(2) == 1;
        foreach (var target in Server.GetPlayersInRange(player.Position, 15))
        {
            target.SendChatMessage(
              new CustomString(
                new StyleText(tryResult ? "УДАЧНО" : "НЕУДАЧНО",
                  ChatHandler.BackgroundStyle + (tryResult ? ChatTryTrue : ChatHandler.RedBackground) + "color:#fff;"),
                player.Character!.GetNameForId(target.Character!.Id),
                new StyleText($" #{player.Character!.Id}", "opacity:0.7"),
                new StyleText(" " + message, "font-weight:500")).SetColor("#FF30DE").SetFontWeight(700)
            );
        }

        return Task.CompletedTask;
    }
}