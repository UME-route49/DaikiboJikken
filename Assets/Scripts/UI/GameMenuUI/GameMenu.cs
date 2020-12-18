using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using DG.Tweening;

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

    /// The selected character
    public static CharactersData SelectedCharacter ;

    /// The selected character
    public Toggle TeamToggle;

    void Awake()
    {
        GetComponent<Canvas>().enabled = false;
		GetComponent<CanvasGroup> ().interactable = false;
		SelectedCharacter = Main.CharacterList.FirstOrDefault ();
    }

    void Update() {}

    /// This procedure display or hide the menu and also pause the game
    public void ShowHideMenu()
    {
        if (!IsInputLocked())
        {
            //HOTween.Complete ();
            GetComponent<Canvas>().enabled = !GetComponent<Canvas>().enabled;
			GetComponent<CanvasGroup>().interactable = !GetComponent<CanvasGroup>().interactable;
            LockInput();
            if (Main.IsGamePaused) Main.PauseGame(false);
            else
            {
                DisplayPanel(EnumWorldMenudAction.Team);
                if (TeamToggle) TeamToggle.isOn = true;
                Main.PauseGame(true); 
            }
            SoundManager.UISound();
        }
    }
  
    /// This procedure unlock the input
    void UnlockInput()
    {
        inputLocked = false;
    }

    /// This procedure lock the input
    void LockInput()
    {
        inputLocked = true;
        //Invoke("UnlockInput",inputlockingTime);
		Invoker.InvokeDelayed(UnlockInput, Settings.InputlockingTime);
    }

    /// This procedure check state of the input
    public bool IsInputLocked()
    {
        return inputLocked;
    }

    /// This procedure check the resume toggle control and call the ShowHideMenu() procedure
    public void ToggleResumeAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();

        if (toggle.isOn) ShowHideMenu();
    }

    /// This procedure check the resume toggle control and load the first scene loaded in the game
    public void ToggleExitAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
        Main.PauseGame(false);
        if (toggle.isOn) SceneManager.LoadScene(Settings.LoaderScene);
    }

    /// This procedure check the resume toggle control and displays items canvas
    public void ToggleItemsAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn) DisplayPanel(EnumWorldMenudAction.Items);
    }

    /// This procedure check the resume toggle control and displays spells canvas
    public void ToggleSpellsAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn) DisplayPanel(EnumWorldMenudAction.Spells);
    }

    /// This procedure check the resume toggle control and displays equips canvas
    public void ToggleEquipAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn) DisplayPanel(EnumWorldMenudAction.Equip);
    }


    /// This procedure check the resume toggle control and displays status canvas
    public void ToggleTeamAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn) DisplayPanel(EnumWorldMenudAction.Team);
    }

    /// This procedure check the resume toggle control and displays configs canvas
    public void ToggleConfigAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
 
        if (toggle.isOn) DisplayPanel(EnumWorldMenudAction.Config);
    }

    /// This procedure check the resume toggle control and fired the save action
    public void ToggleSaveAction(GameObject gameObject)
    {
        SoundManager.UISound();
        var toggle = gameObject.GetComponent<Toggle>();
        Main.CurrentScene = SceneManager.GetActiveScene().name;
		if (toggle.isOn) Main.Save ();
    }

    /// This procedure check the resume toggle control and call the ShowHideMenu() procedure
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