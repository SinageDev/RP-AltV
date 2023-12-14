using AltV.Net;
using AltV.Net.Async;
using ARP.Core;
using ARP.Factories;

namespace ARP.Events.ScriptEvent;

public sealed partial class ScriptEvents : IScript
{
    [AsyncScriptEvent(ScriptEventType.VehicleRemove)]
    public Task OnVehicleRemove(EVehicle vehicle)
    {
        Pool.RemoveVehicle(vehicle);

        return Task.CompletedTask;
    }
}