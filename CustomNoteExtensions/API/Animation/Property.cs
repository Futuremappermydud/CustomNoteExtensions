using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API.Animation
{
	public class Property<T>
	{
		public Action<T> OnChange;
		public Dictionary<float, T> Values = new Dictionary<float, T>();
		public Func<T, T, float, T> LerpFunction;

		public static float Map(float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		public void SetT(float T)
		{
			for (int i = 0; i < Values.Count-1; i++) 
			{
				var currentElement = Values.ElementAt(i);
				var nextElement = Values.ElementAt(i+1);
				float currentT = currentElement.Key;
				float nextT = nextElement.Key;
				Plugin.Log.Info(currentT + " " + nextT);
				if(nextT > T && currentT < T)
				{
					float newT = Map(T, currentT, nextT, 0f, 1f);
					OnChange(LerpFunction(currentElement.Value, nextElement.Value, newT));
					break;
				}
			}
		}
	}
}
