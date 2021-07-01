using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRandomColorEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public const float FADE_TIME = 0.1f;
	public static Color[ ] COLORS = new Color[ ] {
		new Color(255 / 255f, 229 / 255f, 102 / 255f), // Yellow
		new Color(211 / 255f, 255 / 255f, 102 / 255f), // Green
		new Color(102 / 255f, 255 / 255f, 255 / 255f), // Blue
		new Color(153 / 255f, 102 / 255f, 255 / 255f), // Purple
		new Color(255 / 255f, 102 / 255f, 102 / 255f), // Red
	};

	[Header(" --- UI Random Color Effect Class --- ")]
	[SerializeField] private Image image;

	private float fadeTimer = 0f;

	private void OnValidate ( ) {
		if (image == null) {
			image = GetComponent<Image>( );
		}
	}

	public void OnPointerEnter (PointerEventData eventData) {
		StopAllCoroutines( );
		StartCoroutine(IFadeToColor(Utils.Choose(COLORS)));
	}

	public void OnPointerExit (PointerEventData eventData) {
		StopAllCoroutines( );
		StartCoroutine(IFadeToColor(Color.white));
	}

	private IEnumerator IFadeToColor (Color toColor) {
		Color currentColor = image.color;

		fadeTimer = FADE_TIME - fadeTimer;
		while (fadeTimer < FADE_TIME) {
			fadeTimer += Time.unscaledDeltaTime;
			image.color = Color.Lerp(currentColor, toColor, fadeTimer / FADE_TIME);

			yield return null;
		}

		image.color = toColor;

		yield break;
	}
}
