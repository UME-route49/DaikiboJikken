using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FadeOut : MonoBehaviour {

    void Start ()
	{
		StartCoroutine (Fadein(1f));
	}

    IEnumerator Fadein(float time)
	{
		// Set the texture so that it is the the size of the screen and covers it.
		transform.localScale= new Vector3( Screen.width, Screen.height,1);

		Sequence mySequence = DOTween.Sequence();
		Color oldColor = GetComponent<Image>().color;
		GetComponent<Image>().color = new Color (oldColor.r, oldColor.b, oldColor.g, 1f);
		mySequence.Append(GetComponent<Image>().DOColor(new Color(oldColor.r, oldColor.b, oldColor.g, 0f), time)).SetEase(Ease.InQuart);
		mySequence.Play ();

		yield return new WaitForSeconds (time);
	}

	public IEnumerator Fadeout(string NextScene, float time)
    {
		transform.localScale = new Vector3(Screen.width, Screen.height, 1);

		Sequence mySequence = DOTween.Sequence();
		Color oldColor = GetComponent<Image>().color;
		GetComponent<Image>().color = new Color(oldColor.r, oldColor.b, oldColor.g, 0f);
		mySequence.Append(GetComponent<Image>().DOColor(new Color(oldColor.r, oldColor.b, oldColor.g, 1f), time)).SetEase(Ease.InQuart);
		mySequence.Play();

		yield return new WaitForSeconds(time);

		SceneManager.LoadScene(NextScene);
	}
}
