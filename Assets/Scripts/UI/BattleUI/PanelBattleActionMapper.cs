using UnityEngine;
using System;
using UnityEngine.UI;

//Without serialisation this item will not be displayed
/// Struct PanelBattleActionMapper
[Serializable]
public struct PanelBattleActionMapper
{
    /// The battle action
    public EnumBattleAction BattleAction;
    /// The panel
    public GameObject Panel;

    public Toggle parentToggle;

    public Transform contents;
}