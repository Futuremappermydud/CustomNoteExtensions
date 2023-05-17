using BeatSaberMarkupLanguage.FloatingScreen;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using Quaternion = UnityEngine.Quaternion;
using BeatSaberMarkupLanguage;
using UnityEngine.UI;

namespace CustomNoteExtensions.Services
{
	internal class LargePromptService : MonoBehaviour
	{
		[Inject]
		BpmController bpmController;
		private TextMeshProUGUI text;
		private Vector3[] positons = new Vector3[] { new Vector3(-100f, 1.9f, 5f), new Vector3(0f, 1.9f, 5f), new Vector3(100f, 1.9f, 5f) };
		private float length;

		void Awake()
		{
			Initialize();
		}

		void Initialize()
		{
			if (text != null) return;

			var floatingScreen = FloatingScreen.CreateFloatingScreen(new Vector2(250f, 50f), false, new Vector3(0f, 1.9f, 5f), Quaternion.identity);

			text = BeatSaberUI.CreateText(floatingScreen.transform as RectTransform, "!Display!", new Vector2(0f, 0f));
			text.alpha = 0f;
			text.fontSize = 13f;
			text.alignment = TextAlignmentOptions.Center;
			var fitter = text.gameObject.AddComponent<ContentSizeFitter>();
			fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		}
		public void Prompt(string toDisplay, float length, Color color, float delay)
		{
			text.text = toDisplay;
			text.color = color;
			this.length = length;
			text.alpha = 0f;
			StopAllCoroutines();
			StartCoroutine(Prompt(delay));
			StartCoroutine(Beat());
		}
		float easing(float x)
		{
			if (x < 0.5f)
			{
				return (4f * x) * ((2f * Mathf.Pow(x, 2f)) - (2f * x) + 1f) * (1f - x);
			}
			else
			{
				return 1f + (8 * Mathf.Pow(x, 4)) - (16 * Mathf.Pow(x, 3)) + (12 * Mathf.Pow(x, 2)) - (x * 4f);
			}
		}
		float alphaEasing(float x)
		{
			if (x < 0.5f)
			{
				return 2f * (1f - 8f * Mathf.Pow(1f - (x + 0.5f), 3));
			}
			else
			{
				return 2f * (1f + 8f * Mathf.Pow(1f - (x + 0.5f), 3));
			}
		}
		public Vector3 GetPosition(float time, float angle)
		{
			Vector3[] newPositions = new Vector3[positons.Length - 1];
			for (int i = 0; i < positons.Length - 1; i++)
			{
				newPositions[i] = Vector3.Lerp(positons[i], positons[i + 1], easing(time));
			}

			Vector3 position = Vector3.Lerp(newPositions[0], newPositions[1], easing(time));
			return position;
		}
		IEnumerator Prompt(float delay)
		{
			yield return new WaitForSeconds(delay);
			float angle;
			float value = 0.01f;
			while (value < length)
			{
				value += Time.deltaTime;
				angle = Mathf.Lerp(-20f, 20f, easing(value / length));
				text.transform.localPosition = GetPosition(value / length, angle);
				text.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

				text.alpha = Mathf.Clamp01(alphaEasing(value / length));
				yield return new WaitForEndOfFrame();
			}
		}
		private float bpm = 200f;
		private float beatLength => 60f / bpm;
		private float scaleMutliplier = 1f;
		IEnumerator Beat()
		{
			bpm = bpmController.currentBpm;
			float value = 0.01f;
			while (value < beatLength)
			{
				value += Time.deltaTime;
				float mult = Mathf.Lerp(1f, 1.15f, easing(value / beatLength));
				scaleMutliplier = Mathf.Lerp(scaleMutliplier, mult, 0.3f);
				text.transform.localScale = Vector3.one * scaleMutliplier;

				yield return new WaitForEndOfFrame();
			}

			StartCoroutine(Beat());
		}
	}
}
