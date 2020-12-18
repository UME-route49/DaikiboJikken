using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// This class is used to animate the splatch screen
public class SplatchScreen : MonoBehaviour
{
    /// The fade out time
    public float FadeOutTime=2.5f;
    /// The next scene
    public string NextScene;//The next scene to load

    /// Starts this instance.
    void Start ()
	{
		StartCoroutine (Init ());
	}



    // Animate the Logos with fadeIn and fadeOut effect
    /// Initializes this instance.
    IEnumerator Init ()
	{    
		Sequence mySequence = DOTween.Sequence();

		Color oldColor = GetComponent<Image>().color;

		GetComponent<Image>().color = new Color (oldColor.r, oldColor.b, oldColor.g, 1f);

		mySequence.Append(GetComponent<Image>().DOColor(new Color(oldColor.r, oldColor.b, oldColor.g, 0f), FadeOutTime)).SetEase(Ease.InQuart);
		mySequence.Play ();

		yield return new WaitForSeconds(FadeOutTime);

		SceneManager.LoadScene(NextScene);
	}



	


}