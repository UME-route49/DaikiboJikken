using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using System.Linq;

public class HomeMenu : MonoBehaviour {

    /// The newgame toggle
    public Toggle NewgameToggle;
    /// The scene to load for new game
    public String SceneToLoadForNewGame;

    FadeOut fadeOut;
   
    // Use this for initialization
    void Start() 
    {
		Datas.PopulateDatas ();
        SoundManager.TitleMusic();
        fadeOut = FindObjectOfType<FadeOut>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NewgameChoice();
        }
    }

    private void NewgameChoice()
    {
        SoundManager.UISound();
        Main.CurrentScene = SceneToLoadForNewGame;
        fadeOut.StartCoroutine("Fadeout", SceneToLoadForNewGame);
    }

    /// This procedure check the no toggle control and send action to the action variable
    public void NewgameToggleChoice(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
            gameObject.SetActive (false); 
			Main.CurrentScene = SceneToLoadForNewGame;
			SceneManager.LoadScene (SceneToLoadForNewGame);
		}
	}

    /// This procedure check the no toggle control and send action to the action variable
    public void ContinueToggleChoice(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
            gameObject.SetActive (false); 
			Main.Load ();
		}
	}
}
