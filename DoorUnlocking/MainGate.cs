using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;

namespace CayoPericoRPH.DoorUnlocking
{
    internal static class MainGate
    {
        public static void Unlock()
        {
            Settings.MansionMainDoors.ForEach(d => d.IsLocked = false);
        }

        public static void UnlockInLoop()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Sleep(1000);
                    Unlock();
                }
            });
        }
    }
}
