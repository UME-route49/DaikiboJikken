using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Holoville.HOTween;
using System.Linq;
using System;

public class BattlePanels : MonoBehaviour {

    /// The action panels
    public PanelBattleActionMapper[] ActionPanels;
    /// The selected toggle
    public Toggle SelectedToggle;
    /// The selected character
    public static CharactersData SelectedCharacter ;
    /// The selected weapon
    public static ItemsData SelectedWeapon;
    /// The selected spell
    public static SpellsData SelectedSpell ;
    /// The selected item
    public static ItemsData SelectedItem;
    /// The fade out time
    public float FadeOutTime=2.5f;
    
    //The fadeIn animation time
    /// The log text
    public Text logText;
    /// The drop text
    public Text DropText;
    /// The pop up
    public Text PopUp;
    /// The drop menu
    public GameObject DropMenu;
    /// The decision menu
    public GameObject DecisionMenu;
    /// The action menu
    public GameObject ActionMenu;
    /// The logic game object
    private GameObject logicGameObject ;

    /// Starts this instance.
    void Start()
	{
		SelectedWeapon = null;
		SelectedSpell = null;
		SelectedItem = null;
	}

    /// Awakes this instance.
    void Awake ()
	{
		logicGameObject = GameObject.FindGameObjectsWithTag(Settings.Logic).FirstOrDefault();
		ToggleFightAction (SelectedToggle);
		SelectedCharacter = Main.CharacterList [0];
	}


    /// This procedure use the selected item
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void ToggleFightAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException>(toggle != null);
		Contract.Requires<UnassignedReferenceException>(SelectedToggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
            SoundManager.UISound();
            SelectedToggle = toggle;
			DisplayPanel (EnumBattleAction.Weapon);
		}
	}

    /// This procedure use the selected item
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void ToggleMagicAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
		Contract.Requires<UnassignedReferenceException> (SelectedToggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
			SelectedToggle = toggle;
			DisplayPanel (EnumBattleAction.Magic);
		}
	}

    /// This procedure use the selected item
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void ToggleItemAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
		Contract.Requires<UnassignedReferenceException> (SelectedToggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {

			SelectedToggle = toggle;
			DisplayPanel (EnumBattleAction.Item);
		}
	}

    /// This procedure use the selected item
    /// <param name="toggle">The toggle that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void TogglePassAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
		Contract.Requires<UnassignedReferenceException> (SelectedToggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {

			SelectedToggle = toggle;
			DisplayPanel (EnumBattleAction.None);

			if ( logicGameObject ) logicGameObject.BroadcastMessage ("PassAction");
		}
	}

    /// This procedure show or hide the different panels
    /// <param name="action">The action that correspond to the panel to display</param>
    /// <param name="action">The action.</param>
    void DisplayPanel(EnumBattleAction action)
	{
		foreach (PanelBattleActionMapper row in ActionPanels)
		{
			if(row.Panel != null) {

				if (row.BattleAction == action){
					row.Panel.SetActive(true);
					row.Panel.SendMessage("Start"); 
				}
				else  row.Panel.SetActive(false);
			}
		}

	}

    /// Fights this instance.
    void Fight ()
	{
		Debug.Log ("Fight");
		SendMessageUpwards("DisplayPanel",EnumBattleAction.Weapon);
	}

    /// Magics this instance.
    void Magic ()
	{
		Debug.Log ("Magic");
		SendMessageUpwards("DisplayPanel",EnumBattleAction.Magic);
	}

    /// Items this instance.
    void Item ()
	{
		Debug.Log ("Item");
		SendMessageUpwards("DisplayPanel",EnumBattleAction.Item);
	}

    /// Logs the text.
    /// <param name="text">The text.</param>
    void LogText (string text)
	{
		Debug.Log ("Loging"+text);
		logText.text = text;
	}

    /// Shows the drop menu.
    /// <param name="text">The text.</param>
    public void ShowDropMenu(string text)
	{
		DropMenu.SetActive (true);
		DropText.text = text;
		float time = 0.75f;

		Sequence actions = new Sequence(new SequenceParms());
		TweenParms parms = new TweenParms().Prop("localScale", DropMenu.transform.localScale*2f ).Ease(EaseType.EaseOutElastic);

		actions.Append(HOTween.To(DropMenu.transform, time, parms));

		actions.Play();
	}

    /// Hides the decision.
    public void HideDecision()
	{
		DecisionMenu.SetActive (false);
	}

    /// Shows the decision.
    public void ShowDecision()
	{
		DecisionMenu.SetActive (true);
	}


    /// Hides the action menu.
    public void HideActionMenu()
	{
		ActionMenu.SetActive (false);
	}

    /// Shows the action menu.
    public void ShowActionMenu()
	{
		ActionMenu.SetActive (true);
	}

    /// Declines the decision.
    public void DeclineDecision()
	{
		if (logicGameObject) logicGameObject.BroadcastMessage ("DeclineDecision");
	}

    /// Accepts the decision.
    public void AcceptDecision()
	{
		if (logicGameObject ) logicGameObject.BroadcastMessage ("AcceptDecision");
	}

    /// Shows the popup.
    /// <param name="parameters">The parameters.</param>
    public void ShowPopup(object[] parameters)
	{
		string text = (string)(parameters[0]);
		Vector3 position = (Vector3)parameters[1];
		PopUp.gameObject.SetActive (true);
		PopUp.text = text;
		PopUp.gameObject.transform.position = new Vector3(position.x, position.y, PopUp.gameObject.transform.position.z);
		float time = 0.75f;

		Sequence actions = new Sequence(new SequenceParms());
		TweenParms parms = new TweenParms().Prop("color", new Color(1.0f, 1.0f, 1.0f, 1.0f)).Ease(EaseType.EaseOutQuart);
		parms.Prop("fontSize", PopUp.fontSize * 2).Ease(EaseType.EaseOutBounce);

		TweenParms parmsReset = new TweenParms().Prop("color", new Color(1.0f, 1.0f, 1.0f, 0.0f)).Ease(EaseType.EaseOutQuart);
		parmsReset.Prop("fontSize", PopUp.fontSize ).Ease(EaseType.EaseOutQuart);

		actions.Append(HOTween.To(PopUp, time, parms));
		actions.Append(HOTween.To(PopUp, time, parmsReset));

		actions.Play();
	}

    /// Hides the popup.
    public void HidePopup()
	{
		PopUp.gameObject.SetActive (false);
		PopUp.text = string.Empty;
	}

    /// Ends the battle.
    public void EndBattle()
	{
		if ( logicGameObject ) logicGameObject.BroadcastMessage ("EndBattle");
	}
}
