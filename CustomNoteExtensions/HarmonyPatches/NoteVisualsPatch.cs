using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Heck;
using System.Security.AccessControl;
using UnityEngine;

namespace CustomNoteExtensions.HarmonyPatches
{
	/*
	[HarmonyPatch(typeof(ColorNoteVisuals))]
	[HarmonyPatch("HandleNoteControllerDidInit")]
	public static class NoteVisualsPatch
	{
		public static CodeMatcher PrintInstructions(this CodeMatcher codeMatcher, string seperator = "\t")
		{
			codeMatcher.Instructions().ForEach(n => Plugin.Log.Info(seperator + n));
			return codeMatcher;
		}
		public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			var codeMatcher = new CodeMatcher(instructions)
						 .MatchForward(false,
						 new CodeMatch(OpCodes.Ldarg_0, null, "OG This"),
						 new CodeMatch(OpCodes.Ldarg_0),
						 new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(ColorNoteVisuals), "_colorManager")),
						 new CodeMatch(OpCodes.Ldloc_0)
						 );
			Plugin.Log.Info(codeMatcher.Pos.ToString());
			var label = codeMatcher.NamedMatch("OG This").labels[0];
			return codeMatcher
						 .RemoveInstructions(7)
						 .InsertAndAdvance()
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1).WithLabels(label))
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(ColorNoteVisuals), "_colorManager")))
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))	
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(NoteVisualsPatch), "Patch")))
						 .InsertAndAdvance(new CodeInstruction(OpCodes.Stfld, AccessTools.Field(typeof(ColorNoteVisuals), "_noteColor")))
						 .PrintInstructions()
						 .InstructionEnumeration();
		}

		public static Color Patch(NoteControllerBase controllerBase, ColorManager colorManager)
		{
			Plugin.Log.Info("yeaaa");
			return Color.white;
		}
	}*/
}
