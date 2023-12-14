using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("gotoveh", Arguments = [ "ID Персонажа" ])]
    public static Task Cmd_Goto(EPlayer player, ushort id)
    {
        if (!Pool.TryGetVehicleById(id, out EVehicle target))
        {
            player.SendChatMessage($"Автомобиля ID {id} не найдено на сервере!");
            return Task.CompletedTask;
        }

        target.Position = player.Position;
        target.Dimension = player.Dimension;

        player.SendChatMessage($"Вы успешно телепортировали автомобиль {target.Info?.DisplayName} [{id}]");
        return Task.CompletedTask;
    }
}