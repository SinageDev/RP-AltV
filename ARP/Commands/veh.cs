using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("veh", Arguments = [ "Имя модели", "Цвет 1", "Цвет 2" ])]
    public static async Task Cmd_Veh(EPlayer player, string hash, byte color1 = 0, byte color2 = 0)
    {
        IVehicle vehicle = await AltAsync.CreateVehicle(Alt.Hash(hash), player.Position, player.Rotation).ConfigureAwait(false);
        if (vehicle == null)
        {
            player.SendChatMessage($"Машины {hash} не найдено!");
            return;
        }

        if (vehicle is not EVehicle car) return;

        vehicle.PrimaryColor = color1;
        vehicle.SecondaryColor = color2;

        await Task.Delay(200);
        player.SetIntoVehicle(vehicle, 1);
        player.SendChatMessage($"Вы успешно заспавнили машину {car.Info?.DisplayName}");
    }
}