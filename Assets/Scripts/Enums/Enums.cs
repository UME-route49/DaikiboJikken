public enum EnumBattleAction
{
    None,
    Weapon,
    MoveAttack,
    Magic,
    Item,
    Pass
}

public enum EnumBattleState
{
    None,
    Beginning,
    PlayerTurn,
    SelectingPanel,
    SelectingTarget,
    SelectedTarget,
    EnemyTurn,
    PlayerWon,
    EnemyWon,
    EndBattle,
    Idle,
    Attack,
    Magic,
    Hit
}

public enum EnumPanelPosition
{
    UP, 
    UPRIGHT,
    RIGHT,
    DOWNRIGHT,
    DOWN,
    DOWNLEFT,
    LEFT,
    UPLEFT
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