using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rage;
using Rage.Native;
using CayoPericoRPH.Doors;

[assembly: Rage.Attributes.Plugin("Cayo Perico RPH", Author = "Vincentsgm", EntryPoint = "CayoPericoRPH.EntryPoint.Main", ExitPoint = "CayoPericoRPH.EntryPoint.Main", PrefersSingleInstance = true)]

namespace CayoPericoRPH
{

	internal static class EntryPoint
	{
		private static string RageFolder = new Uri(Directory.GetCurrentDirectory()).LocalPath;
		private static string PluginsFolder = RageFolder + "/plugins/";
		private static List<string> IPLsToLoad = new List<string>();

		public static void Main()
		{
			IPLsToLoad = File.ReadAllLines(PluginsFolder + "CayoPericoIPLs.txt").ToList();
			IPLsToLoad.ForEach(i => Game.LogTrivial(i));
			LoadMPMap();
			Game.LogTrivial(Settings.DisableWaves ? "Waves are disabled" : "Waves are enabled");
			GameFiber.Sleep(5000);
			Game.DisplayNotification(@"Welcome to Cayo Perico
Made by Vincentsgm");
			//NativeFunction.CallByHash(0x9A9D1BA639675CF1, typeof(int), "HeistIsland", true);
			if (Settings.ShowIslandOnRadar) NativeFunction.CallByHash(0x5E1460624D194A38, typeof(int), true);
			if (Settings.LoadAmbientSounds) LoadSounds();
			if (Settings.UnlockMainGate) DoorUnlocking.MainGate.UnlockInLoop();
			//if (Settings.DisableWaves) Waves.DisableWaves();
			Waves.DisableWaves();
			if (Settings.EnableTrafficPaths) TrafficPaths.EnableTrafficPathsInLoop();

			Menus.MainMenu.Initialize();


			while (true)
			{
				GameFiber.Sleep(200);
				IPLsToLoad.ForEach(i => LoadIPL(i));
			}
		}

        public static void Exit(bool isTerminating)
        {
			Game.LogTrivial("Exitting CayoPericoRPH");
			IPLsToLoad.ForEach(i => UnloadIPL(i));
			NativeFunction.CallByHash(0x5E1460624D194A38, typeof(int), false);
		}

		private static void LoadMPMap() => NativeFunction.Natives.x0888C3502DBBEEF5();

		private static void LoadIPL(string iplName)
        {
			try
			{
				NativeFunction.Natives.x41B4893843BBDB74(iplName);
			}
			catch (AccessViolationException)
            {
				Game.LogTrivial($"Error while loading the following IPL: {iplName}");
            }
		}

		private static void UnloadIPL(string iplName)
		{
			try
			{
				NativeFunction.Natives.xEE6C5AD3ECE0A82D(iplName);
			}
			catch (AccessViolationException)
			{
				Game.LogTrivial($"Error while unloading the following IPL: {iplName}");
			}
		}

		private static void LoadSounds()
        {
			NativeFunction.Natives.SET_AMBIENT_ZONE_LIST_STATE("azl_dlc_hei4_island_zones", 1, 0);
			NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED("se_dlc_hei4_island_beach_party_music_new_01_left", true);
			NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED("se_dlc_hei4_island_beach_party_music_new_02_right", true);
		}
	}
}
