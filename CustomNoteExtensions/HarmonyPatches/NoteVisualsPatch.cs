using CustomNoteExtensions.CustomNotes.Pooling;
using HarmonyLib;
using IPA.Loader;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.HarmonyPatches
{
	[HarmonyPatch]
	internal class ChromaPatch
	{
		private static bool Prepare()
		{
			return ChromaPatch.Chroma != null;
		}

		private static MethodBase TargetMethod()
		{
			NoteColorizerType = ChromaPatch.Chroma.Assembly.GetType("Chroma.Colorizer.NoteColorizerManager");
			return (ChromaPatch.Chroma == null) ? null : NoteColorizerType.GetMethod("GetColorizer", BindingFlags.Instance | BindingFlags.Public);
		}

		private static bool Prefix(ref NoteControllerBase noteController, object __instance)
		{
			if(noteController is CustomNoteGameNoteController)
			{
				//TODO: fixy ):
				//NoteColorizerType.GetMethod("Create", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(__instance, new object[] { noteController as CustomNoteGameNoteController });
			}
			return true;
		}

		private static Type NoteColorizerType;
		private static PluginMetadata Chroma = PluginManager.GetPluginFromId("Chroma");
	}
}
