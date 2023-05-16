using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	public class FailPlayerEvent : ICustomEvent
	{
		public void OnEvent(NoteEvent noteEvent)
		{
			if(noteEvent.EventType == EventType.Hit)
			{
				EventUtils.Instance.gameEnergyCounter.ProcessEnergyChange(-1f);
			}
		}
	}
}
