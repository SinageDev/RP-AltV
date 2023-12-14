using AltV.Net;
using AltV.Net.Async;
using AltV.Net.ColoredConsole;
using AltV.Net.Data;
using ARP.DataModels;
using ARP.Factories;
using ARP.Systems.CharacterSys;
using ARP.Utils;
using Microsoft.EntityFrameworkCore;

namespace ARP.Events.ScriptEvent;

public sealed partial class ScriptEvents
{
    [AsyncScriptEvent(ScriptEventType.PlayerConnect)]
    public async Task OnPlayerConnect(EPlayer player, string reason)
    {
        Alt.LogColored(new ColoredMessage() + TextColor.Blue + $"Игрок {player.SocialClubId} пытается подключится к серверу!");

        player.Spawn(Position.Zero);

        player.SetDateTime(Server.GetDateTime());

        player.Dimension = (int)player.Id;
        var playerSocial = player.SocialClubId;
        Alt.LogColored(new ColoredMessage() + TextColor.Blue + $"Игрок {playerSocial} подключился к серверу!");

        await using var db = new DynamicContext();
        var account = await db.Accounts.FirstOrDefaultAsync(account => account.Social == playerSocial)
          .ConfigureAwait(false);

        if (account == null)
        {
            account = new Account
            {
                Social = playerSocial,
                RegDate = DateTime.UtcNow,
                RegIp = player.Ip
            };
            await db.Accounts.AddAsync(account);
            await db.SaveChangesAsync();
        }
        player.Account = account;

        if (await db.Characters.AnyAsync(x => x.OwnerId == account.Id)) await Select.Start(player, account);
        else await Creator.Start(player);
    }
}