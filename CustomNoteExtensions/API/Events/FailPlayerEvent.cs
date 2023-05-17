using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	public class FailPlayerEvent : ICustomEvent
	{
		public OnEvent onEvent { get; set; }
		public FailPlayerEvent(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;
		}
		public void OnEvent(NoteEvent noteEvent)
		{
			if(noteEvent.EventType == onEvent)
			{
				EventUtils.Instance.gameEnergyCounter.ProcessEnergyChange(-EventUtils.Instance.gameEnergyCounter.energy);
			}
		}
	}
}
