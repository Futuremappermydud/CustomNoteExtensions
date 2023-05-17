using CustomNoteExtensions.API.Events;
using CustomNoteExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomNoteExtensions.Installers
{
	public class CustomNoteExtensionsGameInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<EventUtils>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<NoteTypeAudioService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<LargePromptService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
		}
	}
}
