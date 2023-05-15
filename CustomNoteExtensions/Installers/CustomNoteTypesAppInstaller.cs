using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomNoteExtensions.Installers
{
	public class CustomNoteTypesAppInstaller : Installer
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<Services.NoteTypeJSONLoaderService>().AsSingle().NonLazy();
		}
	}
}
