using AltV.Net;
using AltV.Net.Async;
using ARP.Core;
using ARP.Factories;
using ARP.Utils;
using System.Text.Json;

namespace ARP.Events.ScriptEvent;

public sealed partial class ScriptEvents
{
    [AsyncScriptEvent(ScriptEventType.PlayerDisconnect)]
    public async Task OnPlayerDisconnect(EPlayer player, string reason)
    {
        try
        {
            Pool.RemovePlayer(player);
            if (player.Character == null) return;
            Alt.Log(JsonSerializer.Serialize(player.Character));
            await using var db = new DynamicContext();

            db.Characters.Update(player.Character);
            await db.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Alt.Log(e.Message);
        }
    }
}