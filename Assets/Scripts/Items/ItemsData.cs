using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemsData
{
    //画像の名前
    public string PicturesName = string.Empty;
    //名前
    public string Name = string.Empty;
    //説明
    public string Description = string.Empty;

    //アイテムのパラメーター
    public int HpDamege = default(int);
    public int MpDamege = default(int);

    public int Attack = default(int);
    public int Defense = default(int);
    public int Magic = default(int);
    public int MagicDefense = default(int);
    public int Price = default(int);

    //装備できる役職
    public EnumCharacterType AllowedCharacterType = EnumCharacterType.None;
    //アイテムの属性（使い方）
    public EnumItemType EquipementType = EnumItemType.None;
    //装備されているかどうか
    public bool isEquiped = false;
}
