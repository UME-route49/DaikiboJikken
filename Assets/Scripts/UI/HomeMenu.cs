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
    bool canControl = true;
   
    // Use this for initialization
    void Start() 
    {
		Datas.PopulateDatas ();
        SoundManager.TitleMusic();
        fadeOut = FindObjectOfType<FadeOut>();
        canControl = true;
    }

    private void Update()
    {
        if (!canControl) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            canControl = false;
            NewgameChoice();
        }
    }

    private void NewgameChoice()
    {
        SoundManager.StartSound();
        Main.CurrentScene = SceneToLoadForNewGame;
        fadeOut.StartCoroutine(fadeOut.Fadeout(SceneToLoadForNewGame, 5f));
    }

    /// This procedure check the no toggle control and send action to the action variable
    public void NewgameToggleChoice(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        SoundManager.StartSound();
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
        SoundManager.StartSound();
        if (toggle.isOn) {
            gameObject.SetActive (false); 
			Main.Load ();
		}
	}
}
