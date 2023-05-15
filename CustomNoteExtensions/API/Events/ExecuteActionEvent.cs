using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	//Not Available for JSON obv
	public class ExecuteActionEvent : ICustomEvent
	{
		Action<NoteEvent> hitAction = null;
		public ExecuteActionEvent(Action<NoteEvent> action) { this.hitAction = action; }
		public void OnEvent(NoteEvent noteEvent)
		{
			if(hitAction != null) hitAction(noteEvent);
		}
	}
}
