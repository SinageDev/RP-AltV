using AltV.Net.Data;
using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("slap", Arguments = ["ID персонажа"])]
    public static Task Cmd_Slap(EPlayer player, uint id)
    {
        if (!Pool.TryGetPlayerByCharacterId(id, out EPlayer target))
        {
            player.SendChatMessage($"Игрока {id} не найдено на сервере");
            return Task.CompletedTask;
        }

        target.Position += new Position(0, 0, 3);

        player.SendChatMessage($"Вы подкинули игрока {target.Character!.GetName()} #{target.Character!.Id}");
        target.SendChatMessage($"Администратор {player.Character!.GetName()} #{player.Character!.Id} пнул вас");

        return Task.CompletedTask;
    }
}