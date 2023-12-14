using AltV.Net;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("gethere", Arguments = [ "ID Персонажа" ])]
    public static Task Cmd_GetHere(EPlayer player, uint id)
    {
        if (player.Character!.Id == id)
        {
            player.SendChatMessage("Вы не моежете телепортировать самого себя");
            return Task.CompletedTask;
        }

        if (Alt.GetPlayerById(id) is not EPlayer target)
        {
            player.SendChatMessage($"Игрока с ID {id} не найдено на сервере!");
            return Task.CompletedTask;
        }

        if (player.IsInVehicle)
        {
            player.Vehicle.Dimension = player.Dimension;
            player.Vehicle.Position = player.Position;
        }
        else
        {
            target.Position = player.Position;
            target.Dimension = player.Dimension;
        }

        player.SendChatMessage($"Вы успешно телепортировали к себе игрока {target.Character!.Name}[{id}]");
        target.SendChatMessage($"Администратор {player.Character!.GetName()}[{player.Character!.Id}] телепортировал вас к себе!");

        return Task.CompletedTask;
    }
}