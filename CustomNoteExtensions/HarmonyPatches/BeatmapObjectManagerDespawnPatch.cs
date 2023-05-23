using CustomJSONData.CustomBeatmap;
using CustomNoteExtensions.API;
using CustomNoteExtensions.CustomNotes.Pooling;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.HarmonyPatches
{
	[HarmonyPatch(typeof(BasicBeatmapObjectManager))]
	[HarmonyPatch("DespawnInternal", new Type[] { typeof(NoteController) })]
	public class BeatmapObjectManagerDespawnPatch
	{
		public static bool Prefix(NoteController noteController)
		{
			if (noteController.noteData is CustomNoteData customNoteData)
			{
				object type;
				if (customNoteData.customData.TryGetValue("_customNoteType", out type))
				{
					if (type == null) return true;
					IBasicCustomNoteType customNoteType;
					if (CustomNoteTypeRegistry.registeredCustomNotes.TryGetValue(type as string, out customNoteType))
					{
						CustomNoteBeatmapObjectManager.Instance.Despawn(noteController);
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
	}
}
