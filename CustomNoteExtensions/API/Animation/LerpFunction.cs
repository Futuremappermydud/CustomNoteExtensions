using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API.Animation
{
	public static class LerpFunction
	{
		public static Dictionary<string, Func<object, object, float, object>> functions = new Dictionary<string, Func<object, object, float, object>>
		{
			{ "color", (object c1, object c2, float t ) => {
				Color color1 = Color.white;
				ColorUtility.TryParseHtmlString(c1 as string, out color1);
				Color color2 = Color.white;
				ColorUtility.TryParseHtmlString(c2 as string, out color2);
				float h1, s1, v1, h2, s2, v2;
				Color.RGBToHSV(color1, out h1, out s1, out v1);
				Color.RGBToHSV(color2, out h2, out s2, out v2);
				return Color.HSVToRGB(Mathf.Lerp(h1, h2, t), Mathf.Lerp(s1, s2, t), Mathf.Lerp(v1, v2, t));
			} }
		};
	}
}
