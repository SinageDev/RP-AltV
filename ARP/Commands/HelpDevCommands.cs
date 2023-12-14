using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{

    [Command("tppos")]
    public static void Cmd_TpPos(EPlayer player, float x, float y, float z)
    {
        if (player.IsInVehicle) player.Vehicle.SetPosition(x, y, z);
        else player.SetPosition(x, y, z);

        player.SendChatMessage($"Вы успешно телепортировались на координаты {x}, {y}, {z}");
    }

    [Command("getpos")]
    public static void Cmd_GetPos(EPlayer player, string name = "")
    {
        File.AppendAllText("Positions.txt",
          $"\nPos = [{player.Position.X}, {player.Position.Y}, {player.Position.Z}];\n\t Rot = {player.Rotation.Yaw} //{name}");
    }

    [Command("setrot")]
    public static void Cmd_SetRot(EPlayer player, float rotation)
    {
        player.Rotation = new Rotation(0, 0, rotation);
    }

    [Command("getcamera")]
    public static void Cmd_GetCamera(EPlayer player)
    {
        player.Emit("Camera:Save");
    }
}