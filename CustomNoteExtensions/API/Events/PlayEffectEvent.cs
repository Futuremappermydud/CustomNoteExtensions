using CustomNoteExtensions.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CustomNoteExtensions.API.Events
{
	public enum PositioningType
	{
		Global,
		Local
	}
	//Load asset bundle and play an effect
	//Some JSON Properties: Global or local(to note) positioning, rotation, scaling, animation(heck integration?), shader colors(other shader fields too?)
	public class PlayEffectEvent : ICustomEvent
	{
		public OnEvent onEvent { get; set; }
		public string effectName { get; set; }
		public PositioningType positioningType { get; set; }
		public Vector3 position { get; set; }
		public Vector3 scale { get; set; }
		private Transform effect;
		private Transform InstantiatedEffect;
		private Transform note;

		public PlayEffectEvent(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;

			PositioningType newPosType;
			Enum.TryParse(values["positioningType"] as string, out newPosType);
			positioningType = newPosType;

			if (values.ContainsKey("effectName"))
			{
				effectName = values["effectName"] as string;
			}
			if (values.ContainsKey("position"))
			{
				float[] floats = values["position"] as float[];
				position = new Vector3(floats[0], floats[1], floats[2]);
			}
			if (values.ContainsKey("scale"))
			{
				float[] floats = values["scale"] as float[];
				position = new Vector3(floats[0], floats[1], floats[2]);
			}
			var path = Path.Combine(NoteTypeJSONLoaderService.fullPath, effectName + ".nteffect");
			NoteTypeBundleLoaderService.Instance.LoadAsset(path, (e) => {
				this.effect = e.transform;
				this.InstantiatedEffect = GameObject.Instantiate(this.effect.gameObject).transform;
				GameObject.DontDestroyOnLoad(this.InstantiatedEffect); 
			});
		}

		public void OnEvent(NoteEvent noteEvent)
		{
			if(!note)
			{
				note = noteEvent.noteObject.transform.GetChild(0);
				if(!InstantiatedEffect)
				{
					this.InstantiatedEffect = GameObject.Instantiate(this.effect.gameObject).transform;
					GameObject.DontDestroyOnLoad(this.InstantiatedEffect);
				}
			}
			if(noteEvent.EventType == onEvent)
			{
				InstantiatedEffect.transform.SetParent(positioningType == PositioningType.Global ? null : note, false);
				InstantiatedEffect.gameObject.SetActive(false);
				InstantiatedEffect.gameObject.SetActive(true);
			}
		}
	}
}
