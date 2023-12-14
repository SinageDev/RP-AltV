using AltV.Net;
using AltV.Net.Async.Elements.Entities;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using ARP.Core;
using ARP.DataModels;
using ARP.Managers;

namespace ARP.Factories;

public partial class EPlayer(ICore core, IntPtr nativePointer, uint id) : AsyncPlayer(core, nativePointer, id)
{
    private const byte FaceFeaturesCount = 20;
    private const byte HeadOverlayCount = 13;

    public Account? Account { get; set; }
    public Character? Character { get; set; }
    public Admin? Admin { get; set; }
    public Helper? Helper { get; set; }

    /// <summary>
    /// Отправить игроку сообщение в чат.
    /// </summary>
    /// <param name="header">Первая строка сообщения</param>
    /// <param name="text">Текст сообщения</param>
    /// <param name="phone"></param>
    public void SendChatMessage(object header, object text, uint phone) =>
      Emit("Chat:Send", header.ToString(), text.ToString(), phone);

    public void SendChatMessage(object header, object text) =>
      Emit("Chat:Send", header.ToString(), text.ToString());

    public void SendChatMessage(object text)
    {
        Emit("Chat:Send", text.ToString());
    }

    public void SendMuteMessage()
    {
        SendChatMessage(new CustomString("color:red") + $"Ваш чат заблокирован ещё на {Character?.Mute?.Minute} минут(ы)");
    }

    public List<EPlayer> GetPlayersInRange(double range)
    {
        return GetPlayersInStream().Where(player => !(DistanceHelper.Distance3D(Position, player.Position) > range)).ToList();
    }

    public List<EPlayer> GetPlayersInStream()
    {
        return Pool.GetPlayers().Where(x => x.IsEntityInStreamingRange(this)).ToList();
    }

    /// <summary>
    /// Вызвать событие прямиком в CEF
    /// </summary>
    /// <param name="name">Имя события</param>
    /// <param name="arguments">Агрументы вызываемого события</param>
    public void EmitCef(string name, params object[] arguments) => Emit("ToCef", name, arguments);

    /// <summary>
    /// Смена маршрута внутри Vue-Router
    /// </summary>
    /// <param name="route">Имя маршрута</param>
    public void ChangeRoute(string route) => Emit("Route:Change", route);

    /// <summary>
    /// Смена маршрута внутри Vue-Router используя аргументы
    /// </summary>
    /// <param name="route">Имя маршрута</param>
    /// <param name="arguments">Аргументы вызываемого события</param>
    public void ChangeRoute(string route, params object[] arguments) =>
      Emit("Route:Change", route, arguments);

    public void ClearFaceFeatures()
    {
        for (byte i = 0; i < FaceFeaturesCount; i++) SetFaceFeature(i, 0);
    }

    public void ClearHeadOverlay()
    {
        for (byte i = 0; i < HeadOverlayCount; i++)
            RemoveHeadOverlay(i);
    }

    public void UpdateCustomization()
    {
        if (Character == null) return;
        Model = Alt.Hash(Character.Gender ? "MP_F_Freemode_01" : "MP_M_Freemode_01");

        PedCustomization customization = Character.Customization;
        uint mather = customization.HeadBlendData.Mather;
        uint father = customization.HeadBlendData.Father;
        float shapeMix = customization.HeadBlendData.Shape;
        float skinMix = customization.HeadBlendData.Skin;
        SetHeadBlendData(mather, father, 0, mather, father, 0, shapeMix, skinMix, 0.0f);

        ClearFaceFeatures();
        foreach (var (key, value) in customization.FaceFeatures)
            SetFaceFeature(key, value);

        ClearHeadOverlay();
        foreach (var (key, value) in customization.HeadOverlays)
        {
            SetHeadOverlay(key, value.Index, value.Opacity);
            SetHeadOverlayColor(key, key switch
            {
                2 or 1 or 10 => 1,
                5 or 10 => 2,
                _ => 0
            }, value.FirstColor, value.SecondColor);
        }

        SetClothes(2, customization.Hair[0], 0, 2);
        HairColor = customization.Hair[1];
        HairHighlightColor = customization.Hair[2];
        SetEyeColor(customization.EyeColor);
    }

    public new void Spawn(Position position, uint delay = 0)
    {
        base.Spawn(position, delay);
    }
}

public class EPlayerFactory : IEntityFactory<IPlayer>
{
    public IPlayer Create(ICore core, IntPtr entityPointer, uint id) => new EPlayer(core, entityPointer, id);
}