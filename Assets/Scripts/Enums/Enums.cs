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

public enum EnumCharacterType
{
    None,
    Warrior,
    Thief,
    Witch
}

//アイテムの属性
public enum EnumItemType
{
    None,
    RightHand,
    LeftHand,
    TwoHands,
    Head,
    Body,
    Damage,
    Heal,
    Resurrect
}

public enum EnumMagicType
{
    Black,
    White,
}

public enum EnumPlayerOrEnemy
{
    Player,
    Enemy
}