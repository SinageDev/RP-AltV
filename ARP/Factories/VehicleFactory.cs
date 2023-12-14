using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using AltV.Net.Elements.Entities;
using ARP.Core;
using ARP.Utils;

namespace ARP.Factories;

public enum VehicleType : byte
{
    Player,
    Admin,
    Work,
}

public sealed class EVehicle : AsyncVehicle
{
    public EVehicle(ICore core, IntPtr nativePointer, uint id) : base(core, nativePointer, id)
    {
        Info = VehicleInfo.All.GetValueOrDefault(Model);
    }

    public VehicleType OwnerType { get; }

    public VehicleInfo? Info { get; }

    private bool _engineState;
    public new bool EngineOn
    {
        get => _engineState;
        set
        {
            SetSyncedMetaData("VehicleEngineState", value);
            //Driver.Emit("vehicleEngineChangeForDriver", this, value);
            _engineState = value;
        }
    }
}

public class EVehicleFactory : IEntityFactory<IVehicle>
{
    IVehicle IEntityFactory<IVehicle>.Create(ICore core, IntPtr entityPointer, uint id)
    {
        var veh = new EVehicle(core, entityPointer, id);
        Pool.AddVehicle(veh);
        return veh;
    }
}