using AltV.Net;
using ARP.Core;
using ARP.Factories;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ARP.Managers;

public class TimeManager : IScript
{
    private static Timer? _minuteTimer;

    public static async Task StartTimer()
    {
        if (Server.GetDateTime().Second != 0) await Task.Delay((60 - Server.GetDateTime().Second) * 1000);
        _minuteTimer = new Timer();
        _minuteTimer.Interval = 60000;
        _minuteTimer.AutoReset = true;
        _minuteTimer.Elapsed += MinuteTimerEvent;
        _minuteTimer.Start();
    }

    private static async void MinuteTimerEvent(object? source, ElapsedEventArgs e)
    {
        if (Server.GetDateTime().Minute == 0) await PayDay().ConfigureAwait(false);
        foreach (var player in Pool.GetPlayers())
        {
            var mute = player.Character!.Mute;
            if (mute == null) continue;
            mute.Minute--;
            if (mute.Minute != 0) continue;
            player.Character.Mute = null;
            player.SendChatMessage("С вас было снято наказание. Вы снова можете пользоваться чатом.");
        }
    }

    private static async Task PayDay()
    {
        foreach (EPlayer player in Pool.GetPlayers())
        {
            player.SetDateTime(Server.GetDateTime());
        }
        Alt.Log("Уху пейдей!");
    }
}