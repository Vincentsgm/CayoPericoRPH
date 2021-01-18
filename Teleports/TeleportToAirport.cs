using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CayoPericoRPH.Teleports
{
    internal static class TeleportToAirport
    {
        public static void Teleport()
        {
            if (Settings.MainPlayer.IsInAnyVehicle(false))
            {
                Settings.MainPlayer.CurrentVehicle.Position = Settings.MapLocation;
                Settings.MainPlayer.CurrentVehicle.Heading = Settings.MapLocationHeading;
            }
            else
            {
                Settings.MainPlayer.Position = Settings.MapLocation;
            }
        }
    }
}
