using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ItemsBattle : MonoBehaviour {
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