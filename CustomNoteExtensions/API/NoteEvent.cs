using CustomNoteExtensions.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API
{
	public class NoteEvent
	{
		public readonly NoteData NoteData;
		public readonly OnEvent EventType;
		public readonly NoteCutInfo noteCutInfo;
		internal NoteEvent(NoteData noteData, OnEvent eventType)
		{
			this.EventType = eventType;
			this.NoteData = noteData;
		}
		internal NoteEvent(NoteData noteData, NoteCutInfo cut, OnEvent eventType)
		{
			this.EventType = eventType;
			this.NoteData = noteData;
			this.noteCutInfo = cut;
		}
	}
}
