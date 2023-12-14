using AltV.Net;
using AltV.Net.Async;
using AltV.Net.ColoredConsole;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using ARP.Core;
using ARP.DataModels;
using ARP.Factories;
using ARP.Managers;
using ARP.Utils;
using System.Collections.Concurrent;
using ARP.DataModels.Static;

namespace ARP
{
    public class Server : AsyncResource
    {
        public static Region Locale { get; } = Region.Ru;

        public override void OnStart()
        {
            using DynamicContext db = new();
            using StaticContext staticContext = new();
            if (!db.Database.CanConnect())
            {
                Alt.LogError("Неуспешное подключение к базе данных!");
                return;
            }

            Alt.LogInfo("Успешное подключение к базе данных!");

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();
            staticContext.Database.EnsureDeleted();
            staticContext.Database.EnsureCreated();

            Alt.LogColored(new ColoredMessage() + TextColor.Yellow + $"Зарегистрировано аккаунтов: {db.Accounts.Count()}");
            Alt.LogColored(new ColoredMessage() + TextColor.Yellow + $"Зарегистрировано персонажей: {db.Characters.Count()}");
            Alt.LogColored(new ColoredMessage() + TextColor.Yellow + $"Зарегистрировано администраторов: {db.Admins.Count()}");

            ChatHandler.InitCommand();

            TimeManager.StartTimer();
        }

        public override void OnStop()
        {
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory() => new EPlayerFactory();
        public override IEntityFactory<IVehicle> GetVehicleFactory() => new EVehicleFactory();

        public static DateTime GetDateTime()
        {
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
        }

        public static List<EPlayer> GetPlayersInRange(Position position, float range)
        {
            return Pool.GetPlayers().Where(x => DistanceHelper.Distance3D(x.Position, position) < range).ToList();
        }
    }
}