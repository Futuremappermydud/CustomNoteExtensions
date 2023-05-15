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
		[JsonIgnore]
		public Color color;

		public float r => color.r;
		public float g => color.g;
		public float b => color.b;
		public float a => color.a;

		public static implicit operator Color(ColorWrapper instance)
		{
			return instance.color;
		}

		public static implicit operator ColorWrapper(Color color)
		{
			return new ColorWrapper { color = color };
		}
	}
	public interface IBasicCustomNoteType
	{
		string Name { get; }
		ICustomEvent CustomEvent { get; }
		ColorWrapper NoteColor { get; }
	}
}
