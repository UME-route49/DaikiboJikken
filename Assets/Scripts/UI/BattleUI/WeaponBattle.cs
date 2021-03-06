using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WeaponBattle : MonoBehaviour {
    //TODO : Add the number of items in the list  Ex :  potion x2 
    //Equipments texts
    /// The character number
    public int CharacterNumber = 1;
    /// The right hand text
    public Text RightHandText ;
    /// The right hand image
    public Image RightHandImage ;
    /// The selected character infos
    public bool SelectedCharacterInfos  = false  ;
    /// The weapon description
    public Text WeaponDescription;
    /// The weapon
    public Toggle Weapon;
    /// The logic game object
    private GameObject logicGameObject ;

    void Start () {
		//Weapon.isOn = false;
		if (BattlePanels.selectedCharacter  != null) LoadEquipements();
		logicGameObject  = GameObject.FindGameObjectsWithTag(Settings.Logic).FirstOrDefault();
	}

    /// This procedure display theequiped items
    public void LoadEquipements()
	{
		if (BattlePanels.selectedCharacter.RightHand != null) {
			if (RightHandText != null)
                RightHandText.text = BattlePanels.selectedCharacter.RightHand.Name;
        }
        else {
			if (RightHandImage != null)
            {
                RightHandImage.sprite = null;
            }

            if (RightHandText != null)
            {
                RightHandText.text = string.Empty;
            }
        }
    }

    /// This procedure check the resume toggle control and displays equips canvas
    /// <param name="gameObject">The gameobject that sent the action</param>
    /// <param name="toggle">The toggle.</param>
    public void ToggleSelectAction(Toggle toggle)
	{
		Contract.Requires<MissingComponentException> (toggle != null);
        //SoundManager.UISound();
        if (toggle.isOn) {
			ColorBlock cb = toggle.colors;
			cb.normalColor = Color.cyan;
			cb.highlightedColor = Color.cyan;
			toggle.colors = cb;
			if (BattlePanels.selectedCharacter.RightHand != null) {
				BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
				if (logicGameObject) {
					logicGameObject.BroadcastMessage("WeaponAction");	
				}
			}
		}
		else if (!toggle.isOn) {
			ColorBlock cb = toggle.colors;
			cb.normalColor = Color.white;
			cb.highlightedColor = Color.yellow;
			toggle.colors = cb;
		}
	}

    /// Deselects the menus toggles.
    public void DeselectMenusToggles()
	{
		if(Weapon)
        {
            Weapon.isOn = false;
        }
    }
}
