using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	//Load asset bundle and play an effect
	//TODO: Implemet an effect bundle format
	public class PlayEffectEvent : ICustomEvent
	{
		public OnEvent onEvent { get; set; }

		public PlayEffectEvent(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;
		}

		public void OnEvent(NoteEvent noteEvent)
		{
			if(noteEvent.EventType == onEvent)
			{

			}
		}
	}
}
