using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Native;

namespace CayoPericoRPH
{
    internal static class TrafficPaths
    {
        public static void DisableTrafficPaths()
        {
            NativeFunction.Natives.xF74B1FFA4A15FBEA(1);
        }

        public static void EnableTrafficPaths()
        {
            NativeFunction.Natives.xF74B1FFA4A15FBEA(0);
        }

        public static void EnableTrafficPathsInLoop()
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
                            if (Settings.MainPlayer.DistanceTo2D(Settings.MapCenter) <= 1000f) DisableTrafficPaths();
                            else EnableTrafficPaths();
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
