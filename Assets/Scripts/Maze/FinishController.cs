using UnityEngine;
using System.Collections;

public class FinishController : MonoBehaviour {
	public float minLightSize = 1.0f;
	public float maxLightSize = 4.0f;
	public float blinkInterval = 1.0f;

	Light blinkingLight;
	private IEnumerator currentLightSizeLerpCoroutine;

	// Use this for initialization
	void Start () {
		setReferences();
		resetLightSize();
		beginBlinkingCoroutine();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void setReferences () {
		blinkingLight = GetComponentInChildren<Light>();
	}


	private void resetLightSize () {
		blinkingLight.range = minLightSize;	
	}

	private void stopActiveLightSizeLerpCoroutine () {
		if (currentLightSizeLerpCoroutine != null) {
			StopCoroutine(currentLightSizeLerpCoroutine);
		}
	}
	private void beginBlinkingCoroutine () {
		stopActiveLightSizeLerpCoroutine();

		currentLightSizeLerpCoroutine = PulseLightSize(blinkingLight, 
		                                               minLightSize, 
		                                               maxLightSize, 
		                                               blinkInterval, 
		                                               true);

		StartCoroutine(currentLightSizeLerpCoroutine);
	}

	private IEnumerator PulseLightSize (Light light, float minSize, float maxSize, float time, bool repeating = false) {
		light.range = minSize;
		float timer = 0;
		float frameRate = Time.frameCount/Time.time;
		float step = (maxSize - minSize)/frameRate;
		while (timer < time/2.0f) {
			light.range += step;
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		light.range = maxSize;

		while (timer < time) {
			light.range -= step;
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		light.range = minSize;

		if (repeating) {
			beginBlinkingCoroutine();
		}
	}
}
