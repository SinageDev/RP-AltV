using AltV.Net;
using AltV.Net.Data;
using ARP.Core;
using ARP.DataModels;
using ARP.Factories;
using ARP.Managers;
using ARP.Utils;
using Jil;
using Microsoft.EntityFrameworkCore;

namespace ARP.Systems.CharacterSys;

public class Load
{
    public static async Task Character(EPlayer player, Character character)
    {
        if (player.HasData("Temp:Characters")) player.DeleteData("Temp:Characters");

        player.Dimension = 0;
        player.Spawn(new Position(340.57584f, -214.77362f, 54.21753f));
        player.Rotation = new Rotation(0, 0, 1.1873736f);
        player.Character = character;
        Alt.Log(JSON.Serialize(character.Customization));
        player.UpdateCustomization();
        player.ChangeRoute("hud");

        player.SetStreamSyncedMetaData("Player:UpdateId", character.Id);
        await InventoryManager.Load(player);
        player.SendChatMessage("Приятной игры на America Role Play!");
        Pool.AddPlayer(player);

        await using var db = new DynamicContext();
        Admin? admin = await db.Admins.FirstOrDefaultAsync(x => x.OwnerId == character.Id);
        if (admin != null) await Admin(player, admin);
    }

    public static Task Admin(EPlayer player, Admin admin)
    {
        player.Emit("AdminLoad");
        player.Admin = admin;
        return Task.CompletedTask;
    }
}