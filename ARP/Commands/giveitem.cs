using AltV.Net;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("giveitem", Arguments = new[] { "ID Предмета", "Кол-во" })]
    public static async Task Cmd_GiveItem(EPlayer player, ushort id, ushort count = 0)
    {
        //Alt.Log($"{id}, ${count}");
        //var result = player.Character!.Inventory.GiveItem(id, count);
        //Alt.Log(result.ToString());
        //if (!result) return;
    }
}