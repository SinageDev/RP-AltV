using AltV.Net;
using AltV.Net.Elements.Entities;
using ARP.Factories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARP.Core
{
    public static class Pool
    {
        private static ConcurrentDictionary<uint, EPlayer> Players { get; } = [];
        private static ConcurrentDictionary<uint, EVehicle> Vehicles { get; } = [];

        // Other pools for small groups
        private static ConcurrentDictionary<uint, EPlayer> Admins { get; } = [];

        #region Vehicles

        private static ConcurrentDictionary<uint, EVehicle> PersonalVehicles { get; } = [];

        #endregion

        #region Interaction with players

        public static void AddPlayer(EPlayer player)
        {
            Players.TryAdd(player.Character!.Id, player);
        }

        public static ICollection<EPlayer> GetPlayers()
        {
            return Players.Values;
        }

        public static EPlayer GetPlayerByCharacterId(uint id)
        {
            return Players[id];
        }

        public static bool TryGetPlayerByCharacterId(uint id, out EPlayer? player)
        {
            return Players.TryGetValue(id, out player);
        }

        public static EPlayer GetPlayerById(uint id)
        {
            return (EPlayer)Alt.GetPlayerById(id);
        }

        public static bool TryGetPlayerById(uint id, out EPlayer player)
        {
            player = GetPlayerById(id);
            return player != null;
        }

        public static void RemovePlayer(EPlayer player)
        {
            Players.Remove(player.Character!.Id, out _);
        }

        #endregion

        #region Interaction with vehicles

        public static void AddVehicle(EVehicle vehicle)
        {
            Vehicles.TryAdd(vehicle.Id, vehicle);
        }

        public static ICollection<EVehicle> GetVehicles()
        {
            return Vehicles.Values;
        }

        public static EVehicle GetVehicleByUniqueId(uint id)
        {
            return Vehicles[id];
        }

        public static bool TryGetPlayerByUniqueId(uint id, out EVehicle? vehicle)
        {
            return Vehicles.TryGetValue(id, out vehicle);
        }

        public static EVehicle GetVehicleById(uint id)
        {
            return (EVehicle)Alt.GetVehicleById(id);
        }

        public static bool TryGetVehicleById(uint id, out EVehicle vehicle)
        {
            vehicle = GetVehicleById(id);
            return vehicle != null;
        }

        public static void RemoveVehicle(EVehicle vehicle)
        {
            Vehicles.Remove(vehicle.Id, out _);
        }

        #endregion

        public static ICollection<EPlayer> GetAdmins()
        {
            return Admins.Values;
        }
    }
}
