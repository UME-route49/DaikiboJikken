using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;
using DG.Tweening;

public class BattlePanels : MonoBehaviour {

    /// The action panels
    public PanelBattleActionMapper[] actionPanels;
    /// The selected toggle
    public Toggle selectedToggle;
    /// The selected character
    public static CharactersData selectedCharacter ;
    /// The selected weapon
    public static ItemsData selectedWeapon;
    /// The selected spell
    public static SpellsData selectedSpell ;
    /// The selected item
    public static ItemsData selectedItem;
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
		selectedWeapon = null;
		selectedSpell = null;
		selectedItem = null;
	}

    /// Awakes this instance.
    void Awake ()
	{
		logicGameObject = GameObject.FindGameObjectsWithTag(Settings.Logic).FirstOrDefault();
		selectedCharacter = Main.CharacterList [0];
	}

    private void Update()
    {
		if (isActivePanel == "ParentPanel")
		{
			if (Input.GetKeyDown(KeyCode.W))
			{
				if (parentCounter <= 0) return;

				parentCounter--;
				actionPanels[parentCounter].parentToggle.isOn = true;
				SoundManager.UISound();
				selectedToggle = actionPanels[parentCounter].parentToggle;
				DisplayPanel(actionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				if (parentCounter >= actionPanels.Length - 1) return;

				parentCounter++;
				actionPanels[parentCounter].parentToggle.isOn = true;
				SoundManager.UISound();
				selectedToggle = actionPanels[parentCounter].parentToggle;
				DisplayPanel(actionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.D))
			{
				if (actionPanels[parentCounter].contents == null) return;
				if (actionPanels[parentCounter].contents.childCount < 2) return;

				SoundManager.UISound();
				isActivePanel = "ChildPanel";
				actionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>().isOn = true;
				selectedToggle = actionPanels[parentCounter].contents.GetChild(1).GetComponent<Toggle>();
				DisplayPanel(actionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				if (actionPanels[parentCounter].contents != null) return;

				logicGameObject.BroadcastMessage("Action", actionPanels[parentCounter].BattleAction);
				parentCounter = 0;
				actionPanels[parentCounter].parentToggle.isOn = true;
				selectedToggle = actionPanels[parentCounter].parentToggle;
			}
		}
		else if(isActivePanel == "ChildPanel")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
				if (childCounter <= 1) return;

				childCounter--;
				actionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>().isOn = true;
				SoundManager.UISound();
				selectedToggle = actionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>();
				DisplayPanel(actionPanels[parentCounter].BattleAction);
			}
			else if(Input.GetKeyDown(KeyCode.S))
            {
				if (childCounter >= actionPanels[parentCounter].contents.childCount - 1) return;

				childCounter++;
				actionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>().isOn = true;
				SoundManager.UISound();
				selectedToggle = actionPanels[parentCounter].contents.GetChild(childCounter).GetComponent<Toggle>();
				DisplayPanel(actionPanels[parentCounter].BattleAction);
			}
			else if (Input.GetKeyDown(KeyCode.A))
            {
				selectedToggle.isOn = false;
				SoundManager.UISound();
				isActivePanel = "ParentPanel";
				actionPanels[parentCounter].parentToggle.isOn = true;
				selectedToggle = actionPanels[parentCounter].parentToggle;
				DisplayPanel(actionPanels[parentCounter].BattleAction);
				childCounter = 1;
			}
			else if (Input.GetKeyDown(KeyCode.Space))
            {
				ItemsUI toggleItem = selectedToggle.GetComponent<ItemsUI>();
                switch (actionPanels[parentCounter].BattleAction)
                {
					case EnumBattleAction.Magic:
						var spellDatas = BattlePanels.selectedCharacter.SpellsList.Where(w => w.Name == toggleItem.Name.text).FirstOrDefault();
						BattlePanels.selectedSpell = spellDatas;
						break;
					case EnumBattleAction.Item:
						var itemDatas = Main.ItemList.Where(w => w.Name == toggleItem.Name.text).FirstOrDefault();
						BattlePanels.selectedCharacter.HP += itemDatas.HealthPoint;
						BattlePanels.selectedCharacter.MP += itemDatas.Mana;
						BattlePanels.selectedItem = itemDatas;
						Main.ItemList.Remove(Main.ItemList.Where(w => w.Name == toggleItem.Name.text).FirstOrDefault());
						Destroy(selectedToggle.gameObject);
						break;
                }
				selectedToggle.isOn = false;
				logicGameObject.BroadcastMessage("Action", actionPanels[parentCounter].BattleAction);
            }
        }
	}

    void DisplayPanel(EnumBattleAction action)
	{
		foreach (PanelBattleActionMapper row in actionPanels)
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

		Sequence actions = DOTween.Sequence();
		actions.Append(DropMenu.transform.DOScale(DropMenu.transform.localScale * 2f, time)).SetEase(Ease.OutElastic);
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
		actionPanels[parentCounter].parentToggle.isOn = true;
		selectedToggle = actionPanels[parentCounter].parentToggle;
		DisplayPanel(actionPanels[parentCounter].BattleAction);
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

		Sequence actions = DOTween.Sequence();
		actions.Append(PopUp.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), time)).SetEase(Ease.OutQuart);
		//actions.Join(PopUp.DOFontsize(PopUp.fontSize * 2, time)).setEase(Ease.OutBounce);
		actions.Append(PopUp.DOColor(new Color(1.0f, 1.0f, 1.0f, 0.0f), time)).SetEase(Ease.OutQuart);
		//actions.Join(PopUp.DOFontsize(PopUp.fontSize, time)).setEase(Ease.OutQuart);
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
