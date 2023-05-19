using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomNoteExtensions.API.Events
{
	public enum OnEvent
	{
		None,
		GoodCut,
		BadCut,
		Miss,
		Spawn,
		Update
	}
	public interface ICustomEvent
	{
		public void OnEvent(NoteEvent noteEvent);
	}
}
