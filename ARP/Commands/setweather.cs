using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("setweather", Arguments = new[] { "Действие" })]
    public static Task Cmd_SetWeather(EPlayer player, string[] messages)
    {

        return Task.CompletedTask;
    }
}