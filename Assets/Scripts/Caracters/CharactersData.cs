using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharactersData : ICloneable
{
    //画像の名前
    public string PicturesName = string.Empty;
    //名前
    public string Name = string.Empty;
    //メインキャラかどうか
    public bool IsTheMainCharacter = false;
    //タイプ（役職）
    public EnumCharacterType Type = default(EnumCharacterType);

    //キャラクターのパラメーター
    public int HP = default(int);
    public int MP = default(int);
    public int Attack = default(int);
    public int Defense = default(int);
    public int Magic = default(int);
    public int MagicDefense = default(int);
    public int Level = default(int);
    public int XP = default(int);

    //キャラクターのHPorMPの最大
    public int MaxHP = default(int);
    public int MaxMP = default(int);

    //装備
    public ItemsData LeftHand = default(ItemsData);
    public ItemsData RightHand = default(ItemsData);
    public ItemsData Head = default(ItemsData);
    public ItemsData Body = default(ItemsData);

    //魔法
    public List<SpellsData> SpellsList = new List<SpellsData>();

    //バトル中のパネルポジション
    public EnumPanelPosition Panel;
    //ぼうぎょ中
    public bool GuardFlag = false;

    //攻撃力の取得（基礎値+装備値）
    public int GetAttack()
    {
        int value = this.Attack +
            (LeftHand == default(ItemsData) ? 0 : LeftHand.Attack) +
            (RightHand == default(ItemsData) ? 0 : RightHand.Attack) +
            (Head == default(ItemsData) ? 0 : Head.Attack) +
            (Body == default(ItemsData) ? 0 : Body.Attack);
        return value;
    }

    //防御力の取得（基礎値+装備値）
    public int GetDefense()
    {
        int value = this.Defense +
            (LeftHand == default(ItemsData) ? 0 : LeftHand.Defense) +
            (RightHand == default(ItemsData) ? 0 : RightHand.Defense) +
            (Head == default(ItemsData) ? 0 : Head.Defense) +
            (Body == default(ItemsData) ? 0 : Body.Defense);
        return value;
    }

    //魔力の取得（基礎値+装備値）
    public int GetMagic()
    {
        int value = this.Magic +
            (LeftHand == default(ItemsData) ? 0 : LeftHand.Magic) +
            (RightHand == default(ItemsData) ? 0 : RightHand.Magic) +
            (Head == default(ItemsData) ? 0 : Head.Magic) +
            (Body == default(ItemsData) ? 0 : Body.Magic);
        return value;
    }

    //魔法防御力の取得（基礎値+装備値）
    public int GetMagicDefense()
    {
        int value = this.MagicDefense +
            (LeftHand == default(ItemsData) ? 0 : LeftHand.MagicDefense) +
            (RightHand == default(ItemsData) ? 0 : RightHand.MagicDefense) +
            (Head == default(ItemsData) ? 0 : Head.MagicDefense) +
            (Body == default(ItemsData) ? 0 : Body.MagicDefense);
        return value;
    }
    
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
