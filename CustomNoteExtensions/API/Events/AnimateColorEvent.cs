using CustomNoteExtensions.API.Animation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API.Events
{
	public class AnimateColor : ICustomEvent
	{
		public OnEvent onEvent { get; set; }
		public Path path { get; set; }
		public float delay { get; set; } = 0f;
		public AnimateColor(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;

			if (values.ContainsKey("path"))
			{
				Dictionary<string, object> pathObjects = values["path"] as Dictionary<string, object>;
				foreach(var x in pathObjects)
				{

				}
			}

			if (values.ContainsKey("delay"))
			{
				float value;
				if (float.TryParse(values["delay"] as string, out value))
				{
					delay = value;
				}
			}
		}
		public void OnEvent(NoteEvent noteEvent)
		{
			if (noteEvent.EventType == onEvent)
			{
			}
		}
	}
}
