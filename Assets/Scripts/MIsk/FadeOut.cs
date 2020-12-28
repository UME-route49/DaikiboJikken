using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeOut : MonoBehaviour {

	/// The fade out time
	public float FadeOutTime = 1;

    // Update is called once per frame
    /// Starts this instance.
    void Start ()
	{
		StartCoroutine (Fadein());
	}

    // Animate the Logos with fadeIn and fadeOut effect
    /// Initializes this instance.
    IEnumerator Fadein()
	{    	
		// Set the texture so that it is the the size of the screen and covers it.
		transform.localScale= new Vector3( Screen.width, Screen.height,1);

		Sequence mySequence = DOTween.Sequence();
		Color oldColor = GetComponent<Image>().color;
		GetComponent<Image>().color = new Color (oldColor.r, oldColor.b, oldColor.g, 1f);
		mySequence.Append(GetComponent<Image>().DOColor(new Color(oldColor.r, oldColor.b, oldColor.g, 0f), FadeOutTime)).SetEase(Ease.InQuart);
		mySequence.Play ();

		yield return new WaitForSeconds (FadeOutTime);
	}

	IEnumerator Fadeout(string NextScene)
    {
		transform.localScale = new Vector3(Screen.width, Screen.height, 1);

		Sequence mySequence = DOTween.Sequence();
		Color oldColor = GetComponent<Image>().color;
		GetComponent<Image>().color = new Color(oldColor.r, oldColor.b, oldColor.g, 0f);
		mySequence.Append(GetComponent<Image>().DOColor(new Color(oldColor.r, oldColor.b, oldColor.g, 1f), FadeOutTime)).SetEase(Ease.InQuart);
		mySequence.Play();

		yield return new WaitForSeconds(FadeOutTime);

		SceneManager.LoadScene(NextScene);
	}
}
