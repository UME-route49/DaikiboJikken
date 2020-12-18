public enum EnumBattleAction
{
    /// <summary>
    /// The none
    /// </summary>
    None,
    /// <summary>
    /// The weapon
    /// </summary>
    Weapon,
    /// <summary>
    /// The magic
    /// </summary>
    Magic,
    /// <summary>
    /// The item
    /// </summary>
    Item,
    /// <summary>
    /// The pass
    /// </summary>
    Pass
}

public enum EnumBattleState
{
    /// <summary>
    /// The none
    /// </summary>
    None,
    /// <summary>
    /// The beginning
    /// </summary>
    Beginning,
    /// <summary>
    /// The player turn
    /// </summary>
    PlayerTurn,
    /// <summary>
    /// The selecting target
    /// </summary>
    SelectingTarget,
    /// <summary>
    /// The selected target
    /// </summary>
    SelectedTarget,
    /// <summary>
    /// The enemy turn
    /// </summary>
    EnemyTurn,
    /// <summary>
    /// The player won
    /// </summary>
    PlayerWon,
    /// <summary>
    /// The enemy won
    /// </summary>
    EnemyWon,
    /// <summary>
    /// The end battle
    /// </summary>
    EndBattle,
    /// <summary>
    /// Idle
    /// </summary>
    Idle,
    /// <summary>
    /// Attack
    /// </summary>
    Attack,
    /// <summary>
    /// Magic
    /// </summary>
    Magic,
    /// <summary>
    /// Hit
    /// </summary>
    Hit

}

public enum EnumCharacterState
{
    /// <summary>
    /// The idle
    /// </summary>
    Idle,
    /// <summary>
    /// The walking
    /// </summary>
    Walking,
    /// <summary>
    /// The running
    /// </summary>
    Running,
}

public enum EnumCharacterType
{
    None,
    Warrior,
    Thief,
    Witch

}

//アイテムの属性
public enum EnumEquipmentType
{
    None,
    RightHand,
    LeftHand,
    TwoHands,
    Head,
    Body,
    Usable
}

public enum EnumFightMenuAction
{
    /// <summary>
    /// The sword
    /// </summary>
    Sword,
    /// <summary>
    /// The wand
    /// </summary>
    Wand
}

public enum EnumItemMenuAction
{
    /// <summary>
    /// The potion
    /// </summary>
    Potion,
    /// <summary>
    /// The mana
    /// </summary>
    Mana
}

public enum EnumMagicMenuAction
{
    /// <summary>
    /// The fireball
    /// </summary>
    Fireball,
    /// <summary>
    /// The ice
    /// </summary>
    Ice
}

public enum EnumPlayerOrEnemy
{
    /// <summary>
    /// The player
    /// </summary>
    Player,
    /// <summary>
    /// The enemy
    /// </summary>
    Enemy
}

public enum EnumSide
{
    /// <summary>
    /// The undefined
    /// </summary>
    Undefined,
    /// <summary>
    /// The none
    /// </summary>
    None,
    /// <summary>
    /// The left
    /// </summary>
    Left,
    /// <summary>
    /// Up
    /// </summary>
    Up,
    /// <summary>
    /// The right
    /// </summary>
    Right,
    /// <summary>
    /// Down
    /// </summary>
    Down,

}