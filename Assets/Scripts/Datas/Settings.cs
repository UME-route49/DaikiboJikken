using System;
using UnityEngine;

public class Settings
{
    public const string PrefabsPath = "Prefabs/";
    public const string SoundPath = "Sounds/";
    public const string BattleMusic = "BattleTheme";
    public const string FIeldMusic = "FieldTheme";
    public const string TitleMusic = "Prelude";
    public const string GameOverMusic = "GameOverMusic";
    public const string WinningMusic = "Fanfare";
    public const string ChatSound = "ChatSound";
    public const string ItemSound = "spell3";
    public const string UIClickSound = "UiSound";
    public const string TurnSound = "TurnSound";
    public const string StartSound = "StartSound";
    public const string EncountSound = "EncountSound";
    public const string MagicAuraEffect = "Magic_Aura";
    public const float InputlockingTime = 0.2f;
    public const float LetterPause = 0.01f;
    public const int DialogCharactersNumber = 100;

    /*TAGS*/
    /// <summary>The player</summary>
    public const string Player = "Player";
    /// <summary>The NPC</summary>
    public const string NPC = "NPC";
    /// <summary> The UI</summary>
    public const string UI = "UI";
    /// <summary>The UI for Battle</summary>
    public const string UIBattle = "UIBattle";
    /// <summary>The logic</summary>
    public const string Logic = "Logic";
    /// <summary>The Music </summary>
    public const string Music = "Music";

    /*Misc*/
    /// <summary>The clone name</summary>
    public const string CloneName = "(Clone)";

    /*Filename to save*/

    /// <summary>The saved file name</summary>
    public const string SavedFileName = "SavedGame.game";

    /*MainMenuScene*/
    /// <summary>The main menu scene</summary>
    public const string MainMenuScene = "Main";
    /// <summary>The loader scene</summary>
    public const string LoaderScene = "Loader";
}
