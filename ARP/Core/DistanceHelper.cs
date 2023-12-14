using AltV.Net.Data;

namespace ARP.Core;

public static class DistanceHelper
{
    public static double Distance3D(Position pos1, Position pos2)
    {
        return Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2) + Math.Pow(pos1.Z - pos2.Z, 2));
    }
}