using CustomNoteExtensions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace CustomNoteExtensions.API.Events
{
	internal class EventUtils
	{
		public static EventUtils Instance;
		public GameEnergyCounter gameEnergyCounter;
		public LargePromptService largePromptService;

		[Inject]
		public void Initialize(GameEnergyCounter _gameEnergyCounter, LargePromptService _largePromptService)
		{
			Instance = this;
			gameEnergyCounter = _gameEnergyCounter;
			largePromptService = _largePromptService;
		}
	}
}
