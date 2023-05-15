using CustomJSONData.CustomBeatmap;
using CustomNoteExtensions.API;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CustomNoteExtensions.CustomNotes.Pooling
{
    public class CustomNoteBeatmapObjectManager
    {
        [Inject]
        public virtual void Init(BasicBeatmapObjectManager.InitData initData, BeatmapObjectManager beatmapObjectManager, [Inject(Id = NoteData.GameplayType.Normal)] CustomNoteGameNoteController.Pool fakeBasicGameNotePool)
        {
			this._initData = initData;
            this._beatmapObjectManager = beatmapObjectManager;
            this._CustomNoteBasicGameNotePoolContainer = new MemoryPoolContainer<CustomNoteGameNoteController>(fakeBasicGameNotePool);
            Instance = this;
        }

        public void Process(NoteData noteData, in BeatmapObjectSpawnMovementData.NoteSpawnData noteSpawnData, float rotation, bool forceIsFirstNoteBehaviour)
        {
			Plugin.Log.Info("x");
			if (this._firstBasicNoteTime == null)
			{
				this._firstBasicNoteTime = new float?(noteData.time);
			}
			bool flag;
			if (!forceIsFirstNoteBehaviour)
			{
				float? firstBasicNoteTime = this._firstBasicNoteTime;
				float time = noteData.time;
				flag = (firstBasicNoteTime.GetValueOrDefault() == time) & (firstBasicNoteTime != null);
			}
			else
			{
				flag = true;
			}
			Plugin.Log.Info("x2");
			bool flag2 = flag;
			NoteVisualModifierType noteVisualModifierType = NoteVisualModifierType.Normal;
			if (this._initData.ghostNotes && !flag2)
			{
				noteVisualModifierType = NoteVisualModifierType.Ghost;
			}
			else if (this._initData.disappearingArrows)
			{
				noteVisualModifierType = NoteVisualModifierType.DisappearingArrow;
			}
			Plugin.Log.Info("x3");
			CustomNoteGameNoteController gameNoteController = this._CustomNoteBasicGameNotePoolContainer.Spawn();
			gameNoteController.Init(noteData, rotation, noteSpawnData.moveStartPos, noteSpawnData.moveEndPos, noteSpawnData.jumpEndPos, noteSpawnData.moveDuration, noteSpawnData.jumpDuration, noteSpawnData.jumpGravity, noteVisualModifierType, this._initData.cutAngleTolerance, this._initData.notesUniformScale);
			_beatmapObjectManager.InvokeMethod<object, BeatmapObjectManager>("AddSpawnedNoteController", gameNoteController, noteSpawnData, rotation);
		}

		protected float? _firstBasicNoteTime;

		protected BasicBeatmapObjectManager.InitData _initData;

		internal static CustomNoteBeatmapObjectManager Instance;

        protected BeatmapObjectManager _beatmapObjectManager;

        protected MemoryPoolContainer<CustomNoteGameNoteController> _CustomNoteBasicGameNotePoolContainer;
    }
}