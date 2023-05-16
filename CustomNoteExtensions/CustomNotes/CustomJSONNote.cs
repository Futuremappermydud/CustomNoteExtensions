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
	[Serializable]
	internal class CustomJSONNote : IBasicCustomNoteType
	{
		public string name = "JsonObject";
		public ICustomEvent[] noteEvents = new ICustomEvent[0];
		public ColorWrapper color = Color.white;
		public string jsonVersion => "0.1.0";

		[JsonIgnore]
		public ICustomEvent[] CustomEvents => noteEvents;
		[JsonIgnore]
		public string Name => name;
		[JsonIgnore]
		public ColorWrapper NoteColor => color;
	}
}
