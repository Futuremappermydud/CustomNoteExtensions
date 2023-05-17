using CustomNoteExtensions.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomNoteExtensions.Installers
{
	public class CustomNoteExtensionsAppInstaller : Installer
	{
		public override void InstallBindings()
		{
			CustomEventRegistry.RegisterCustomEvent<FailPlayerEvent>("CustomNoteExtensions.FailPlayerEvent");
			CustomEventRegistry.RegisterCustomEvent<PlayAudioEvent>("CustomNoteExtensions.PlayAudioEvent");
			CustomEventRegistry.RegisterCustomEvent<ModifyHealthEvent>("CustomNoteExtensions.ModifyHealthEvent");
			CustomEventRegistry.RegisterCustomEvent<PlayEffectEvent>("CustomNoteExtensions.PlayEffectEvent");
			CustomEventRegistry.RegisterCustomEvent<DisplayPromptEvent>("CustomNoteExtensions.DisplayPromptEvent");
			Container.BindInterfacesAndSelfTo<Services.NoteTypeJSONLoaderService>().AsSingle().NonLazy();
		}
	}
}
