using ARP.Core;
using ARP.Core.Attributes;
using ARP.DataModels;
using ARP.Factories;
using ARP.Managers;
using ARP.Utils;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("mute", Arguments = new[] { "Действие" })]
    public static Task Cmd_Mute(EPlayer player, uint id, ushort minute, string[]? reason = null)
    {
        var target = Pool.GetPlayers().Select(x => x.Character).FirstOrDefault(x => x != null && x.Id == id);
        if (target == null)
        {
            player.SendChatMessage($"Игрока с ID {id} не найдено на сервере");
            return Task.CompletedTask;
        }

        if (minute == 0)
        {
            if (target.Mute == null)
            {
                player.SendChatMessage($"У игрока ID {id} нет мута");
                return Task.CompletedTask;
            }
            ChatHandler.SendChatAdminNotify(
              new CustomString(new StyleText("UNMUTE", ChatHandler.BackgroundStyle + ChatHandler.RedBackground),
                new StyleText($"Администратор {player.Character!.GetName()} #{player.Character!.Id} размутил игрока {target.Name}", "color:red"))
            );
            target.Mute = null;
            return Task.CompletedTask;
        }
        if (reason == null)
        {
            player.SendChatMessage("Укажите причину мута!");
            return Task.CompletedTask;
        }

        target.Mute = new MuteInfo
        {
            AdminId = player.Character!.Id,
            Reason = ChatHandler.PackMessage(reason),
            Date = Server.GetDateTime(),
            Minute = minute
        };

        ChatHandler.SendChatAdminNotify(
          new CustomString(new StyleText("MUTE", ChatHandler.BackgroundStyle + ChatHandler.RedBackground),
            new StyleText($"Администратор {player.Character!.GetName()} #{player.Character!.Id} замутил игрока {target.GetName()} на {minute} минут(ы)", "color:red")).SetFontWeight(500),
          new CustomString(new StyleText("Причина: ", "opacity:0.7") + target.Mute.Reason).SetColor("red"));

        using var db = new DynamicContext();
        db.Characters.Update(target);
        db.SaveChanges();
        return Task.CompletedTask;
    }
}