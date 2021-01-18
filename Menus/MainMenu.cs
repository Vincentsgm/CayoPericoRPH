using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rage;
using RAGENativeUI;
using RAGENativeUI.Elements;

namespace CayoPericoRPH.Menus
{
    internal static class MainMenu
    {
        private static MenuPool menuPool = new MenuPool();
        private static UIMenu MainUIMenu = new UIMenu("CayoPericoRPH", "by ~g~Vincentsgm");
        private static UIMenuItem TeleportToIsland = new UIMenuItem("Teleport to Cayo Perico", "Teleport yourself to Cayo Perico's airport runway");
        private static UIMenuItem UnlockMainGates = new UIMenuItem("Unlock main gates", "Unlocks the big doors at the entrance of the mansion");
        //private static UIMenuCheckboxItem DisableWaves = new UIMenuCheckboxItem("Disable waves", Settings.DisableWaves);

        public static void Initialize()
        {
            menuPool.Add(MainUIMenu);
            TeleportToIsland.Activated += (s, e) => Teleports.TeleportToAirport.Teleport();
            UnlockMainGates.Activated += (s, e) => DoorUnlocking.MainGate.Unlock();
            //DisableWaves.CheckboxEvent += (s, c) => Settings.DisableWaves = c;

            MainUIMenu.AddItems(TeleportToIsland, UnlockMainGates /*, DisableWaves*/);

            Game.FrameRender += (s, e) => menuPool.ProcessMenus();

            UpdateMenu();
        }

        public static void UpdateMenu()
        {
            GameFiber.StartNew(delegate
            {
                while (true)
                {
                    GameFiber.Yield();
                    if (Game.IsKeyDown(Settings.MenuKey))
                    {
                        if (menuPool.IsAnyMenuOpen())
                        {
                            menuPool.CloseAllMenus();
                        }
                        else if (!UIMenu.IsAnyMenuVisible)
                        {
                            MainUIMenu.Visible = !MainUIMenu.Visible;
                        }
                        else if (UIMenu.IsAnyMenuVisible)
                        {
                            Game.DisplayNotification("[Cayo Perico RPH] A menu from an other plugin is already opened!");
                        }
                    }
                }
            });
        }
    }
}
