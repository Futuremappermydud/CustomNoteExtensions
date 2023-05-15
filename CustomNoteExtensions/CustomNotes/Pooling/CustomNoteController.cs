using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomNoteController : NoteControllerBase, INoteMirrorable, IBeatmapObjectController
{
	public override ILazyCopyHashSet<INoteControllerDidInitEvent> didInitEvent
	{
		get
		{
			return this._didInitEvent;
		}
	}

	public ILazyCopyHashSet<INoteControllerNoteDidStartJumpEvent> noteDidStartJumpEvent
	{
		get
		{
			return this._noteDidStartJumpEvent;
		}
	}

	public ILazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent> noteDidFinishJumpEvent
	{
		get
		{
			return this._noteDidFinishJumpEvent;
		}
	}

	public override ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> noteDidPassJumpThreeQuartersEvent
	{
		get
		{
			return this._noteDidPassJumpThreeQuartersEvent;
		}
	}

	public ILazyCopyHashSet<INoteControllerNoteWasCutEvent> noteWasCutEvent
	{
		get
		{
			return this._noteWasCutEvent;
		}
	}

	public ILazyCopyHashSet<INoteControllerNoteWasMissedEvent> noteWasMissedEvent
	{
		get
		{
			return this._noteWasMissedEvent;
		}
	}

	public override ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> noteDidStartDissolvingEvent
	{
		get
		{
			return this._noteDidStartDissolvingEvent;
		}
	}

	public ILazyCopyHashSet<INoteControllerNoteDidDissolveEvent> noteDidDissolveEvent
	{
		get
		{
			return this._noteDidDissolveEvent;
		}
	}

	public Transform noteTransform
	{
		get
		{
			return this._noteTransform;
		}
	}

	public Quaternion worldRotation
	{
		get
		{
			return this._noteMovement.worldRotation;
		}
	}

	public Quaternion inverseWorldRotation
	{
		get
		{
			return this._noteMovement.inverseWorldRotation;
		}
	}

	public float moveStartTime
	{
		get
		{
			return this._noteMovement.moveStartTime;
		}
	}

	public float moveDuration
	{
		get
		{
			return this._noteMovement.moveDuration;
		}
	}

	public float jumpDuration
	{
		get
		{
			return this._noteMovement.jumpDuration;
		}
	}

	public Vector3 jumpMoveVec
	{
		get
		{
			return this._noteMovement.jumpMoveVec;
		}
	}

	public Vector3 beatPos
	{
		get
		{
			return this._noteMovement.beatPos;
		}
	}

	public Vector3 jumpStartPos
	{
		get
		{
			return this._noteMovement.moveEndPos;
		}
	}

	public override NoteData noteData
	{
		get
		{
			return this._noteData;
		}
	}

	public Vector3 moveVec
	{
		get
		{
			return this.worldRotation * this.jumpMoveVec;
		}
	}

	public float uniformScale
	{
		get
		{
			return this._uniformScale;
		}
	}

	public bool hidden { get; private set; }

	protected virtual void Awake()
	{
		this._noteMovement.noteDidFinishJumpEvent += this.HandleNoteDidFinishJump;
		this._noteMovement.noteDidStartJumpEvent += this.HandleNoteDidStartJump;
		this._noteMovement.noteDidPassJumpThreeQuartersEvent += this.HandleNoteDidPassJumpThreeQuarters;
		this._noteMovement.noteDidPassMissedMarkerEvent += this.HandleNoteDidPassMissedMarkerEvent;
	}

	protected virtual void OnDestroy()
	{
		if (this._noteMovement != null)
		{
			this._noteMovement.noteDidFinishJumpEvent -= this.HandleNoteDidFinishJump;
			this._noteMovement.noteDidStartJumpEvent -= this.HandleNoteDidStartJump;
			this._noteMovement.noteDidPassJumpThreeQuartersEvent -= this.HandleNoteDidPassJumpThreeQuarters;
			this._noteMovement.noteDidPassMissedMarkerEvent -= this.HandleNoteDidPassMissedMarkerEvent;
		}
	}

	protected void Update()
	{
		this.ManualUpdate();
	}

	public virtual void ManualUpdate()
	{
		this._noteMovement.ManualUpdate();
	}

	private void HandleNoteDidStartJump()
	{
		this.NoteDidStartJump();
		foreach (INoteControllerNoteDidStartJumpEvent noteControllerNoteDidStartJumpEvent in this._noteDidStartJumpEvent.items)
		{
			noteControllerNoteDidStartJumpEvent.HandleNoteControllerNoteDidStartJump((NoteController)(this as NoteControllerBase));
		}
	}

	private void HandleNoteDidFinishJump()
	{
		if (this._dissolving)
		{
			return;
		}
		this.NoteDidFinishJump();
		foreach (INoteControllerNoteDidFinishJumpEvent noteControllerNoteDidFinishJumpEvent in this._noteDidFinishJumpEvent.items)
		{
			noteControllerNoteDidFinishJumpEvent.HandleNoteControllerNoteDidFinishJump((NoteController)(this as NoteControllerBase));
		}
	}

	private void HandleNoteDidPassJumpThreeQuarters(NoteMovement noteMovement)
	{
		if (this._dissolving)
		{
			return;
		}
		this.NoteDidPassJumpThreeQuarters(noteMovement);
		foreach (INoteControllerNoteDidPassJumpThreeQuartersEvent noteControllerNoteDidPassJumpThreeQuartersEvent in this._noteDidPassJumpThreeQuartersEvent.items)
		{
			noteControllerNoteDidPassJumpThreeQuartersEvent.HandleNoteControllerNoteDidPassJumpThreeQuarters(this);
		}
	}

	private void HandleNoteDidPassMissedMarkerEvent()
	{
		if (this._dissolving)
		{
			return;
		}
		this.NoteDidPassMissedMarker();
	}

	protected virtual void NoteDidStartJump()
	{
	}

	protected virtual void NoteDidFinishJump()
	{
	}

	protected virtual void NoteDidPassJumpThreeQuarters(NoteMovement noteMovement)
	{
	}

	protected virtual void NoteDidPassMissedMarker()
	{
	}

	protected virtual void NoteDidStartDissolving()
	{
	}

	protected void SendNoteWasMissedEvent()
	{
		foreach (INoteControllerNoteWasMissedEvent noteControllerNoteWasMissedEvent in this._noteWasMissedEvent.items)
		{
			noteControllerNoteWasMissedEvent.HandleNoteControllerNoteWasMissed((NoteController)(this as NoteControllerBase));
		}
	}

	protected void SendNoteWasCutEvent(in NoteCutInfo noteCutInfo)
	{
		foreach (INoteControllerNoteWasCutEvent noteControllerNoteWasCutEvent in this._noteWasCutEvent.items)
		{
			noteControllerNoteWasCutEvent.HandleNoteControllerNoteWasCut((NoteController)(this as NoteControllerBase), noteCutInfo);
		}
	}

	protected void Init(NoteData noteData, float worldRotation, Vector3 moveStartPos, Vector3 moveEndPos, Vector3 jumpEndPos, float moveDuration, float jumpDuration, float jumpGravity, float endRotation, float uniformScale, bool rotateTowardsPlayer, bool useRandomRotation)
	{
		this._noteData = noteData;
		this._uniformScale = uniformScale;
		base.transform.SetPositionAndRotation(moveStartPos, Quaternion.identity);
		this._noteTransform.localScale = Vector3.one * uniformScale;
		this._noteMovement.Init(noteData.time, worldRotation, moveStartPos, moveEndPos, jumpEndPos, moveDuration, jumpDuration, jumpGravity, noteData.flipYSide, endRotation, rotateTowardsPlayer, useRandomRotation);
		foreach (INoteControllerDidInitEvent noteControllerDidInitEvent in this._didInitEvent.items)
		{
			noteControllerDidInitEvent.HandleNoteControllerDidInit(this);
		}
	}

	private IEnumerator DissolveCoroutine(float duration)
	{
		foreach (INoteControllerNoteDidStartDissolvingEvent noteControllerNoteDidStartDissolvingEvent in this._noteDidStartDissolvingEvent.items)
		{
			noteControllerNoteDidStartDissolvingEvent.HandleNoteControllerNoteDidStartDissolving(this, duration);
		}
		yield return new WaitForSeconds(duration);
		this._dissolving = false;
		using (List<INoteControllerNoteDidDissolveEvent>.Enumerator enumerator2 = this._noteDidDissolveEvent.items.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				INoteControllerNoteDidDissolveEvent noteControllerNoteDidDissolveEvent = enumerator2.Current;
				noteControllerNoteDidDissolveEvent.HandleNoteControllerNoteDidDissolve((NoteController)(this as NoteControllerBase));
			}
			yield break;
		}
	}

	public void Dissolve(float duration)
	{
		if (this._dissolving)
		{
			return;
		}
		this._dissolving = true;
		this.NoteDidStartDissolving();
		base.StartCoroutine(this.DissolveCoroutine(duration));
	}

	protected abstract void HiddenStateDidChange(bool hidden);

	public void Hide(bool hide)
	{
		this.hidden = hide;
		this.HiddenStateDidChange(hide);
	}

	public abstract void Pause(bool pause);

	[SerializeField]
	protected NoteMovement _noteMovement;

	[SerializeField]
	protected Transform _noteTransform;

	private readonly LazyCopyHashSet<INoteControllerDidInitEvent> _didInitEvent = new LazyCopyHashSet<INoteControllerDidInitEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteDidStartJumpEvent> _noteDidStartJumpEvent = new LazyCopyHashSet<INoteControllerNoteDidStartJumpEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent> _noteDidFinishJumpEvent = new LazyCopyHashSet<INoteControllerNoteDidFinishJumpEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> _noteDidPassJumpThreeQuartersEvent = new LazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteWasCutEvent> _noteWasCutEvent = new LazyCopyHashSet<INoteControllerNoteWasCutEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteWasMissedEvent> _noteWasMissedEvent = new LazyCopyHashSet<INoteControllerNoteWasMissedEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> _noteDidStartDissolvingEvent = new LazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent>();

	private readonly LazyCopyHashSet<INoteControllerNoteDidDissolveEvent> _noteDidDissolveEvent = new LazyCopyHashSet<INoteControllerNoteDidDissolveEvent>();

	private NoteData _noteData;

	private bool _dissolving;

	private float _uniformScale;
}
