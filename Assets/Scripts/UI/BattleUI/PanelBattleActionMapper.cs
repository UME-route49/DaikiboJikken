using UnityEngine;
using System;

//Without serialisation this item will not be displayed
/// Struct PanelBattleActionMapper
[Serializable]
public struct PanelBattleActionMapper
{
    /// The battle action
    public EnumBattleAction BattleAction;
    /// The panel
    public GameObject Panel;
}