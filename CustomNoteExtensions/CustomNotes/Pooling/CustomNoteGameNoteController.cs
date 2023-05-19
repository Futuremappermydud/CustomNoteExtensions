using BeatSaberMarkupLanguage;
using CustomNoteExtensions.API;
using CustomNoteExtensions;
using IPA.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Zenject;
using CustomJSONData.CustomBeatmap;
using CustomNoteExtensions.API.Events;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

namespace CustomNoteExtensions.CustomNotes.Pooling
{
	public class CustomNoteGameNoteController : NoteController, ICubeNoteControllerInitializable<CustomNoteGameNoteController>, INoteVisualModifierTypeProvider, INoteMovementProvider, IGameNoteMirrorable, INoteMirrorable
	{
		public event Action<CustomNoteGameNoteController> cubeNoteControllerDidInitEvent;

		public IBasicCustomNoteType customNoteType;

		public NoteMovement noteMovement
		{
			get
			{
				return _noteMovement;
			}
		}

		public NoteVisualModifierType noteVisualModifierType
		{
			get
			{
				return _noteVisualModifierType;
			}
		}

		public NoteData.GameplayType gameplayType
		{
			get
			{
				return _gameplayType;
			}
		}

		public virtual void Init(NoteData noteData, float worldRotation, Vector3 moveStartPos, Vector3 moveEndPos, Vector3 jumpEndPos, float moveDuration, float jumpDuration, float jumpGravity, NoteVisualModifierType noteVisualModifierType, float cutAngleTolerance, float uniformScale)
		{
			_noteVisualModifierType = noteVisualModifierType;
			_gameplayType = noteData.gameplayType;
			_cutAngleTolerance = cutAngleTolerance;
			Vector3 vector = (2f - uniformScale) * Vector3.one;
			foreach (BoxCuttableBySaber boxCuttableBySaber in _bigCuttableBySaberList)
			{
				boxCuttableBySaber.transform.localScale = vector;
				boxCuttableBySaber.canBeCut = false;
			}
			foreach (BoxCuttableBySaber boxCuttableBySaber2 in _smallCuttableBySaberList)
			{
				boxCuttableBySaber2.transform.localScale = vector;
				boxCuttableBySaber2.canBeCut = false;
			}
			bool flag = noteData.gameplayType == NoteData.GameplayType.Normal;
			bool flag2 = noteData.gameplayType == NoteData.GameplayType.Normal;
			base.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.cutDirection.RotationAngle() + noteData.cutDirectionAngleOffset, uniformScale, flag, flag2);
			cubeNoteControllerDidInitEvent(this);
		}

		public void InitializeFromOld(NoteMovement noteMovement, BoxCuttableBySaber[] small, BoxCuttableBySaber[] big, GameObject wrapper, Transform noteCube, AudioTimeSyncController audioTimeSyncController)
		{
			_noteTransform = noteCube;
			_noteMovement = noteMovement;
			_smallCuttableBySaberList = small;
			_bigCuttableBySaberList = big;
			_wrapperGO = wrapper;
			_audioTimeSyncController = audioTimeSyncController;
		}

		protected override void Awake()
		{
			if (noteData is CustomNoteData customNoteData)
			{
				object type;
				if (customNoteData.customData.TryGetValue("_customNoteType", out type))
				{
					if (type == null) return;
					CustomNoteTypeRegistry.registeredCustomNotes.TryGetValue(type as string, out customNoteType);
				}
			}
			base.Awake();
			BoxCuttableBySaber[] array = _bigCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].wasCutBySaberEvent += HandleBigWasCutBySaber;
			}
			array = _smallCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].wasCutBySaberEvent += HandleSmallWasCutBySaber;
			}
			hasInvokedSpawnEvent = false;
		}

		public new void Update()
		{
			base.Update();
			if(customNoteType != null)
			{
				if(!hasInvokedSpawnEvent)
				{
					hasInvokedSpawnEvent = true;
					for (int i = 0; i < customNoteType.CustomEvents.Length; i++)
					{
						customNoteType.CustomEvents[i].OnEvent(new NoteEvent(noteData, OnEvent.Spawn, gameObject));
					}
				}
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (_bigCuttableBySaberList != null)
			{
				foreach (BoxCuttableBySaber boxCuttableBySaber in _bigCuttableBySaberList)
				{
					if (boxCuttableBySaber != null)
					{
						boxCuttableBySaber.wasCutBySaberEvent -= HandleBigWasCutBySaber;
					}
				}
			}
			if (_smallCuttableBySaberList != null)
			{
				foreach (BoxCuttableBySaber boxCuttableBySaber2 in _smallCuttableBySaberList)
				{
					if (boxCuttableBySaber2 != null)
					{
						boxCuttableBySaber2.wasCutBySaberEvent -= HandleSmallWasCutBySaber;
					}
				}
			}
		}

		protected override void NoteDidPassMissedMarker()
		{
			BoxCuttableBySaber[] array = _bigCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
			array = _smallCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
			for (int i = 0; i < customNoteType.CustomEvents.Length; i++)
			{
				customNoteType.CustomEvents[i].OnEvent(new NoteEvent(noteData, OnEvent.Miss, gameObject));
			}
			//This will cause 0 issues whatsoever
			if(customNoteType.IsGood)
			{
				SendNoteWasMissedEvent();
			}
			else
			{
				var saberMovement = new SaberMovementData();
				saberMovement.AddNewData(Vector3.zero, Vector3.up, 0);
				NoteCutInfo newNoteCutInfo = new NoteCutInfo(noteData, true, true, true, false, 15f, Vector3.zero, SaberType.SaberA, 0f, 0f, Vector3.zero, Vector3.up, 0f, 0f, worldRotation, inverseWorldRotation, _noteTransform.rotation, _noteTransform.position, saberMovement);
				SendNoteWasCutEvent(newNoteCutInfo);
			}
		}
		protected override void NoteDidStartDissolving()
		{
			BoxCuttableBySaber[] array = _bigCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
			array = _smallCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
		}

		public virtual void HandleBigWasCutBySaber(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec)
		{
			HandleCut(saber, cutPoint, orientation, cutDirVec, false);
		}

		public virtual void HandleSmallWasCutBySaber(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec)
		{
			HandleCut(saber, cutPoint, orientation, cutDirVec, true);
		}

		public virtual void HandleCut(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec, bool allowBadCut)
		{
			float num = this.noteData.time - _audioTimeSyncController.songTime;
			bool flag;
			bool flag2;
			bool flag3;
			float num2;
			float num3;
			NoteBasicCutInfoHelper.GetBasicCutInfo(_noteTransform, this.noteData.colorType, this.noteData.cutDirection, saber.saberType, saber.bladeSpeed, cutDirVec, _cutAngleTolerance, out flag, out flag2, out flag3, out num2, out num3);
			if ((!flag || !flag2 || !flag3) && !allowBadCut)
			{
				return;
			}
			Vector3 vector = orientation * Vector3.up;
			Plane plane = new Plane(vector, cutPoint);
			Vector3 position = _noteTransform.position;
			float num4 = Mathf.Abs(plane.GetDistanceToPoint(position));
			NoteData noteData = this.noteData;
			bool flag4 = flag2;
			bool flag5 = flag;
			bool flag6 = flag3;
			bool flag7 = false;
			float bladeSpeed = saber.bladeSpeed;
			SaberType saberType = saber.saberType;
			float num5 = num;
			float num6 = num2;
			Vector3 vector2 = plane.ClosestPointOnPlane(transform.position);
			Vector3 vector3 = vector;
			float num7 = num4;
			float num8 = num3;
			Quaternion worldRotation = base.worldRotation;
			Quaternion inverseWorldRotation = base.inverseWorldRotation;
			Vector3 vector4 = position;
			NoteCutInfo noteCutInfo = new NoteCutInfo(noteData, flag4, flag5, flag6, flag7, bladeSpeed, cutDirVec, saberType, num5, num6, vector2, vector3, num7, num8, worldRotation, inverseWorldRotation, _noteTransform.rotation, vector4, saber.movementData);
			BoxCuttableBySaber[] array = _bigCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
			array = _smallCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = false;
			}
			OnEvent Event = noteCutInfo.allIsOK ? OnEvent.GoodCut : OnEvent.BadCut;
			for (int i = 0; i < customNoteType.CustomEvents.Length; i++)
			{
				customNoteType.CustomEvents[i].OnEvent(new NoteEvent(noteData, noteCutInfo, Event, gameObject));
			}

			if (customNoteType.IsGood)
			{
				//Send cut as is
				SendNoteWasCutEvent(noteCutInfo);
			}
			else
			{
				if (noteCutInfo.allIsOK)
				{
					//return a "Bad cut" if the note is a base note but the cut is actually good
					NoteCutInfo newNoteCutInfo = new NoteCutInfo(noteData, flag4, flag5, false, flag7, bladeSpeed, cutDirVec, saberType, num5, num6, vector2, vector3, num7, num8, worldRotation, inverseWorldRotation, _noteTransform.rotation, vector4, saber.movementData);
					SendNoteWasCutEvent(newNoteCutInfo);
				}
				else
				{
					//return a "Good cut" if the note is a bad note and the cut is actually a bad cut
					NoteCutInfo newNoteCutInfo = new NoteCutInfo(noteData, true, true, true, false, bladeSpeed, cutDirVec, saberType, num5, num6, vector2, vector3, num7, num8, worldRotation, inverseWorldRotation, _noteTransform.rotation, vector4, saber.movementData);
					SendNoteWasCutEvent(newNoteCutInfo);
				}
			}
		}

		protected override void NoteDidStartJump()
		{
			BoxCuttableBySaber[] array = _bigCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = true;
			}
			array = _smallCuttableBySaberList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].canBeCut = true;
			}
		}

		protected override void HiddenStateDidChange(bool hide)
		{
			_wrapperGO.SetActive(!hide);
		}

		public override void Pause(bool pause)
		{
			enabled = !pause;
		}

		bool hasInvokedSpawnEvent = false;

		[SerializeField]
		protected BoxCuttableBySaber[] _bigCuttableBySaberList;

		[SerializeField]
		protected BoxCuttableBySaber[] _smallCuttableBySaberList;

		[SerializeField]
		protected GameObject _wrapperGO;

		[Inject]
		protected AudioTimeSyncController _audioTimeSyncController;

		protected NoteVisualModifierType _noteVisualModifierType;

		protected NoteData.GameplayType _gameplayType;

		protected float _cutAngleTolerance;

		[Inject]
		protected readonly ColorManager _colorManager;

		protected static readonly int _colorId = Shader.PropertyToID("_Color");

		public class Pool : MonoMemoryPool<CustomNoteGameNoteController>
		{
		}
	}
}