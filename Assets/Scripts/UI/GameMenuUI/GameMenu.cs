using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    /// The input locked
    private bool inputLocked =false;

    /// The action panels
    public PanelActionMapper[] ActionPanels;
    /// The player name
    public Text PlayerName;
    /// The played time
    public Text PlayedTime;
    /// The steps
    public Text Steps;
    /// The gold
    public Text Gold;

    /// <summary>
    /// The selected character
    /// </summary>
    public static CharactersData SelectedCharacter ;

    /// <summary>
    /// The selected character
    /// </summary>
    public Toggle TeamToggle;

    /// <summary>
    /// This is where the initialisations are made.
    /// </summary>
    void Awake()
    {
        GetComponent<Canvas>().enabled = false;
		GetComponent<CanvasGroup> ().interactable = false;
		SelectedCharacter = Main.CharacterList.FirstOrDefault ();
	
    }

    /// <summary>
    /// This procedure display or hide the menu and also pause the game
    /// </summary>
    public void ShowHideMenu()
    {
        if (!IsInputLocked())
        {
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
			GetComponent<CanvasGroup>().interactable = !GetComponent<CanvasGroup>().interactable;
            LockInput();
            if (Main.IsGamePaused) {  Main.PauseGame(false); }
            else
            {
                DisplayPanel(EnumWorldMenudAction.Team);
                if (TeamToggle) TeamToggle.isOn = true;
                Main.PauseGame(true); }
            SoundManager.UISound();
        }
 
    }
  

    /// <summary>
    /// This procedure unlock the input
    /// </summary>
    void UnlockInput()
    {
        inputLocked = false;
    }


    /// <summary>
    /// This procedure lock the input
    /// </summary>
    void LockInput()
    {
        inputLocked = true;
        //Invoke("UnlockInput",inputlockingTime);
		Invoker.InvokeDelayed(UnlockInput, Settings.InputlockingTime);
    }


    /// <summary>
    /// This procedure check state of the input
    /// <returns>State of the inputLocked variable</returns>
    /// </summary>
    /// <returns><c>true</c> if [is input locked]; otherwise, <c>false</c>.</returns>
    public bool IsInputLocked()
    {
        return inputLocked;
    }


    /// <summary>
    /// This procedure check the resume toggle control and call the ShowHideMenu() procedure
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleResumeAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();

        if (toggle.isOn)
        {
            ShowHideMenu();
           
        }
    }


    /// <summary>
    /// This procedure check the resume toggle control and load the first scene loaded in the game
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleExitAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
        Main.PauseGame(false);
        if (toggle.isOn)
            SceneManager.LoadScene(Settings.LoaderScene);
    }


    /// <summary>
    /// This procedure check the resume toggle control and displays items canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleItemsAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn)
        {   DisplayPanel(EnumWorldMenudAction.Items);
           
        }
    }


    /// <summary>
    /// This procedure check the resume toggle control and displays spells canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleSpellsAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn)
        { DisplayPanel(EnumWorldMenudAction.Spells);
           
        }
    }

    /// <summary>
    /// This procedure check the resume toggle control and displays equips canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleEquipAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn)
        {  DisplayPanel(EnumWorldMenudAction.Equip);
            
        }
    }


    /// <summary>
    /// This procedure check the resume toggle control and displays status canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleTeamAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn)
        { DisplayPanel(EnumWorldMenudAction.Team);
           
        }
    }


    /// <summary>
    /// This procedure check the resume toggle control and displays configs canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleConfigAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn)
            DisplayPanel(EnumWorldMenudAction.Config);
    }


    /// <summary>
    /// This procedure check the resume toggle control and fired the save action
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="gameObject">The game object.</param>
    public void ToggleSaveAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
        Main.CurrentScene = SceneManager.GetActiveScene().name;
		if (toggle.isOn)
        { Main.Save ();
           
        }
    }

    /// <summary>
    /// This procedure check the resume toggle control and call the ShowHideMenu() procedure
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="action">The action.</param>
    void DisplayPanel(EnumWorldMenudAction action)
    {
		foreach (PanelActionMapper row in ActionPanels)
        {
            if(row.Panel != null) {

				if (row.MenuAction == action){
					row.Panel.SetActive(true);
					row.Panel.SendMessage("Start"); 
                    row.Panel.BroadcastMessage("LoadCharactersAbilities");
                }
				else  row.Panel.SetActive(false);
			}
        }
   
    }
   
}
 
