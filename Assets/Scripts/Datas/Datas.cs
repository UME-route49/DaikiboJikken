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
	public Datas()
	{ }


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
        character.HP = 20;
        character.MaxHP = 20;
        character.MP = 30;
        character.MaxMP = 30;
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
		item.EquipementType = EnumEquipmentType.RightHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Attack = 15;
		item.Price = 10;
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
		item.EquipementType = EnumEquipmentType.TwoHands;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Attack = 20;
		item.Magic = 20;
		item.Price = 10;
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
		item.EquipementType = EnumEquipmentType.Head;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 15;
		item.Price = 10;
		ItemsData[12] = item;

		item = new ItemsData();
		item.Name = "Superior helmet";
		item.Description = "A superior quality helmet";
		item.PicturesName = "C_Elm03";
		item.EquipementType = EnumEquipmentType.Head;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 25;
		item.Price = 25;
		ItemsData[13] = item;

		item = new ItemsData();
		item.Name = "Golden helmet";
		item.Description = "A golden quality helmet";
		item.PicturesName = "C_Elm04";
		item.EquipementType = EnumEquipmentType.Head;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 35;
		item.Price = 35;
		ItemsData[14] = item;

		item = new ItemsData();
		item.Name = "Hat";
		item.Description = "A noob hat";
		item.PicturesName = "C_Hat01";
		item.EquipementType = EnumEquipmentType.Head;
		item.AllowedCharacterType = EnumCharacterType.Witch;
		item.MagicDefense = 20;
		item.Price = 10;
		ItemsData[15] = item;

		item = new ItemsData();
		item.Name = "Star hat";
		item.Description = "A star hat";
		item.PicturesName = "C_Hat02";
		item.EquipementType = EnumEquipmentType.Head;
		item.AllowedCharacterType = EnumCharacterType.Witch;
		item.MagicDefense = 40;
		item.Price = 20;
		ItemsData[16] = item;

		//シールド
		item = new ItemsData();
		item.Name = "Shield";
		item.Description = "A noob shield";
		item.PicturesName = "E_Wood01";
		item.EquipementType = EnumEquipmentType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 15;
		item.Price = 10;
		ItemsData[17] = item;

		item = new ItemsData();
		item.Name = "Superior shield";
		item.Description = "A superior shield";
		item.PicturesName = "E_Wood03";
		item.EquipementType = EnumEquipmentType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 25;
		item.Price = 15;
		ItemsData[18] = item;

		item = new ItemsData();
		item.Name = "Iron shield";
		item.Description = "An iron shield";
		item.PicturesName = "E_Metal03";
		item.EquipementType = EnumEquipmentType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 30;
		item.Price = 25;
		ItemsData[19] = item;

		item = new ItemsData();
		item.Name = "Golden shield";
		item.Description = "A golden shield";
		item.PicturesName = "E_Gold02";
		item.EquipementType = EnumEquipmentType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 60;
		item.Price = 50;
		ItemsData[20] = item;

		item = new ItemsData();
		item.Name = "Bone shield";
		item.Description = "A bone shield";
		item.PicturesName = "E_Bones02";
		item.EquipementType = EnumEquipmentType.LeftHand;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 90;
		item.Price = 90;
		ItemsData[21] = item;

		//Armors
		item = new ItemsData();
		item.Name = "Armor";
		item.Description = "A noob armor";
		item.PicturesName = "A_Armour01";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 20;
		item.Price = 20;
		ItemsData[22] = item;

		item = new ItemsData();
		item.Name = "Superior Armor";
		item.Description = "A superior armor";
		item.PicturesName = "A_Armour02";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 30;
		item.Price = 30;
		ItemsData[23] = item;

		item = new ItemsData();
		item.Name = "Golden Armor";
		item.Description = "A golden armor";
		item.PicturesName = "A_Armour03";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 50;
		item.Price = 50;
		ItemsData[24] = item;

		item = new ItemsData();
		item.Name = "Red Armor";
		item.Description = "A red armor";
		item.PicturesName = "A_Armor05";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.Defense = 80;
		item.Price = 90;
		ItemsData[25] = item;

		item = new ItemsData();
		item.Name = "Clothes";
		item.Description = "A noob clothes";
		item.PicturesName = "A_Clothing01";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Warrior;
		item.MagicDefense = 20;
		item.Price = 20;
		ItemsData[26] = item;

		item = new ItemsData();
		item.Name = "Superior clothes";
		item.Description = "A superior clothes";
		item.PicturesName = "A_Clothing02";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Witch;
		item.MagicDefense = 40;
		item.Price = 40;
		ItemsData[27] = item;

		item = new ItemsData();
		item.Name = "Adamentium clothes";
		item.Description = "An adamentium clothes";
		item.PicturesName = "A_Armor04";
		item.EquipementType = EnumEquipmentType.Body;
		item.AllowedCharacterType = EnumCharacterType.Witch;
		item.MagicDefense = 80;
		item.Price = 100;
		ItemsData[28] = item;

		//アイテム
		item = new ItemsData();
		item.Name = "ポーション";
		item.Description = "A potion that add 20 Hp points";
		item.PicturesName = "P_Red04";
		item.EquipementType = EnumEquipmentType.Usable;
		item.HealthPoint = 20;
		item.Price = 5;
		ItemsData[29] = item;

		item = new ItemsData();
		item.Name = "メガポーション";
		item.Description = "A potion that add 40 Hp points";
		item.PicturesName = "P_Red03";
		item.EquipementType = EnumEquipmentType.Usable;
		item.HealthPoint = 40;
		item.Price = 10;
		ItemsData[30] = item;

		item = new ItemsData();
		item.Name = "ウルトラポーション";
		item.Description = "A big potion that add 60 Hp points";
		item.PicturesName = "P_Red03";
		item.EquipementType = EnumEquipmentType.Usable;
		item.HealthPoint = 60;
		item.Price = 30;
		ItemsData[31] = item;

		item = new ItemsData();
		item.Name = "エーテル";
		item.Description = "A potion that add 20 mana points";
		item.PicturesName = "P_Red04";
		item.EquipementType = EnumEquipmentType.Usable;
		item.Mana = 20;
		item.Price = 5;
		ItemsData[32] = item;

		item = new ItemsData();
		item.Name = "ハイエーテル";
		item.Description = "A potion that add 40 mana points";
		item.PicturesName = "P_Red03";
		item.EquipementType = EnumEquipmentType.Usable;
		item.Mana = 40;
		item.Price = 10;
		ItemsData[33] = item;

		item = new ItemsData();
		item.Name = "ウルトラポーション";
		item.Description = "A big potion that add 60 mana points";
		item.PicturesName = "P_Red03";
		item.EquipementType = EnumEquipmentType.Usable;
		item.Mana = 60;
		item.Price = 30;
		ItemsData[34] = item;
	}

	//魔法データの設定
	public static void PopulateSpellsDatas()
	{

		var spell = new SpellsData();
		spell.Name = "";
		spell.Description = "Send a fireball on your ennemies";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 5;
		spell.Attack = 10;
		spell.ParticleEffect = "FireBall";
		spell.SoundEffect = "foom_0";
		SpellsData[1] = spell;

		spell = new SpellsData();
		spell.Name = "Ice ball";
		spell.Description = "Send an ice ball on your ennemies";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 7;
		spell.Attack = 15;
		spell.ParticleEffect = "IceCold";
		spell.SoundEffect = "spell3";
		SpellsData[2] = spell;

		spell = new SpellsData();
		spell.Name = "Shadow ball";
		spell.Description = "Send a shadow ball on your ennemies";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 15;
		spell.Attack = 20;
		spell.ParticleEffect = "IceWave";
		spell.SoundEffect = "foom_0";
		SpellsData[3] = spell;

		spell = new SpellsData();
		spell.Name = "Earth quak";
		spell.Description = "Shake the ground";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 20;
		spell.Attack = 30;
		spell.ParticleEffect = "IceWave";
		spell.SoundEffect = "spell3";
		SpellsData[4] = spell;

		spell = new SpellsData();
		spell.Name = "Fire wall";
		spell.Description = "Invok a firewall";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 30;
		spell.Attack = 35;
		spell.ParticleEffect = "FireFlamish";
		spell.SoundEffect = "foom_0";
		SpellsData[5] = spell;

		spell = new SpellsData();
		spell.Name = "Ice mirror";
		spell.Description = "Freeze your ennemies inside";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 40;
		spell.Attack = 45;
		spell.ParticleEffect = "IceCold";
		SpellsData[6] = spell;

		spell = new SpellsData();
		spell.Name = "Death shadow";
		spell.Description = "Open the gate for dead souls";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 60;
		spell.Attack = 60;
		spell.ParticleEffect = "IceStars";
		spell.SoundEffect = "spell3";
		SpellsData[7] = spell;

		spell = new SpellsData();
		spell.Name = "Thunder storm";
		spell.Description = "A big thunder storm";
		spell.AllowedCharacterType = EnumCharacterType.Witch;
		spell.ManaAmount = 150;
		spell.Attack = 150;
		spell.ParticleEffect = "IceStars";
		spell.SoundEffect = "spell3";
		SpellsData[8] = spell;

		spell = new SpellsData();
		spell.Name = "Fire sword";
		spell.Description = "A fire attack with sword";
		spell.AllowedCharacterType = EnumCharacterType.Warrior;
		spell.ManaAmount = 20;
		spell.Attack = 30;
		spell.ParticleEffect = "FireExplosion";
		spell.SoundEffect = "spell3";
		SpellsData[9] = spell;

		spell = new SpellsData();
		spell.Name = "Ice sword";
		spell.Description = "An ice sword attack";
		spell.AllowedCharacterType = EnumCharacterType.Warrior;
		spell.ManaAmount = 30;
		spell.Attack = 40;
		spell.ParticleEffect = "IceCold";
		spell.SoundEffect = "spell3";
		SpellsData[10] = spell;

		spell = new SpellsData();
		spell.Name = "Deadly sword";
		spell.Description = "A deadly attack";
		spell.AllowedCharacterType = EnumCharacterType.Warrior;
		spell.ManaAmount = 50;
		spell.Attack = 80;
		spell.ParticleEffect = "IceWave";
		spell.SoundEffect = "spell3";
		SpellsData[11] = spell;
	}
}
