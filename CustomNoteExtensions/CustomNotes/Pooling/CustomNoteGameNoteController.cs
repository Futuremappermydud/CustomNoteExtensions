using CustomNoteExtensions.API;
using IPA.Utilities;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Zenject;

public class CustomNoteGameNoteController : NoteController, ICubeNoteControllerInitializable<CustomNoteGameNoteController>, INoteVisualModifierTypeProvider, INoteMovementProvider, IGameNoteMirrorable, INoteMirrorable
{
    public event Action<CustomNoteGameNoteController> cubeNoteControllerDidInitEvent;

    public IBasicCustomNoteType customNoteType;

	public NoteMovement noteMovement
	{
		get
		{
			return this._noteMovement;
		}
	}

	public NoteVisualModifierType noteVisualModifierType
	{
		get
		{
			return this._noteVisualModifierType;
		}
	}

	public NoteData.GameplayType gameplayType
	{
		get
		{
			return this._gameplayType;
		}
	}

	public virtual void Init(NoteData noteData, float worldRotation, Vector3 moveStartPos, Vector3 moveEndPos, Vector3 jumpEndPos, float moveDuration, float jumpDuration, float jumpGravity, NoteVisualModifierType noteVisualModifierType, float cutAngleTolerance, float uniformScale)
	{
		this._noteVisualModifierType = noteVisualModifierType;
		this._gameplayType = noteData.gameplayType;
		this._cutAngleTolerance = cutAngleTolerance;
		Vector3 vector = (2f - uniformScale) * Vector3.one;
		foreach (BoxCuttableBySaber boxCuttableBySaber in this._bigCuttableBySaberList)
		{
			boxCuttableBySaber.transform.localScale = vector;
			boxCuttableBySaber.canBeCut = false;
		}
		foreach (BoxCuttableBySaber boxCuttableBySaber2 in this._smallCuttableBySaberList)
		{
			boxCuttableBySaber2.transform.localScale = vector;
			boxCuttableBySaber2.canBeCut = false;
		}
		bool flag = noteData.gameplayType == NoteData.GameplayType.Normal;
		bool flag2 = noteData.gameplayType == NoteData.GameplayType.Normal;
		base.Init(noteData, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.cutDirection.RotationAngle() + noteData.cutDirectionAngleOffset, uniformScale, flag, flag2);
		cubeNoteControllerDidInitEvent(this);
	}

	public void InitializeFromOld(NoteMovement noteMovement, BoxCuttableBySaber[] small, BoxCuttableBySaber[] big, GameObject wrapper, Transform noteCube)
	{
		_noteTransform = noteCube;
		_noteMovement = noteMovement;
		_smallCuttableBySaberList = small;
		_bigCuttableBySaberList = big;
		_wrapperGO = wrapper;
	}

	protected override void Awake()
	{
		base.Awake();
		BoxCuttableBySaber[] array = this._bigCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].wasCutBySaberEvent += this.HandleBigWasCutBySaber;
		}
		array = this._smallCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].wasCutBySaberEvent += this.HandleSmallWasCutBySaber;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this._bigCuttableBySaberList != null)
		{
			foreach (BoxCuttableBySaber boxCuttableBySaber in this._bigCuttableBySaberList)
			{
				if (boxCuttableBySaber != null)
				{
					boxCuttableBySaber.wasCutBySaberEvent -= this.HandleBigWasCutBySaber;
				}
			}
		}
		if (this._smallCuttableBySaberList != null)
		{
			foreach (BoxCuttableBySaber boxCuttableBySaber2 in this._smallCuttableBySaberList)
			{
				if (boxCuttableBySaber2 != null)
				{
					boxCuttableBySaber2.wasCutBySaberEvent -= this.HandleSmallWasCutBySaber;
				}
			}
		}
	}

	protected override void NoteDidPassMissedMarker()
	{
		BoxCuttableBySaber[] array = this._bigCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
		array = this._smallCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
		base.SendNoteWasMissedEvent();
	}

	protected override void NoteDidStartDissolving()
	{
		BoxCuttableBySaber[] array = this._bigCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
		array = this._smallCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
	}

	public virtual void HandleBigWasCutBySaber(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec)
	{
		this.HandleCut(saber, cutPoint, orientation, cutDirVec, false);
	}

	public virtual void HandleSmallWasCutBySaber(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec)
	{
		this.HandleCut(saber, cutPoint, orientation, cutDirVec, true);
	}

	public virtual void HandleCut(Saber saber, Vector3 cutPoint, Quaternion orientation, Vector3 cutDirVec, bool allowBadCut)
	{
		float num = this.noteData.time - this._audioTimeSyncController.songTime;
		bool flag;
		bool flag2;
		bool flag3;
		float num2;
		float num3;
		NoteBasicCutInfoHelper.GetBasicCutInfo(this._noteTransform, this.noteData.colorType, this.noteData.cutDirection, saber.saberType, saber.bladeSpeed, cutDirVec, this._cutAngleTolerance, out flag, out flag2, out flag3, out num2, out num3);
		if ((!flag || !flag2 || !flag3) && !allowBadCut)
		{
			return;
		}
		Vector3 vector = orientation * Vector3.up;
		Plane plane = new Plane(vector, cutPoint);
		Vector3 position = this._noteTransform.position;
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
		Vector3 vector2 = plane.ClosestPointOnPlane(base.transform.position);
		Vector3 vector3 = vector;
		float num7 = num4;
		float num8 = num3;
		Quaternion worldRotation = base.worldRotation;
		Quaternion inverseWorldRotation = base.inverseWorldRotation;
		Vector3 vector4 = position;
		NoteCutInfo noteCutInfo = new NoteCutInfo(noteData, flag4, flag5, flag6, flag7, bladeSpeed, cutDirVec, saberType, num5, num6, vector2, vector3, num7, num8, worldRotation, inverseWorldRotation, this._noteTransform.rotation, vector4, saber.movementData);
		BoxCuttableBySaber[] array = this._bigCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
		array = this._smallCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = false;
		}
		base.SendNoteWasCutEvent(noteCutInfo);
	}

	protected override void NoteDidStartJump()
	{
		BoxCuttableBySaber[] array = this._bigCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = true;
		}
		array = this._smallCuttableBySaberList;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].canBeCut = true;
		}
	}

	protected override void HiddenStateDidChange(bool hide)
	{
		this._wrapperGO.SetActive(!hide);
	}

	public override void Pause(bool pause)
	{
		base.enabled = !pause;
	}

	[SerializeField]
	protected BoxCuttableBySaber[] _bigCuttableBySaberList;

	[SerializeField]
	protected BoxCuttableBySaber[] _smallCuttableBySaberList;

	[SerializeField]
	protected GameObject _wrapperGO;

	[Inject]
	protected readonly AudioTimeSyncController _audioTimeSyncController;

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
