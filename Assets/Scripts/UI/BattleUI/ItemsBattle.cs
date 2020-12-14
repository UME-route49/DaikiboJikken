using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemsBattle : MonoBehaviour {
    //TODO : Add the number of items in the list  Ex :  potion x2 

    /// The toggle to duplicate
    public GameObject ToggleToDuplicate;
    /// The content panel
    public Transform ContentPanel;
    //public Toggle useActionToggle;
    /// The item description
    public Text ItemDescription;
    /// The selected toggle
    private Toggle selectedToggle;
    /// The logic game object
    private GameObject logicGameObject ;

    void Start () {
		ClearItemList ();
		PopulateList ();
		logicGameObject  = GameObject.FindGameObjectsWithTag(Settings.Logic).FirstOrDefault();

	}

    /// Populates the list.
    void PopulateList () {
	
		Contract.Requires<UnassignedReferenceException> (BattlePanels.selectedCharacter != null);

		foreach (var item in Main.ItemList) {
			GameObject newToggle = Instantiate (ToggleToDuplicate) as GameObject;
			ItemsUI toggle = newToggle.GetComponent <ItemsUI> ();
			toggle.Name.text = item.Name;
			toggle.Toggle.isOn = false;
			newToggle.SetActive(true);
			newToggle.transform.SetParent( ContentPanel.transform);
			newToggle.transform.localScale= Vector3.one;
			newToggle.transform.position= Vector3.one;
			if(item.EquipementType != EnumEquipmentType.Usable) 
				newToggle.GetComponent<Toggle>().interactable = false ;
		}
	}


    /// This procedure check the resume toggle control and displays equips canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void ToggleSelectAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
			toggle.group.NotifyToggleOn(toggle);
			selectedToggle = toggle;
			//ItemsUI toggleItem = selectedToggle.GetComponent <ItemsUI> ();
			//var itemDatas = Main.ItemList.Where(w =>w.Name == toggleItem.Name.text).FirstOrDefault();
			
			//BattlePanels.SelectedCharacter.HP += itemDatas.HealthPoint;
			//BattlePanels.SelectedCharacter.MP += itemDatas.Mana;
			//BattlePanels.SelectedItem = itemDatas;
			//Main.ItemList.Remove(Main.ItemList.Where(w =>w.Name == toggleItem.Name.text).FirstOrDefault());
			//if (logicGameObject) {
			//	//logicGameObject.BroadcastMessage("ItemAction");	
			//}
		}
		else if (!toggle.isOn) {
		}
	}

   

    /// <summary>
    /// This procedure clear all the items in the list
    /// </summary>
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

    /// <summary>
    /// Deselects the menus toggles.
    /// </summary>
    public void DeselectMenusToggles()
	{
		if(selectedToggle)
        {
            selectedToggle.isOn = false;
        }

    }

}

