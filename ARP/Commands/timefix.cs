using AltV.Net;
using AltV.Net.Elements.Entities;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("timefix", Arguments = new[] { "Действие" })]
    public static Task Cmd_TimeFix(EPlayer player, int hour, int minute, uint weather)
    {
        var dateTimeNow = Server.GetDateTime();
        foreach (IPlayer target in Alt.GetAllPlayers())
        {
            target.SetDateTime(0, 0, 0, hour, minute, 0);
            target.SetWeather(weather);
        }

        return Task.CompletedTask;
    }
}