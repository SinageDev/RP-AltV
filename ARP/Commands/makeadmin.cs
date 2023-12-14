using ARP.Core;
using ARP.Core.Attributes;
using ARP.DataModels;
using ARP.Factories;
using ARP.Systems.CharacterSys;
using ARP.Utils;
using Microsoft.EntityFrameworkCore;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("makeadmin", Arguments = new[] { "ID Персонажа", "Уровень" })]
    public static async Task Cmd_MakeAdmin(EPlayer player, uint id, byte level)
    {
        using DynamicContext db = new DynamicContext();
        if (!await db.Characters.AnyAsync(character => character.Id == id).ConfigureAwait(false))
        {
            player.SendChatMessage($"Персонажа с ID {id} не найдено в нашей базе данных!");
            return;
        }

        var admin = new Admin
        {
            OwnerId = id,
            Level = level
        };
        await db.Admins.AddAsync(admin);
        await db.SaveChangesAsync();
        if (Pool.TryGetPlayerByCharacterId(id, out EPlayer target))
        {
            player.SendChatMessage($"Вы выдали административные права игроку {target.Character!.GetName()} #{id}, уровень: {level}!");
            target.SendChatMessage(
              $"Администратор {player.Character!.GetName()} #{player.Character!.Id} выдал админ права, уровень: {level}!");
            await Load.Admin(target, admin);
        }
        else
        {
            player.SendChatMessage($"Вы выдали админ права игроку #{id}, уровень: {level}!");
        }
    }
}