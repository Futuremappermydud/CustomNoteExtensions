using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	public class ModifyHealthEvent : ICustomEvent
	{
		public float HealthDelta { get; set; }
		public void OnEvent(NoteEvent noteEvent)
		{
		}
	}
}
