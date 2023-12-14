using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;
using ARP.Utils;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("hi")]
    public static async Task Cmd_Hi(EPlayer player, uint id)
    {
        if (!Pool.TryGetPlayerById(id, out EPlayer target) || DistanceHelper.Distance3D(target.Position, player.Position) > 5)
        {
            player.SendChatMessage($"Не найдено игрока с ID ${id} по близости");
            return;
        }

        if (player.Character!.Friends.Contains(id))
        {
            player.SendChatMessage($"Вы уже знакомы с игроком ${target.Character!.GetName()}");
            return;
        }

        player.Character.Friends.Add(target.Character!.Id);
        target.Character.Friends.Add(player.Character!.Id);

        player.Emit("UpdateFriendId", target, target.Character.GetName());
        target.Emit("UpdateFriendId", player, player.Character.GetName());

        await using var db = new DynamicContext();
        db.Characters.UpdateRange(player.Character, target.Character);
        await db.SaveChangesAsync();
    }
}