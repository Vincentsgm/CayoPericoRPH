using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Native;

namespace CayoPericoRPH
{
    internal static class Minimap
    {
        public static void DisableIslandRadar()
        {
            NativeFunction.CallByHash(0x5E1460624D194A38, typeof(int), false);
        }

        public static void EnableIslandRadar()
        {
            NativeFunction.CallByHash(0x5E1460624D194A38, typeof(int), true);
        }

        public static void EnableIslandRadarInLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Sleep(5000);
                    if (Settings.EnableTrafficPaths)
                    {
                        try
                        {
                            if (Settings.MainPlayer.DistanceTo2D(Settings.MapCenter) <= 2000f) EnableIslandRadar();
                            else DisableIslandRadar();
                        }
                        catch (Exception ex)
                        {
                            Game.LogTrivial(ex.ToString());
                        }
                    }
                }
            });
        }
    }
}
