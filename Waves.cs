using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using Rage.Native;

namespace CayoPericoRPH
{
    internal static class Waves
    {
        public static bool PlayerOnIslandPrevious = Settings.MainPlayer.DistanceTo2D(Settings.MapCenter) <= 1000f;
        public static bool PlayerOnIsland => Settings.MainPlayer.DistanceTo2D(Settings.MapCenter) <= 1000f;


        public static void DisableWaves()
        {
            //Game.DisplayNotification("Disabling waves");
            NativeFunction.Natives.x7E3F55ED251B76D3(1);
        }

        public static void EnableWaves()
        {
            //Game.DisplayNotification("Enabling waves");
            NativeFunction.Natives.x7E3F55ED251B76D3(0);
        }

        public static void DisableWavesInLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Sleep(5000);
                    if (Settings.DisableWaves)
                    {
                        try
                        {
                            if (PlayerOnIsland != PlayerOnIslandPrevious)
                            {
                                //Game.DisplayNotification("Detected player left or joined island");
                                if (PlayerOnIsland) DisableWaves();
                                else EnableWaves();
                            }
                            PlayerOnIslandPrevious = PlayerOnIsland;
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
