using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using ARP.Factories;

namespace ARP.Events.ScriptEvent;

public sealed partial class ScriptEvents
{
    [AsyncScriptEvent(ScriptEventType.PlayerDead)]
    public Task OnPlayerDead(EPlayer player, IEntity killer, uint weapon)
    {
        player.Spawn(new Position(340.57584f, -214.77362f, 54.21753f));
        player.Rotation = new Rotation(0, 0, 1.1873736f);
        return Task.CompletedTask;
    }
}