using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("setclothes", Arguments = new[] { "Действие" })]
    public static Task Cmd_SetClothes(EPlayer player, byte componentId, ushort drawable, byte texture)
    {
        player.SetClothes(componentId, drawable, texture, 2);

        return Task.CompletedTask;
    }
}