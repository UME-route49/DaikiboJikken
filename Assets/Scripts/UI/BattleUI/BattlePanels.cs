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
    /// The action menu
    public GameObject ActionMenu;
    /// The logic game object
    private GameObject logicGameObject ;

	private int parentCounter = 0;
	private int childCounter = 1;

	private string isActivePanel = "ParentPanel";

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
		//ToggleFightAction (SelectedToggle);
		SelectedCharacter = Main.CharacterList [0];
	}

    private void Update()
    {
		if (isActivePanel == "ParentPanel")
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (parentCounter <= 0) return;

				parentCounter--;
				ActionPanels[parentCounter].parentToggle.isOn = true;
				SoundManager.UISound();
				SelectedToggle = ActionPanels[parentCounter].parentToggle;
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				if (parentCounter >= ActionPanels.Length - 1) return;

				parentCounter++;
				ActionPanels[parentCounter].parentToggle.isOn = true;
				SoundManager.UISound();
				SelectedToggle = ActionPanels[parentCounter].parentToggle;
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				if (ActionPanels[parentCounter].contents == null) return;

				SoundManager.UISound();
				isActivePanel = "ChildPanel";
				ActionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>().isOn = true;
				SelectedToggle = ActionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>();
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if(ActionPanels[parentCounter].contents != null)
                {
					//
					SoundManager.UISound();
					isActivePanel = "ChildPanel";
					ActionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>().isOn = true;
					SelectedToggle = ActionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>();
					DisplayPanel(ActionPanels[parentCounter].BattleAction);
				}
				else
                {
					//ActionPanels[parentCounter].Panel.SendMessage("Start");
					logicGameObject.BroadcastMessage("Action", ActionPanels[parentCounter].BattleAction);
				}
				//Debug.Log(parentCounter);
			}
			else if (Input.GetKeyDown(KeyCode.C))
			{
				if (logicGameObject) logicGameObject.BroadcastMessage("PassAction");
			}
		}
		else if(isActivePanel == "ChildPanel")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
				if (childCounter <= 1) return;

				childCounter--;
				ActionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>().isOn = true;
				SoundManager.UISound();
				SelectedToggle = ActionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>();
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
			}
			else if(Input.GetKeyDown(KeyCode.S))
            {
				if (childCounter >= ActionPanels[parentCounter].contents.childCount - 1) return;

				childCounter++;
				ActionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>().isOn = true;
				SoundManager.UISound();
				SelectedToggle = ActionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>();
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.A))
            {
				SelectedToggle.isOn = false;
				SoundManager.UISound();
				isActivePanel = "ParentPanel";
				ActionPanels[parentCounter].parentToggle.isOn = true;
				SelectedToggle = ActionPanels[parentCounter].parentToggle;
				DisplayPanel(ActionPanels[parentCounter].BattleAction);
				childCounter = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (ActionPanels[parentCounter].BattleAction)
                {
					case EnumBattleAction.Magic:

						break;
                }
            }
        }
	}

    void DisplayPanel(EnumBattleAction action)
	{
		foreach (PanelBattleActionMapper row in ActionPanels)
		{
			if(row.Panel != null) {

				if (row.BattleAction == action){
					row.Panel.SetActive(true);
					//row.Panel.SendMessage("Start"); 
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

    void LogText (string text)
	{
		Debug.Log ("Loging"+text);
		logText.text = text;
	}

    public void ShowDropMenu(string text)
	{
		DropMenu.SetActive (true);
		DropText.text = text;
		float time = 0.75f;

		//Sequence actions = new Sequence(new SequenceParms());
		//TweenParms parms = new TweenParms().Prop("localScale", DropMenu.transform.localScale*2f ).Ease(EaseType.EaseOutElastic);
		//actions.Append(HOTween.To(DropMenu.transform, time, parms));
		//actions.Play();
	}

    /// Hides the action menu.
    public void HideActionMenu()
	{
		ActionMenu.SetActive (false);
		isActivePanel = "None";
	}

    /// Shows the action menu.
    public void ShowActionMenu()
	{
		ActionMenu.SetActive (true);
		isActivePanel = "ParentPanel";
		parentCounter = 0;
		childCounter = 1;
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
