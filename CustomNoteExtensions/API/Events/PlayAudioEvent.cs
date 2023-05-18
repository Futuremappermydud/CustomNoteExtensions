using CustomNoteExtensions.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace CustomNoteExtensions.API.Events
{
	public class PlayAudioEvent : ICustomEvent
	{
		public OnEvent onEvent { get; set; }
		public string audioPath { get; set; }
		public float delay { get; set; } = 0f;
		[JsonIgnore]
		private AudioClip audioClip = null;

		public void OnEvent(NoteEvent noteEvent)
		{
			if(noteEvent.EventType == onEvent)
			{
				if (audioClip != null)
					NoteTypeAudioService.Instance.PlayClip(audioClip, delay);
			}
		}

		public PlayAudioEvent(Dictionary<string, object> values)
		{
			OnEvent newEvent;
			Enum.TryParse(values["onEvent"] as string, out newEvent);
			onEvent = newEvent;

			if(values.ContainsKey("audioPath"))
			{
				audioPath = values["audioPath"] as string;
				Task.Run(async () => { await LoadClip(); });
			}
			else
			{
				Plugin.Log.Warn("Audio Event contains no value named audioPath");
			}

			if (values.ContainsKey("delay"))
			{
				float value;
				if (float.TryParse(values["delay"] as string, out value))
				{
					delay = value;
				}
			}
		}

		[OnDeserialized]
		internal void Deserialized(StreamingContext context)
		{
			Task.Run(async ()=> { await LoadClip();  });
		}

		async Task LoadClip()
		{
			var path = Path.Combine(NoteTypeJSONLoaderService.fullPath, audioPath);
			AudioClip clip = null;
			using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.OGGVORBIS))
			{
				uwr.SendWebRequest();

				try
				{
					while (!uwr.isDone) await Task.Delay(5);

					if (uwr.isNetworkError || uwr.isHttpError) Plugin.Log.Info($"{uwr.error}");
					else
					{
						clip = DownloadHandlerAudioClip.GetContent(uwr);
					}
				}
				catch (Exception err)
				{
					Plugin.Log.Warn($"{err.Message}");
					audioClip = null;
				}
			}
			audioClip = clip;
		}
	}
}
