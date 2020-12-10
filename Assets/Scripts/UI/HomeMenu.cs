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
   
    // Use this for initialization
    /// Starts this instance.
    void Start () {
		Datas.PopulateDatas ();
    }



    /// This procedure check the no toggle control and send action to the action variable
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
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
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
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
