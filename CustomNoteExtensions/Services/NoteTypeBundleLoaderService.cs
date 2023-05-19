using Zenject;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomNoteExtensions.Effects;

namespace CustomNoteExtensions.Services
{
	internal class NoteTypeBundleLoaderService : MonoBehaviour
	{
		public static NoteTypeBundleLoaderService Instance;
		public Dictionary<string, Effect> loadedEffects = new Dictionary<string, Effect>();
		void Awake()
		{
			Instance = this;
		}
		public void LoadAsset(string fullPath, Action<Effect> action)
		{
			if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			}
			LoadAsset(fullPath, "Assets/CNE/effect.prefab", (e, name)=> { loadedEffects.Add(name, e); action(e); });
		}

		public static void LoadAsset(string path, string assetName, Action<Effect, string> onComplete = null)
		{
			Effect asset;
			var createRequest = AssetBundle.LoadFromFileAsync(path);
			createRequest.completed += delegate
			{
				if (!createRequest.assetBundle)
				{
					return;
				}

				var assetRequest = createRequest.assetBundle.LoadAssetAsync<GameObject>(assetName);
				assetRequest.completed += delegate {
					Plugin.Log.Info(assetRequest.asset.GetType().Name);
					asset = (assetRequest.asset as GameObject).GetComponent<Effect>(); 
					if(onComplete != null)
					{
						onComplete(asset, asset.Name);
					}
				};
			};
		}
	}
}
