using CustomNoteExtensions.API;
using CustomNoteExtensions.API.Animation;
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
	internal class SerializedProperty
	{
		public Dictionary<string, string> Values;
	}
	[Serializable]
	internal class CustomJSONNote : IBasicCustomNoteType
	{
		public string name = "JsonObject";
		public SerializedEvent[] noteEvents = new SerializedEvent[0];
		public ColorWrapper color = Color.white;
		public bool isGood = true;
		public string jsonVersion => "0.1.0";
		public Dictionary<string, Dictionary<string, string>> properties = new Dictionary<string, Dictionary<string, string>>();

		[OnDeserialized]
		internal void Deserialized(StreamingContext context)
		{
			convertedEvents = new ICustomEvent[noteEvents.Length];
			for (int i = 0; i < noteEvents.Length; i++)
			{
				var type = noteEvents[i].Type;
				var values = noteEvents[i].Values;
				values.Add("CustomNote", this);
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
			convertedProperties = new Dictionary<string, Property<object>>();
			for (int i = 0; i < properties.Count; i++)
			{
				var property = new Property<object>();
				var values = properties.ElementAt(i);
				for (int x = 0; x < values.Value.Count; x ++)
				{
					var value = values.Value.ElementAt(x);
					if (value.Key == "type")
					{
						Plugin.Log.Info(value.Key);
						property.LerpFunction = LerpFunction.functions[value.Value];
					}
					else
					{
						Plugin.Log.Info(value.Key);
						property.Values.Add(float.Parse(value.Key), value.Value);
					}
				}
				property.Values.OrderBy(x => x.Key).ToList();
				convertedProperties.Add(values.Key, property);
			}

			if(convertedProperties.ContainsKey(color.property))
			{
				Plugin.Log.Info("b");
				convertedProperties[color.property].OnChange += (c1) => { color = c1 as Color?; Plugin.Log.Info(((Color)color).ToString()); };
			}
		}
		[JsonIgnore]
		private ICustomEvent[] convertedEvents;
		[JsonIgnore]
		private Dictionary<string, Property<object>> convertedProperties;
		[JsonIgnore]
		public ICustomEvent[] CustomEvents => convertedEvents;
		[JsonIgnore]
		public string Name => name;
		[JsonIgnore]
		public ColorWrapper NoteColor => color;

		public bool IsGood => isGood;
		[JsonIgnore]
		public Property<object>[] Properties => convertedProperties.Values.ToArray();
	}
}
