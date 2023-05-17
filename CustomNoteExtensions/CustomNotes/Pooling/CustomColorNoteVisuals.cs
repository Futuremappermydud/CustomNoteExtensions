using CustomJSONData.CustomBeatmap;
using CustomNoteExtensions.API;
using System;
using UnityEngine;
using Zenject;
using static BeatmapObjectSpawnMovementData;

namespace CustomNoteExtensions.CustomNotes.Pooling
{
    internal class CustomColorNoteVisuals : MonoBehaviour, INoteControllerDidInitEvent, INoteControllerNoteDidPassJumpThreeQuartersEvent, INoteControllerNoteDidStartDissolvingEvent
    {
        public event Action<CustomColorNoteVisuals, NoteControllerBase> didInitEvent;

        private bool showArrow
        {
            set
            {
                MeshRenderer[] arrowMeshRenderers = _arrowMeshRenderers;
                for (int i = 0; i < arrowMeshRenderers.Length; i++)
                {
                    arrowMeshRenderers[i].enabled = value;
                }
            }
        }

        private bool showCircle
        {
            set
            {
                MeshRenderer[] circleMeshRenderers = _circleMeshRenderers;
                for (int i = 0; i < circleMeshRenderers.Length; i++)
                {
                    circleMeshRenderers[i].enabled = value;
                }
            }
        }

        public virtual void Awake()
        {
            _noteController.didInitEvent.Add(this);
            _noteController.noteDidPassJumpThreeQuartersEvent.Add(this);
            _noteController.noteDidStartDissolvingEvent.Add(this);
        }

        public virtual void OnDestroy()
        {
            if (_noteController)
            {
                _noteController.didInitEvent.Remove(this);
                _noteController.noteDidPassJumpThreeQuartersEvent.Remove(this);
                _noteController.noteDidStartDissolvingEvent.Remove(this);
            }
        }

        public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            NoteData noteData = _noteController.noteData;
            if (noteData.cutDirection == NoteCutDirection.Any)
            {
                showArrow = false;
                showCircle = true;
            }
            else
            {
                showArrow = true;
                showCircle = false;
            }
			_noteColor = _colorManager.ColorForType(noteData.colorType);
			if(this._noteController.customNoteType != null)
            {
                _noteColor = this._noteController.customNoteType.NoteColor;
			}
            foreach (MaterialPropertyBlockController materialPropertyBlockController in _materialPropertyBlockControllers)
            {
                materialPropertyBlockController.materialPropertyBlock.SetColor(_colorId, _noteColor.ColorWithAlpha(_defaultColorAlpha));
                materialPropertyBlockController.ApplyChanges();
            }

            if(didInitEvent != null) didInitEvent(this, _noteController);

        }

        public virtual void HandleNoteControllerNoteDidPassJumpThreeQuarters(NoteControllerBase noteController)
        {
            showArrow = false;
            showCircle = false;
        }

        public virtual void HandleNoteControllerNoteDidStartDissolving(NoteControllerBase noteController, float duration)
        {
            showArrow = false;
            showCircle = false;
        }

        public float _defaultColorAlpha = 1f;


        public CustomNoteGameNoteController _noteController;

        public MaterialPropertyBlockController[] _materialPropertyBlockControllers;

        public MeshRenderer[] _arrowMeshRenderers;

        public MeshRenderer[] _circleMeshRenderers;

        [Inject]
        protected readonly ColorManager _colorManager;

        protected static readonly int _colorId = Shader.PropertyToID("_Color");

        protected Color _noteColor;

    }
}
