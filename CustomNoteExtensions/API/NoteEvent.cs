using CustomNoteExtensions.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API
{
	public class NoteEvent
	{
		public GameObject noteObject;
		public readonly NoteData NoteData;
		public readonly OnEvent EventType;
		public readonly NoteCutInfo noteCutInfo;
		internal NoteEvent(NoteData noteData, OnEvent eventType, GameObject note)
		{
			this.EventType = eventType;
			this.NoteData = noteData;
			this.noteObject = note;
		}
		internal NoteEvent(NoteData noteData, NoteCutInfo cut, OnEvent eventType, GameObject note)
		{
			this.EventType = eventType;
			this.NoteData = noteData;
			this.noteCutInfo = cut;
			this.noteObject = note;
		}
	}
}
