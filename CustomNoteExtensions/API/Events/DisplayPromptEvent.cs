using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API.Events
{
	public class DisplayPromptEvent : ICustomEvent
	{
		public OnEvent onEvent { get; set; }
		public string text { get; set; } = "!Display!";
		public float length { get; set; } = 1.5f;
		public Color color { get; set; } = Color.white;
		public float delay { get; set; } = 0f;
		public DisplayPromptEvent(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;

			if (values.ContainsKey("text"))
			{
				text = values["text"] as string;
			}
			else
			{
				Plugin.Log.Warn("DisplayPrompt Event contains no value named text");
			}

			if (values.ContainsKey("length"))
			{
				float value;
				if(float.TryParse(values["length"] as string, out value))
				{
					length = value;
				}
			}

			if (values.ContainsKey("color"))
			{
				Color color;
				if(ColorUtility.TryParseHtmlString(values["color"] as string, out color))
				{
					this.color = color;
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
				EventUtils.Instance.largePromptService.Prompt(text, length, color, delay);
			}
		}
	}
}
