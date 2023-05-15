using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API
{
	public class CustomNoteTypeRegistry
	{
		internal static Dictionary<string, IBasicCustomNoteType> registeredCustomNotes { get; set; }

		public static void RegisterCustomNote(string name, IBasicCustomNoteType customNote)
		{
			if(registeredCustomNotes.ContainsKey(name))
			{
				registeredCustomNotes[name] = customNote;
			}
			else
			{
				registeredCustomNotes.Add(name, customNote);
			}
		}

		public static void UnregisterCustomNote(string name)
		{
			if (registeredCustomNotes.ContainsKey(name))
			{
				registeredCustomNotes.Remove(name);
			}
			else
			{
				Plugin.Log.Info(string.Format("Custom Note {0} is not registered, ignoring unregister", name));
			}
		}
    }
}
