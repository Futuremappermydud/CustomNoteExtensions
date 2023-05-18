using CustomNoteExtensions.API;
using CustomNoteExtensions.API.Events;
using IPA.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.CustomNotes
{
	internal class SerializedEvent
	{
		public string Type;
		public Dictionary<string, object> Values;
	}
	[Serializable]
	internal class CustomJSONNote : IBasicCustomNoteType
	{
		public string name = "JsonObject";
		public SerializedEvent[] noteEvents = new SerializedEvent[0];
		public ColorWrapper color = Color.white;
		public bool isGood = true;
		public string jsonVersion => "0.1.0";

		[OnDeserialized]
		internal void Deserialized(StreamingContext context)
		{
			convertedEvents = new ICustomEvent[noteEvents.Length];
			for (int i = 0; i < noteEvents.Length; i++)
			{
				var type = noteEvents[i].Type;
				var values = noteEvents[i].Values;
				if (CustomEventRegistry.registeredCustomEvents.ContainsKey(type))
				{
					ICustomEvent instance = Activator.CreateInstance(CustomEventRegistry.registeredCustomEvents[type], new object[] { values }) as ICustomEvent;
					convertedEvents[i] = instance;
				}
				else
				{
					Plugin.Log.Error(string.Format("Missing Event Type {} Disabling Score Submission", type));
					//TODO: Disable Score Submission
				}
			}
		}
		[JsonIgnore]
		private ICustomEvent[] convertedEvents;
		[JsonIgnore]
		public ICustomEvent[] CustomEvents => convertedEvents;
		[JsonIgnore]
		public string Name => name;
		[JsonIgnore]
		public ColorWrapper NoteColor => color;

		public bool IsGood => isGood;
	}
}
