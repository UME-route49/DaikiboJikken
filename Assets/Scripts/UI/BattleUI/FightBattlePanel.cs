using UnityEngine;
using System.Collections;

public class FightBattlePanel : MonoBehaviour {

    /// The actual battle action
    public EnumFightMenuAction ActualBattleAction   ;

    /// Awakes this instance.
    void Awake ()
	{
		SendMessage (string.Format ("{0}", ActualBattleAction));
	}

    /// Changes the enum character action.
    /// <param name="action">The action.</param>
    public void ChangeEnumCharacterAction (EnumFightMenuAction action)
	{ 
		if (ActualBattleAction != action) {
			ActualBattleAction = action;
			SendMessage (string.Format ("{0}", ActualBattleAction));
		}
	}

    /// Swords action.
    void Sword ()
	{	
        SendMessageUpwards("DisplayPanel",EnumFightMenuAction.Sword);
	}
	

}
