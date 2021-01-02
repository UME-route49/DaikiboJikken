using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemsGameMenu : MonoBehaviour {
    /// The toggle to duplicate
    public GameObject ToggleToDuplicate;
    /// The content panel
    public Transform ContentPanel;
    /// The use action toggle
    public Toggle UseActionToggle;
    /// The item description
    public Text ItemDescription;
    /// The selected toggle
    private Toggle selectedToggle;

    /// Starts this instance.
    void Start () {
		ClearItemList ();
		PopulateList ();
	}

    /// Populates the list.
    void PopulateList () {
	
		Contract.Requires<UnassignedReferenceException> (GameMenu.SelectedCharacter != null);

		foreach (var item in Main.ItemList) {
			GameObject newToggle = Instantiate (ToggleToDuplicate) as GameObject;
			ItemsUI toggle = newToggle.GetComponent <ItemsUI> ();
			toggle.Name.text = item.Name;
			toggle.Toggle.isOn = false;
			newToggle.SetActive(true);
			newToggle.transform.SetParent( ContentPanel);
			newToggle.transform.localScale= Vector3.one;
			newToggle.transform.position= Vector3.one;
			if(item.EquipementType != EnumEquipmentType.Usable) 
				newToggle.GetComponent<Toggle>().interactable = false ;
		}
	}


    /// This procedure check the resume toggle control and displays equips canvas
    public void ToggleSelectAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
		Contract.Requires<MissingComponentException> (UseActionToggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
			UseActionToggle.Select ();
			//equipActionToggle.isOn = true;
			ColorBlock cb = toggle.colors;
			cb.normalColor = Color.cyan;
			cb.highlightedColor = Color.cyan;
			toggle.colors = cb;
			selectedToggle = toggle;
			ItemsUI toggleItem = selectedToggle.GetComponent <ItemsUI> ();
			var itemDatas=Main.ItemList.Where(w =>w.Name == toggleItem.Name.text).FirstOrDefault();
			ItemDescription.text =itemDatas.Description;
		
		}
		else if (!toggle.isOn) {
			//equipActionToggle.Select ();
			//equipActionToggle.isOn = true;
			ColorBlock cb = toggle.colors;
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.yellow;
			toggle.colors = cb;
		
		}
	}

    /// This procedure use the selected item
    public void ToggleUseAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
		Contract.Requires<UnassignedReferenceException> (selectedToggle != null);
		Contract.Requires<UnassignedReferenceException> (GameMenu.SelectedCharacter != null);
        SoundManager.UISound();
        if (toggle.isOn) {
			ItemsUI toggleItem = selectedToggle.GetComponent <ItemsUI> ();
			var x = Main.ItemList.Where (w => w.Name == toggleItem.Name.text).FirstOrDefault ();
			GameMenu.SelectedCharacter.HP = Mathf.Clamp (GameMenu.SelectedCharacter.HP + x.HealthPoint, GameMenu.SelectedCharacter.HP, GameMenu.SelectedCharacter.MaxHP);
			GameMenu.SelectedCharacter.MP = Mathf.Clamp(GameMenu.SelectedCharacter.MP + x.Mana, GameMenu.SelectedCharacter.MP, GameMenu.SelectedCharacter.MaxMP) ;
			Main.ItemList.Remove(Main.ItemList.Where(w =>w.Name == toggleItem.Name.text ).FirstOrDefault());
			SendMessage("LoadCharactersAbilities");
			ClearItemList();
			PopulateList ();
		}
	}


    /// This procedure clear all the items in the list
    public void ClearItemList()
	{
		Contract.Requires<UnassignedReferenceException> (ContentPanel != null);

		foreach (Transform child in ContentPanel.transform) {
			if(child.gameObject.activeInHierarchy)
            {
                GameObject.Destroy(child.gameObject);
            }

        }
    }
}

