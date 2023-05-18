using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CustomNoteExtensions.Services
{
	public class NoteTypeAudioService : MonoBehaviour
	{
		public static NoteTypeAudioService Instance;
		private AudioSource audioSource;
		public void Start()
		{
			Instance = this;
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		public void PlayClip(AudioClip clip, float delay)
		{
			StartCoroutine(PlayDelayedClip(clip, delay));
		}
		IEnumerator PlayDelayedClip(AudioClip clip, float delay)
		{
			yield return new WaitForSeconds(delay);
			audioSource.PlayOneShot(clip);
		}
	}
}
