using AltV.Net;
using ARP.DataModels;
using ARP.Factories;
using Jil;

namespace ARP.Managers;

public class InventoryManager : IScript
{
    public static async Task Load(EPlayer player)
    {
        //player.EmitCef("inventoryItems", JSON.Serialize(Inventory.Items));
        //player.EmitCef("setInventory", JSON.Serialize(player.Character!.Inventory, Options.ExcludeNulls));
        //player.Character.Inventory.MainSlots.TargetPlayer = player;
    }
}