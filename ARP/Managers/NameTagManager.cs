using AltV.Net;
using AltV.Net.Async;
using ARP.Core;
using ARP.Factories;

namespace ARP.Managers;

public class NameTagManager : IScript
{
    [AsyncClientEvent("getPlayerNameTag")]
    public Task OnPlayerTryGetNameTag(EPlayer player, uint id)
    {
        if (player.Character == null) return Task.CompletedTask;
        EPlayer target = Pool.GetPlayerByCharacterId(id);
        if (target == null) return Task.CompletedTask;
        player.Emit("playerSetNametag", target, target.Character!.GetNameForId(player.Character.Id));
        return Task.CompletedTask;
    }
}