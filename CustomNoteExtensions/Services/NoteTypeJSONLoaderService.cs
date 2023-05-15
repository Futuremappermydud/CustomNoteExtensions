using Zenject;
using UnityEngine;
using System.IO;
using IPA.Utilities;
using Newtonsoft.Json;
using CustomNoteExtensions.CustomNotes;
using CustomNoteExtensions.API;
using System;

namespace CustomNoteExtensions.Services
{
	internal class NoteTypeJSONLoaderService : IInitializable
	{
		private const string PATH = "UserData/CustomNoteTypes/";
		private string fullPath = Path.Combine(UnityGame.InstallPath, PATH);
		private JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.Auto
		};
		public void Initialize()
		{
			if(!Directory.Exists(fullPath))
			{
				Directory.CreateDirectory(fullPath);
			}
			Console.WriteLine(fullPath);
			var files =
				Directory.GetFiles(fullPath, "*.json", SearchOption.AllDirectories);
			foreach (string file in files) 
			{
				Console.WriteLine(file);
				var customNote = JsonConvert.DeserializeObject<CustomJSONNote>(File.ReadAllText(file), serializerSettings);
				CustomNoteTypeRegistry.RegisterCustomNote(customNote.Name, customNote);
			}
		}
	}
}
