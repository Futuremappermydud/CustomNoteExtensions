using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	//Load asset bundle and play an effect
	//TODO: Implemet an effect bundle format
	//Some JSON Properties: Global or local(to note) positioning, rotation, scaling, animation(heck integration?), shader colors(other shader fields too?)
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
