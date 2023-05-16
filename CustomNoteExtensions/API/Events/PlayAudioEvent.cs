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
		public string AudioPath { get; set; }
		[JsonIgnore]
		private AudioClip audioClip = null;

		public void OnEvent(NoteEvent noteEvent)
		{
			if(noteEvent.EventType == EventType.Hit)
			{
				if(audioClip != null)
					NoteTypeAudioService.Instance.PlayClip(audioClip);
			}
		}

		[OnDeserialized]
		internal void Deserialized(StreamingContext context)
		{
			Task.Run(async ()=> { await LoadClip();  });
		}

		async Task LoadClip()
		{
			var path = Path.Combine(NoteTypeJSONLoaderService.fullPath, AudioPath);
			AudioClip clip = null;
			using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
			{
				uwr.SendWebRequest();

				// wrap tasks in try/catch, otherwise it'll fail silently
				try
				{
					while (!uwr.isDone) await Task.Delay(5);

					if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
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
