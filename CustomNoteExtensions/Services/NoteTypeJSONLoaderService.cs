using Zenject;
using UnityEngine;
using System.IO;
using IPA.Utilities;
using Newtonsoft.Json;
using CustomNoteExtensions.CustomNotes;
using CustomNoteExtensions.API;

namespace CustomNoteExtensions.Services
{
	internal class NoteTypeJSONLoaderService : IInitializable
	{
		private const string PATH = "/UserData/CustomNoteTypes";
		private string fullPath = Path.Combine(UnityGame.InstallPath, PATH);
		private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.Auto,
			Formatting = Formatting.Indented,
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};
		public void Initialize()
		{
			if(!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}
			var files =
				Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories);
			foreach (string file in files) 
			{
				var customNote = JsonConvert.DeserializeObject<CustomJSONNote>(file, serializerSettings);
				CustomNoteTypeRegistry.RegisterCustomNote(customNote.Name, customNote);
			}
		}
	}
}
