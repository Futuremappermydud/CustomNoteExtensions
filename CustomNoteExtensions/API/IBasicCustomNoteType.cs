using CustomNoteExtensions.API.Events;
using IPA.Config.Stores.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API
{
	public class ColorWrapper
	{
		public float r; 
		public float g; 
		public float b; 
		public float a;

		public static implicit operator Color(ColorWrapper instance)
		{
			return new Color(instance.r, instance.g, instance.b, instance.a);
		}

		public static implicit operator ColorWrapper(Color color)
		{
			return new ColorWrapper { r = color.r, g = color.g, b = color.b, a = color.a };
		}
	}
	public interface IBasicCustomNoteType
	{
		string Name { get; }
		ICustomEvent[] CustomEvents { get; }
		ColorWrapper NoteColor { get; }
	}
}
