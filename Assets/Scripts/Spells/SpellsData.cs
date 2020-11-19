using System;

[Serializable]
//魔法のデータ
public class SpellsData
{
    //名前
    public string Name = string.Empty;
    //説明
    public string Description = string.Empty;
    
    //威力
    public int Attack = default(int);
    //消費MP
    public int ManaAmount = default(int);
    //エフェクト
    public string ParticleEffect = string.Empty;
    //SE
    public string SoundEffect = string.Empty;
    //使える役職
    public EnumCharacterType AllowedCharacterType = EnumCharacterType.None;
}