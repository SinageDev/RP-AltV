using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using ARP.DataModels;
using ARP.Factories;
using ARP.Utils;
using Jil;
using Microsoft.EntityFrameworkCore;

namespace ARP.Systems.CharacterSys;

public class Select : IScript
{
    public static async Task Start(EPlayer player, Account account)
    {
        if (player.GetData<List<Character>>("Temp:Characters", out var temp))
        {
            player.ChangeRoute("select", 2, JSON.Serialize(temp, Options.ISO8601));
            return;
        }

        await using var db = new DynamicContext();
        List<Character> characters = await db.Characters.AsNoTracking().Where(character => character.OwnerId == account.Id)
          .ToListAsync()
          .ConfigureAwait(false);

        player.Position = new Position(-75.283516f, -818.8483f, 326.17358f);
        player.SetData("Temp:Characters", characters);
        player.ChangeRoute("select", 2, JSON.Serialize(characters, Options.ISO8601));
    }

    [AsyncClientEvent("Select:Enter")]
    public static async Task OnEnterCharacter(EPlayer player, byte select)
    {
        if (player.Account is null || player.Character is not null) return;
        if (!player.GetData<List<Character>>("Temp:Characters", out var characters)) return;
        if (characters.Count < select) return;

        await Load.Character(player, characters[select]);
    }
}