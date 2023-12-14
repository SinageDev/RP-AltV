using AltV.Net;
using AltV.Net.Async;
using ARP.Factories;

namespace ARP.Systems.VehicleSys;

public class EngineStateSys : IScript
{
    [AsyncClientEvent("Player:TryEngineChange")]
    public static Task OnVehicleTryEngineStateChange(EPlayer player, EVehicle vehicle)
    {
        vehicle.EngineOn = !vehicle.EngineOn;
        return Task.CompletedTask;
    }
}