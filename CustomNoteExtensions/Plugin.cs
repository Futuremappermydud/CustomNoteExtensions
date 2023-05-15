﻿using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using Newtonsoft.Json;
using CustomNoteExtensions.CustomNotes;
using CustomNoteExtensions.API.Events;
using System.IO;
using SiraUtil.Zenject;
using CustomNoteExtensions.Installers;
using UnityEngine;

namespace CustomNoteExtensions
{
	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin
	{
        internal const string HARMONY_ID = "com.FutureMapper.CustomNoteTypes";
        internal static Harmony harmony = new Harmony(HARMONY_ID);
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }

		[Init]
		public Plugin(IPALogger logger, Zenjector zenjector)
		{
			Instance = this;
			Plugin.Log = logger;

			var test = new CustomJSONNote();
			test.noteEvent = new ModifyHealthEvent();
			test.name = "TestNote";
            test.color = Color.black;

			var json = JsonConvert.SerializeObject(test, new JsonSerializerSettings()
			{
				TypeNameHandling = TypeNameHandling.Auto,
				Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
			});
			File.WriteAllText(Path.Combine(IPA.Utilities.UnityGame.InstallPath, "UserData/CustomNoteTypes/test.json"), json);

			zenjector.Install<CustomNoteTypesAppInstaller>(Location.App);
		}

        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
        }

		[OnEnable]
		public void OnEnable()
		{
			ApplyHarmonyPatches();
		}

		[OnDisable]
		public void OnDisable()
		{
            RemoveHarmonyPatches();
		}
        internal static void ApplyHarmonyPatches()
        {
            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error applying Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            try
            {
                harmony.UnpatchSelf();
            }
            catch (Exception ex)
            {
                Plugin.Log?.Error("Error removing Harmony patches: " + ex.Message);
                Plugin.Log?.Debug(ex);
            }
        }
	}
}