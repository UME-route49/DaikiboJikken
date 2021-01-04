using UnityEngine;

public class EnemyCharacterDatas: MonoBehaviour
{

    //Character abilities
    /// The hp
    public int HP= default(int);
    /// The mp
    public int MP= default(int);

    /// The attack
    public int Attack= default(int);
    /// The defense
    public int Defense= default(int);
    /// The magic
    public int Magic= default(int);
    /// The magic defense
    public int MagicDefense= default(int);
    /// The level
    public int Level= default(int);

    //The maximum amount of mana that the charater can reach
    /// The maximum hp
    public int MaxHP= default(int);
    /// The maximum mp
    public int MaxMP= default(int);

    //Gained XP
    /// The xp
    public int XP= default(int);

    public EnumPanelPosition Foward;

    public struct Action
    {
        string Name;
        int Damage;
        bool[] panel;
        EnumBattleAction Type;
    }

    public Action[] actions;
}


