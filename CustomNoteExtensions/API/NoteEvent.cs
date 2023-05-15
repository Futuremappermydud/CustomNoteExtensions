using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API
{
	public enum EventType
	{
		Hit,
		Missed
	}
	public class NoteEvent
	{
		public readonly NoteData NoteData;
		public readonly EventType EventType;
		internal NoteEvent(NoteData noteData, EventType eventType)
		{
			this.EventType = eventType;
			this.NoteData = noteData;
		}
	}
}
