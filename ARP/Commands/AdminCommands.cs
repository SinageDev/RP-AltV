using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using ARP.Core.Attributes;
using ARP.Factories;

namespace ARP.Commands;

public sealed partial class PlayerCommands
{
    [Command("setmodel")]
    public static void Cmd_SetModel(EPlayer player, string model)
    {
        player.Model = Alt.Hash(model);
    }

    [Command("tp")]
    public static void Cmd_Tp(EPlayer player, long accountId)
    {

    }

    [Command("cloth")]
    public static void Cmd_Cloth(EPlayer player, byte body, ushort drawable, byte texture)
    {
        player.SetClothes(body, drawable, texture, 0);
    }

    [Command("dlccloth")]
    public static void Cmd_DlcCloth(EPlayer player, string dlc, byte body, ushort drawable, byte texture)
    {
        player.SetDlcClothes(body, drawable, texture, 0, Alt.Hash(dlc));
    }


}