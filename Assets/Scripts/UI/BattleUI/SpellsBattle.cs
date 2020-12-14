using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SpellsBattle : MonoBehaviour {

    /// The toggle to duplicate
    public GameObject ToggleToDuplicate;
    /// The content panel
    public Transform ContentPanel;
    /// The spell description
    public Text SpellDescription;
    /// The selected toggle
    private Toggle selectedToggle;
    /// The logic game object
    private GameObject logicGameObject ;


    void Start () {
		ClearItemList ();
		PopulateList ();
		logicGameObject  = GameObject.FindGameObjectsWithTag(Settings.Logic).FirstOrDefault();
	}

    void PopulateList () {
	
		Contract.Requires<UnassignedReferenceException> (BattlePanels.selectedCharacter != null);

		foreach (var spell in BattlePanels.selectedCharacter.SpellsList) {
			GameObject newToggle = Instantiate (ToggleToDuplicate) as GameObject;
			ItemsUI toggle = newToggle.GetComponent <ItemsUI> ();
			toggle.Name.text = spell.Name;
			//toggle.Icon.sprite =Resources.Load <Sprite> (Settings.IconsPaths + spell.PicturesName); ;
			toggle.Toggle.isOn = false;
			newToggle.SetActive(true);
			newToggle.transform.SetParent(ContentPanel.transform);
			newToggle.transform.localScale= Vector3.one;
			newToggle.transform.position= Vector3.one;
			if (BattlePanels.selectedCharacter.MP < spell.ManaAmount)
				toggle.Toggle.interactable = false;
			else 
				toggle.Toggle.interactable = true;
		}
	}


    /// <summary>
    /// This procedure check the resume toggle control and displays equips canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// </summary>
    /// <param name="toggle">The toggle.</param>
    public void ToggleSelectAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        SoundManager.UISound();
        if (toggle.isOn) {
			selectedToggle = toggle;
			ItemsUI toggleItem = selectedToggle.GetComponent <ItemsUI> ();
			var itemDatas = BattlePanels.selectedCharacter.SpellsList.Where(w =>w.Name == toggleItem.Name.text).FirstOrDefault();
			BattlePanels.selectedSpell = itemDatas;
			if (logicGameObject) {
				//logicGameObject.BroadcastMessage("MagicAction");	
			}
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

