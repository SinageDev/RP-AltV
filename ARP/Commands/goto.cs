using ARP.Core;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("goto", Arguments = [ "ID Персонажа" ])]
    public static Task Cmd_Goto(EPlayer player, uint id)
    {
        if (player.Character!.Id == id)
        {
            player.SendChatMessage("Вы не моежете телепортироваться к самому себе");
            return Task.CompletedTask;
        }

        var target = Pool.GetPlayers().FirstOrDefault(x => x.Character != null && x.Character.Id == id);
        if (target is null)
        {
            player.SendChatMessage($"Игрока с ID {id} не найдено на сервере!");
            return Task.CompletedTask;
        }

        player.Position = target.Position;
        player.Dimension = target.Dimension;

        player.SendChatMessage($"Вы успешно телепортировались к игроку {target.Character!.GetName()}[{id}]");
        return Task.CompletedTask;
    }
}