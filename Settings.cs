using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CayoPericoRPH.Doors;
using Rage;

namespace CayoPericoRPH
{
    internal static class Settings
    {
        public static Ped MainPlayer => Game.LocalPlayer.Character;

        private static InitializationFile ini = new InitializationFile("plugins/CayoPericoRPH.ini");
        public static bool ShowIslandOnRadar = ini.ReadBoolean("Settings", "ShowIslandOnRadar", false);
        public static bool LoadAmbientSounds = ini.ReadBoolean("Settings", "LoadAmbientSounds", true);
        public static bool UnlockMainGate = ini.ReadBoolean("Settings", "UnlockMainGate", true);
        public static bool DisableWaves = ini.ReadBoolean("Settings", "DisableWaves", true);
        public static bool EnableTrafficPaths = ini.ReadBoolean("Settings", "EnableTrafficPaths", true);

        public static Keys MenuKey = ini.ReadEnum("Keybindings", "OpenMenu", Keys.F5);

        public static Vector3 MapCenter = new Vector3(5031.428f, -5150.907f, 0f);

        public static Vector3 MapLocation = new Vector3(4494.252f, -4510.028f, 4.190997f);
        public static float MapLocationHeading = 323.2874f;

        public static Door MainDoorNorthRight = new Door(0xa22c5a6a, new Vector3(4981.0122f, -5712.747f, 20.7810f));
        public static Door MainDoorNorthLeft = new Door(0x4872b7e6, new Vector3(4984.1388f, -5709.248f, 20.7810f));
        public static Door MainDoorSouthRight = new Door(0xa22c5a6a, new Vector3(4990.6812f, -5715.106f, 20.7810f));
        public static Door MainDoorSouthLeft = new Door(0x4872b7e6, new Vector3(4987.5874f, -5718.634f, 20.7810f));
        public static List<Door> MansionMainDoors = new List<Door>
        {
            MainDoorNorthLeft,
            MainDoorNorthRight,
            MainDoorSouthLeft,
            MainDoorSouthRight
        };
    }
}
