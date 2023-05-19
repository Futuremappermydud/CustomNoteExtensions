using IPA;
using IPA.Config;
using IPA.Config.Stores;
using System;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using SiraUtil.Zenject;
using CustomNoteExtensions.Installers;

namespace CustomNoteExtensions
{
	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin
	{
        internal const string HARMONY_ID = "com.FutureMapper.CustomNoteExtensions";
        internal static Harmony harmony = new Harmony(HARMONY_ID);
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }

		[Init]
		public Plugin(IPALogger logger, Zenjector zenjector)
		{
			Instance = this;
			Plugin.Log = logger;

			zenjector.Install<CustomNoteExtensionsAppInstaller>(Location.App);
			zenjector.Install<CustomNotes.Pooling.CustomNoteNoteObjectsInstaller>(Location.Player);
			zenjector.Install<CustomNoteExtensionsGameInstaller>(Location.Player);

			BS_Utils.Utilities.BSEvents.levelSelected += LevelSelected;
		}

		private void LevelSelected(LevelCollectionViewController levelCollectionView, IPreviewBeatmapLevel previewBeatmapLevel)
		{
            //Register Custom Notes here
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
