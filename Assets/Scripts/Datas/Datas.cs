using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas : MonoBehaviour
{
	//キャラクターのデータ
	public static Dictionary<int, CharactersData> CharactersData = new Dictionary<int, CharactersData>();
	//アイテムのデータ
	public static Dictionary<int, ItemsData> ItemsData = new Dictionary<int, ItemsData>();
	//魔法のデータ
	public static Dictionary<int, SpellsData> SpellsData = new Dictionary<int, SpellsData>();

	//クラスの新しいインスタンスの初期化
	public Datas(){}

	//データ設定
	public static void PopulateDatas()
	{
		PopulateCharactersDatas();
		PopulateItemsDatas();
		PopulateSpellsDatas();
	}

	//キャラクターデータ設定
	public static void PopulateCharactersDatas()
	{
		var character = new CharactersData();
		character.Name = "Warrior";
		character.Type = EnumCharacterType.Warrior;
		character.Level = 1;
		character.PicturesName = "Warrior";
		character.HP = 100;
		character.MaxHP = 100;
		character.MP = 10;
		character.MaxMP = 10;
		CharactersData[1] = character;

        character = new CharactersData();
        character.Name = "Thief";
        character.Type = EnumCharacterType.Thief;
        character.Level = 1;
        character.PicturesName = "Thief";
        character.HP = 100;
        character.MaxHP = 100;
        character.MP = 30;
        character.MaxMP = 30;
        CharactersData[2] = character;

        character = new CharactersData();
        character.Name = "Witch";
        character.Type = EnumCharacterType.Witch;
        character.Level = 1;
        character.PicturesName = "Witch";
        character.HP = 100;
        character.MaxHP = 100;
        character.MP = 50;
        character.MaxMP = 50;
        CharactersData[3] = character;
    }

	//アイテムデータの設定
	public static void PopulateItemsDatas()
	{
		//武器
		var item = new ItemsData();
		item.Name = "ブロードソード";
		item.Description = "見習い戦士に国から支給される剣。";
		item.PicturesName = "sword001";
		item.itemType = EnumItemType.RightHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Attack = 15;
		item.Price = 10;
		item.Particle = "Sword";
		item.Sound = "Sword";
		ItemsData[1] = item;

		//item = new ItemsData();
		//item.Name = "マスターソード";
		//item.Description = "サーの称号を手にした英雄にのみ装備することを許された剣。";
		//item.PicturesName = "W_Sword004";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 30;
		//item.Price = 20;
		//ItemsData[2] = item;

		//item = new ItemsData();
		//item.Name = "Vedetta sword";
		//item.Description = "A sword to take revenge";
		//item.PicturesName = "W_Sword008";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 50;
		//item.Price = 30;
		//ItemsData[3] = item;

		//item = new ItemsData();
		//item.Name = "Cimitare cimitery";
		//item.Description = "To send your enemies to the cimitary";
		//item.PicturesName = "W_Sword013";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 60;
		//item.Price = 40;
		//ItemsData[4] = item;

		//item = new ItemsData();
		//item.Name = "Lighting sword";
		//item.Description = "Contains lightning power";
		//item.PicturesName = "W_Sword015";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 70;
		//item.Price = 50;
		//ItemsData[5] = item;

		//item = new ItemsData();
		//item.Name = "Fire sword";
		//item.Description = "Contains Fire power";
		//item.PicturesName = "W_Sword016";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 80;
		//item.Price = 60;
		//ItemsData[6] = item;

		//item = new ItemsData();
		//item.Name = "Water sword";
		//item.Description = "Contains Water power";
		//item.PicturesName = "W_Sword017";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 90;
		//item.Price = 70;
		//ItemsData[7] = item;

		//item = new ItemsData();
		//item.Name = "Black sword";
		//item.Description = "No one know the real power of this sword";
		//item.PicturesName = "W_Sword021";
		//item.EquipementType = EnumEquipmentType.RightHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 120;
		//item.Price = 150;
		//ItemsData[8] = item;

		item = new ItemsData();
		item.Name = "Wand";
		item.Description = "A noob wand";
		item.PicturesName = "W_Mace010";
		item.itemType = EnumItemType.TwoHands;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Attack = 20;
		item.Magic = 20;
		item.Price = 10;
		item.Particle = "Wand";
		item.Sound = "Wand";
		ItemsData[9] = item;

		//item = new ItemsData();
		//item.Name = "Master wand";
		//item.Description = "A Master wand";
		//item.PicturesName = "W_Mace007";
		//item.EquipementType = EnumEquipmentType.TwoHands;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Attack = 30;
		//item.Magic = 30;
		//item.Price = 30;
		//ItemsData[10] = item;

		//item = new ItemsData();
		//item.Name = "Solar wand";
		//item.Description = "A wand that contains the solar power";
		//item.PicturesName = "W_Mace014";
		//item.EquipementType = EnumEquipmentType.TwoHands;
		//item.AllowedCharacterType = EnumCharacterType.Wizard;
		//item.Attack = 60;
		//item.Magic = 70;
		//item.Price = 120;
		//ItemsData[11] = item;

		//ヘルメット
		item = new ItemsData();
		item.Name = "helmet";
		item.Description = "A noob helmet";
		item.PicturesName = "C_Elm01";
		item.itemType = EnumItemType.Head;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 15;
		item.Price = 10;
		ItemsData[12] = item;

        //item = new ItemsData();
        //item.Name = "Superior helmet";
        //item.Description = "A superior quality helmet";
        //item.PicturesName = "C_Elm03";
        //item.EquipementType = EnumEquipmentType.Head;
        //item.AllowedCharacterType = EnumCharacterType.Warrior;
        //item.Defense = 25;
        //item.Price = 25;
        //ItemsData[13] = item;

        //item = new ItemsData();
        //item.Name = "Golden helmet";
        //item.Description = "A golden quality helmet";
        //item.PicturesName = "C_Elm04";
        //item.EquipementType = EnumEquipmentType.Head;
        //item.AllowedCharacterType = EnumCharacterType.Warrior;
        //item.Defense = 35;
        //item.Price = 35;
        //ItemsData[14] = item;

        item = new ItemsData();
        item.Name = "Hat";
        item.Description = "A noob hat";
        item.PicturesName = "C_Hat01";
        item.itemType = EnumItemType.Head;
        item.AllowedCharacterType = EnumCharacterType.Witch;
        item.MagicDefense = 20;
        item.Price = 10;
        ItemsData[15] = item;

        //item = new ItemsData();
        //item.Name = "Star hat";
        //item.Description = "A star hat";
        //item.PicturesName = "C_Hat02";
        //item.EquipementType = EnumEquipmentType.Head;
        //item.AllowedCharacterType = EnumCharacterType.Witch;
        //item.MagicDefense = 40;
        //item.Price = 20;
        //ItemsData[16] = item;

        //シールド
        item = new ItemsData();
		item.Name = "Shield";
		item.Description = "A noob shield";
		item.PicturesName = "E_Wood01";
		item.itemType = EnumItemType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 15;
		item.Price = 10;
		ItemsData[17] = item;

		//item = new ItemsData();
		//item.Name = "Superior shield";
		//item.Description = "A superior shield";
		//item.PicturesName = "E_Wood03";
		//item.EquipementType = EnumEquipmentType.LeftHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Defense = 25;
		//item.Price = 15;
		//ItemsData[18] = item;

		//item = new ItemsData();
		//item.Name = "Iron shield";
		//item.Description = "An iron shield";
		//item.PicturesName = "E_Metal03";
		//item.EquipementType = EnumEquipmentType.LeftHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Defense = 30;
		//item.Price = 25;
		//ItemsData[19] = item;

		//item = new ItemsData();
		//item.Name = "Golden shield";
		//item.Description = "A golden shield";
		//item.PicturesName = "E_Gold02";
		//item.EquipementType = EnumEquipmentType.LeftHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Defense = 60;
		//item.Price = 50;
		//ItemsData[20] = item;

		//item = new ItemsData();
		//item.Name = "Bone shield";
		//item.Description = "A bone shield";
		//item.PicturesName = "E_Bones02";
		//item.EquipementType = EnumEquipmentType.LeftHand;
		//item.AllowedCharacterType = EnumCharacterType.Warrior;
		//item.Defense = 90;
		//item.Price = 90;
		//ItemsData[21] = item;

		//Armors
		item = new ItemsData();
		item.Name = "Armor";
		item.Description = "A noob armor";
		item.PicturesName = "A_Armour01";
		item.itemType = EnumItemType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 20;
		item.Price = 20;
		ItemsData[22] = item;

        //item = new ItemsData();
        //item.Name = "Superior Armor";
        //item.Description = "A superior armor";
        //item.PicturesName = "A_Armour02";
        //item.EquipementType = EnumEquipmentType.Body;
        //item.AllowedCharacterType = EnumCharacterType.Warrior;
        //item.Defense = 30;
        //item.Price = 30;
        //ItemsData[23] = item;

        //item = new ItemsData();
        //item.Name = "Golden Armor";
        //item.Description = "A golden armor";
        //item.PicturesName = "A_Armour03";
        //item.EquipementType = EnumEquipmentType.Body;
        //item.AllowedCharacterType = EnumCharacterType.Warrior;
        //item.Defense = 50;
        //item.Price = 50;
        //ItemsData[24] = item;

        //item = new ItemsData();
        //item.Name = "Red Armor";
        //item.Description = "A red armor";
        //item.PicturesName = "A_Armor05";
        //item.EquipementType = EnumEquipmentType.Body;
        //item.AllowedCharacterType = EnumCharacterType.Warrior;
        //item.Defense = 80;
        //item.Price = 90;
        //ItemsData[25] = item;

        item = new ItemsData();
        item.Name = "Clothes";
        item.Description = "A noob clothes";
        item.PicturesName = "A_Clothing01";
        item.itemType = EnumItemType.Body;
        item.AllowedCharacterType = EnumCharacterType.Warrior;
        item.MagicDefense = 20;
        item.Price = 20;
        ItemsData[26] = item;

        //item = new ItemsData();
        //item.Name = "Superior clothes";
        //item.Description = "A superior clothes";
        //item.PicturesName = "A_Clothing02";
        //item.EquipementType = EnumEquipmentType.Body;
        //item.AllowedCharacterType = EnumCharacterType.Witch;
        //item.MagicDefense = 40;
        //item.Price = 40;
        //ItemsData[27] = item;

        //item = new ItemsData();
        //item.Name = "Adamentium clothes";
        //item.Description = "An adamentium clothes";
        //item.PicturesName = "A_Armor04";
        //item.EquipementType = EnumEquipmentType.Body;
        //item.AllowedCharacterType = EnumCharacterType.Witch;
        //item.MagicDefense = 80;
        //item.Price = 100;
        //ItemsData[28] = item;

        //アイテム
        item = new ItemsData();
		item.Name = "ポーション";
		item.Description = "HPを20回復。";
		item.PicturesName = "P_Red04";
		item.itemType = EnumItemType.Heal;
		item.HpDamege = 20;
		item.Price = 5;
		item.Particle = "Potion";
		item.Sound = "Potion";
		ItemsData[29] = item;

		item = new ItemsData();
		item.Name = "メガポーション";
		item.Description = "HPを40回復。";
		item.PicturesName = "P_Red03";
		item.itemType = EnumItemType.Heal;
		item.HpDamege = 40;
		item.Price = 10;
		item.Particle = "MegaPotion";
		item.Sound = "MegaPotion";
		ItemsData[30] = item;

		item = new ItemsData();
		item.Name = "ウルトラポーション";
		item.Description = "HPを60回復。";
		item.PicturesName = "P_Red03";
		item.itemType = EnumItemType.Heal;
		item.HpDamege = 60;
		item.Price = 30;
		item.Particle = "UltraPotion";
		item.Sound = "UltraPotion";
		ItemsData[31] = item;

		item = new ItemsData();
		item.Name = "エーテル";
		item.Description = "MPを30回復。";
		item.PicturesName = "P_Red04";
		item.itemType = EnumItemType.Heal;
		item.MpDamege = 30;
		item.Price = 5;
		item.Particle = "Ether";
		item.Sound = "Ether";
		ItemsData[32] = item;

		item = new ItemsData();
		item.Name = "メガエーテル";
		item.Description = "MPを120回復。";
		item.PicturesName = "P_Red03";
		item.itemType = EnumItemType.Heal;
		item.MpDamege = 120;
		item.Price = 10;
		item.Particle = "MegaEther";
		item.Sound = "MegaEther";
		ItemsData[33] = item;

		item = new ItemsData();
		item.Name = "ウルトラエーテル";
		item.Description = "MPを500回復";
		item.PicturesName = "P_Red03";
		item.itemType = EnumItemType.Heal;
		item.MpDamege = 500;
		item.Price = 30;
		item.Particle = "UltraEther";
		item.Sound = "UltraEther";
		ItemsData[34] = item;
	}

	//魔法データの設定
	public static void PopulateSpellsDatas()
	{
		var spell = new SpellsData();
		spell.Name = "ホート";
		spell.Description = "アツい炎で敵を燃やす。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 5;
		spell.Attack = 10;
		spell.Particle = "Hoat";
		spell.Sound = "Hoat";
		SpellsData[1] = spell;

		spell = new SpellsData();
		spell.Name = "ホーター";
		spell.Description = "アツアツの炎で敵を燃やす。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 7;
		spell.Attack = 15;
		spell.Particle = "Hoater";
		spell.Sound = "Hoater";
		SpellsData[2] = spell;

		spell = new SpellsData();
		spell.Name = "ホティスト";
		spell.Description = "アッツアツの火炎で敵を燃やし尽くす。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 15;
		spell.Attack = 20;
		spell.Particle = "Hotist";
		spell.Sound = "Hotist";
		SpellsData[3] = spell;

		spell = new SpellsData();
		spell.Name = "コル";
		spell.Description = "冷たい風が敵を襲う。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 20;
		spell.Attack = 30;
		spell.Particle = "Col";
		spell.Sound = "Col";
		SpellsData[4] = spell;

		spell = new SpellsData();
		spell.Name = "コレル";
		spell.Description = "氷のつぶてが敵を襲う。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 30;
		spell.Attack = 35;
		spell.Particle = "Colel";
		spell.Sound = "Colel";
		SpellsData[5] = spell;

		spell = new SpellsData();
		spell.Name = "コレスト";
		spell.Description = "猛烈な吹雪が敵を凍り付かせる。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 40;
		spell.Attack = 45;
		spell.Particle = "Colest";
		spell.Sound = "Colest";
		SpellsData[6] = spell;

		spell = new SpellsData();
		spell.Name = "レス";
		spell.Description = "かすり傷程度の小さな傷を癒す。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 5;
		spell.Attack = -10;
		spell.Particle = "Res";
		spell.Sound = "Res";
		SpellsData[7] = spell;

		spell = new SpellsData();
		spell.Name = "レシア";
		spell.Description = "多少大きな傷も癒す。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 12;
		spell.Attack = -30;
		spell.Particle = "Resia";
		spell.Sound = "Resia";
		SpellsData[8] = spell;

		spell = new SpellsData();
		spell.Name = "レシエスト";
		spell.Description = "腕がちぎれていても何とかなるくらい癒す。";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 20;
		spell.Attack = -70;
		spell.Particle = "Resiast";
		spell.Sound = "Resiast";
		SpellsData[9] = spell;
	}
}
