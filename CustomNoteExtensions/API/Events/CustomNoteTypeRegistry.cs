using System;
using System.Collections.Generic;

namespace CustomNoteExtensions.API.Events
{
	public class CustomEventRegistry
	{
		internal static Dictionary<string, Type> registeredCustomEvents { get; set; } = new Dictionary<string, Type> ();

		public static void RegisterCustomEvent<T>(string name) where T : ICustomEvent
		{
			if(registeredCustomEvents.ContainsKey(name))
			{
				registeredCustomEvents[name] = typeof(T);
			}
			else
			{
				registeredCustomEvents.Add(name, typeof(T));
			}
		}

		public static void UnregisterCustomEvent(string name)
		{
			if (registeredCustomEvents.ContainsKey(name))
			{
				registeredCustomEvents.Remove(name);
			}
			else
			{
				Plugin.Log.Info(string.Format("Custom Note {0} is not registered, ignoring unregister", name));
			}
		}
    }
}
