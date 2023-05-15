using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ModestTree;
using CustomJSONData.CustomBeatmap;
using CustomNoteExtensions.API;
using System.Diagnostics;
using CustomNoteExtensions.CustomNotes.Pooling;

namespace CustomNoteExtensions.HarmonyPatches
{
	[HarmonyPatch(typeof(BasicBeatmapObjectManager))]
	[HarmonyPatch(nameof(BasicBeatmapObjectManager.ProcessNoteData))]
	public class BasicBeatmapObjectManagerProcessNoteDataPatch
	{
		public static bool Prefix(BasicBeatmapObjectManager __instance, NoteData noteData, BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData, float rotation, bool forceIsFirstNoteBehaviour)
		{
			if (noteData is CustomNoteData customNoteData)
			{
				object type;
				if (customNoteData.customData.TryGetValue("_customNoteType", out type))
				{
					if (type == null) return true;
					IBasicCustomNoteType customNoteType = null;
					if (CustomNoteTypeRegistry.registeredCustomNotes.TryGetValue(type as string, out customNoteType))
					{
						CustomNoteBeatmapObjectManager.Instance.Process(noteData, noteSpawnData, rotation, forceIsFirstNoteBehaviour);
						return false;
					}
					else
					{
						return true;
					}
				}
			}
			return true;
		}
		public static void PoolObject(NoteData data)
		{
		}
	}
}
