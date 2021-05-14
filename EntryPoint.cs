using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rage;
using Rage.Native;
using CayoPericoRPH.Doors;

[assembly: Rage.Attributes.Plugin("Cayo Perico RPH", Author = "Vincentsgm", EntryPoint = "CayoPericoRPH.EntryPoint.Main", ExitPoint = "CayoPericoRPH.EntryPoint.Main", PrefersSingleInstance = true, ShouldTickInPauseMenu = true)]

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
			WaitForMPMap();
			LoadScenarioGroups(true);
			EnableZone();
			Game.DisplayNotification(@"Welcome to Cayo Perico
Made by Vincentsgm");

			//NativeFunction.CallByHash(0x9A9D1BA639675CF1, typeof(int), "HeistIsland", true);

			Game.LogTrivial($"ShowIslandOnRadar: {Settings.ShowIslandOnRadar}");
			if (Settings.ShowIslandOnRadar)
			{
				Game.FrameRender += (s, e) => EnableMinimap();
			}

			Game.LogTrivial($"LoadAmbientSounds: {Settings.LoadAmbientSounds}");
			if (Settings.LoadAmbientSounds) LoadSounds();

			Game.LogTrivial($"UnlockMainGate: {Settings.UnlockMainGate}");
			if (Settings.UnlockMainGate) DoorUnlocking.MainGate.UnlockInLoop();

			Game.LogTrivial($"UnlockMainGate: {Settings.DisableWaves}");
			if (Settings.DisableWaves) Waves.DisableWavesInLoop();

			Game.LogTrivial($"EnableTrafficPath: {Settings.EnableTrafficPaths}");
			if (Settings.EnableTrafficPaths) TrafficPaths.EnableTrafficPathsInLoop();

			Game.LogTrivial("Initialising menu");
			Menus.MainMenu.Initialize();

			Game.LogTrivial("Menu initialision ended");

			IPLsToLoad.ForEach(i => LoadIPL(i));
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
				Game.LogTrivial($"Unloading the following IPL: {iplName}");
				NativeFunction.Natives.xEE6C5AD3ECE0A82D(iplName);
			}
			catch (AccessViolationException)
			{
				Game.LogTrivial($"Error while unloading the following IPL: {iplName}");
			}
		}

		private static bool IsIPLActive(string iplName) => NativeFunction.Natives.x88A741E44A2B3495<bool>(iplName);

		/*private static void EnableInteriorProp(int interiorID, string prop) => NativeFunction.Natives.x55E86AF2712B36A1(interiorID, prop);
		private static void DisableInteriorProp(int interiorID, string prop) => NativeFunction.Natives.x420BD37289EEE162(interiorID, prop);
		private static void ChangeInteriorElementColor(int interiorID, string prop, int color) => NativeFunction.CallByHash(0xC1F1920BAF281317, typeof(int), interiorID, prop, color);
		private static void RefreshInterior(int interiorID) => NativeFunction.Natives.x41F37C3427C75AE0(interiorID);*/

		private static void WaitForMPMap()
        	{
			while (!IsIPLActive("h4_islandairstrip"))
        		{
				GameFiber.Sleep(1000);
				LoadIPL("h4_islandairstrip");
            		}
        	}

		private static void LoadScenarioGroups(bool enable)
        	{
			NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds", enable);
			NativeFunction.Natives.SET_SCENARIO_GROUP_ENABLED("Heist_Island_Peds_2", enable);
		}

		private static void EnableZone()
        	{
			int zone = NativeFunction.Natives.GET_ZONE_FROM_NAME_ID<int>("IsHeist");
			NativeFunction.Natives.SET_ZONE_ENABLED(zone, 1);
		}

		private static void EnableMinimap()
        	{
			if (IsPlayerOutsideInterior)
        		{
				NativeFunction.Natives.SET_RADAR_AS_INTERIOR_THIS_FRAME(Game.GetHashKey("h4_fake_islandx")/*0xc0a90510*/, 4700.0f, -5145.0f, 0, 0);
				NativeFunction.Natives.SET_RADAR_AS_EXTERIOR_THIS_FRAME();
			}
        	}	

		private static bool IsPlayerOutsideInterior => PlayerInterior == 0;

		private static int PlayerInterior => NativeFunction.Natives.GET_INTERIOR_FROM_ENTITY<int>(Settings.MainPlayer);

		private static void LoadSounds()
        	{
			NativeFunction.Natives.SET_AMBIENT_ZONE_LIST_STATE("azl_dlc_hei4_island_zones", 1, 0);
			NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED("se_dlc_hei4_island_beach_party_music_new_01_left", true);
			NativeFunction.Natives.SET_STATIC_EMITTER_ENABLED("se_dlc_hei4_island_beach_party_music_new_02_right", true);
		}
	}
}
