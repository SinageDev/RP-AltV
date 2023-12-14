using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using ARP.DataModels;
using ARP.Factories;
using ARP.Utils;
using Jil;
using Microsoft.EntityFrameworkCore;

namespace ARP.Systems.CharacterSys;

public class Creator : IScript
{
    [AsyncClientEvent("Creator:Start")]
    public static Task Start(EPlayer player)
    {
        if (player.Account is null || player.Character is not null) return Task.CompletedTask;

        player.ChangeRoute("creator");
        player.Position = new Position(-815.61755f, -599.03735f, 30.274048f);
        player.Rotation = new Rotation(0, 0, -2.0943951024f);
        return Task.CompletedTask;
    }

    [AsyncClientEvent("Creator:CheckName")]
    public static async Task OnCharNameExist(EPlayer player, string name)
    {
        if (player.Account is null || player.Character is not null) return;
        await using var db = new DynamicContext();
        player.EmitCef("Creator:CheckResult",
          await db.Characters.AnyAsync(character => character.Name == name).ConfigureAwait(false));
    }

    [AsyncClientEvent("Creator:CreateChar")]
    public static async Task PlayerTryCreateChar(EPlayer player, string name, bool gender, string customization)
    {
        if (player.Account is null || player.Character is not null) return;
        if (!name.Contains('_') || name.Length is < 5 or > 32) return;
        await using var db = new DynamicContext();
        if (await db.Characters.AnyAsync(character => character.Name == name).ConfigureAwait(false))
        {
            player.EmitCef("Creator:CheckResult", false);
            return;
        }

        PedCustomization custom = JSON.Deserialize<PedCustomization>(customization);
        if (custom == null) return;
        Character character = new()
        {
            Name = name,
            OwnerId = player.Account.Id,
            Gender = gender,
            Customization = custom,
            RegIp = player.Ip,
            RegDate = DateTime.UtcNow
        };
        await db.Characters.AddAsync(character);
        await db.SaveChangesAsync();
        await Load.Character(player, character);
    }
}