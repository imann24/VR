using UnityEngine;
using System.Collections;

[RequireComponent (typeof(TextMesh))]
public class LerpFade3DText : MonoBehaviour {
	TextMesh textMesh;
	// Use this for initialization
	void Start () {
		establishReference();
		setStartingOpacity(0.25f);
		beginLerpingOpacity();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void establishReference () {
		textMesh = GetComponent<TextMesh>();
	}

	private void setStartingOpacity (float startingOpacity = 0.0f) {
		textMesh.color = Util.ChangeColorOpacty(textMesh.color, startingOpacity);
	}

	private void beginLerpingOpacity () {
		StartCoroutine(LerpTextMeshTransparency(textMesh, 5.0f, true, 0.25f));
	}
	
	private IEnumerator LerpTextMeshTransparency (TextMesh textMesh, 
	                                              float time, 
		                                          bool repeating = false,
	                                              float minOpacity = 0.0f, 
	                                              float maxOpacity = 1.0f) {


		float timer = 0;
		float frameRate = Time.frameCount/Time.time;
		float step = (maxOpacity - minOpacity)/frameRate;

		while (timer < time/2.0f) {
			textMesh.color = Util.ChangeColorOpacty(textMesh.color,
			                       textMesh.color.a + step);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		
		textMesh.color = Util.ChangeColorOpacty(textMesh.color, maxOpacity);
		
		while (timer < time) {
			textMesh.color = Util.ChangeColorOpacty(textMesh.color,
			                       textMesh.color.a - step);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		
		textMesh.color = Util.ChangeColorOpacty(textMesh.color, minOpacity);
		
		if (repeating) {
			beginLerpingOpacity();
		}
	}
}
