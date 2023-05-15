using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomNoteExtensions.Installers
{
	internal class CustomNoteTypesAppInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.Bind<Services.NoteTypeJSONLoaderService>().AsSingle().NonLazy();
		}
	}
}
