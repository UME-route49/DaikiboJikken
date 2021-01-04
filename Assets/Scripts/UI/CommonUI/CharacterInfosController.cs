using UnityEngine;
using UnityEngine.UI;

public class CharacterInfosController : MonoBehaviour {

    /// The character number
    public int CharacterNumber = 1;

    /// The characters datas
    public CharactersData CharactersDatas;
    /// The selected character infos
    public bool SelectedCharacterInfos  = false  ;
    /// The characters image
    public Image  CharactersImage ;

    //UI public text boxes
    /// The characters name
    public Text CharactersName ;
    /// The characters class
    public Text CharactersClass;
    /// The characters level
    public Text CharactersLevel ;

    //The  ammount of HP/MP that the character can have it's inferior of the max HP/MP if the character is wounded
    /// The characters hp
    public Text CharactersHP ;
    /// The characters mp
    public Text CharactersMP ;

    //The maximum ammount of HP/MP that the character can have 
    /// The characters hp maximum
    public Text CharactersHPMax ;
    /// The characters mp maximum
    public Text CharactersMPMax ;

    //Equipments texts
    /// The right hand text
    public Text RightHandText ;
    /// The left hand text
    public Text LeftHandText;
    /// The head text
    public Text HeadText ;
    /// The body text
    public Text BodyText ;

    //Equipments images
    /// The right hand image
    public Image RightHandImage ;
    /// The left hand image
    public Image LeftHandImage;
    /// The head image
    public Image HeadImage ;
    /// The body image
    public Image BodyImage;

    //Abilities texts
    /// The attack text
    public Text AttackText ;
    /// The magic text
    public Text MagicText;
    /// The defense text
    public Text DefenseText ;
    /// The magic defense text
    public Text MagicDefenseText ;

    //Abilities texts of the new item to compare with the old one 
    /// The attack text compare
    public Text AttackTextCompare;
    /// The magic text compare
    public Text MagicTextCompare;
    /// The defense text compare
    public Text DefenseTextCompare;
    /// The magic defense text compare
    public Text MagicDefenseTextCompare;

    /// Starts this instance.
    void Start () {
		if (GameMenu.SelectedCharacter != default(CharactersData) && SelectedCharacterInfos)
			CharacterNumber = Main.CharacterList.IndexOf (GameMenu.SelectedCharacter)+1;
		if (Main.CharacterList.Count >= CharacterNumber) {
			//Getting the charactersDatas from the list and setting it to the ui objects
			CharactersDatas = Main.CharacterList [CharacterNumber - 1];
			Contract.Requires<MissingReferenceException>(CharactersDatas!=null);
			LoadCharactersAbilities();
			LoadEquipements();
			LoadEquipementsAbilities();
		} else {
			//If the list doesnt contains enough characters the engine hide this character infos
			if(this.gameObject) this.gameObject.SetActive(false);
		}
	}

    /// This procedure display the character abilities points
    public void LoadCharactersAbilities()
	{
        if (CharactersName != null) CharactersName.text = CharactersDatas.Name;
        if (CharactersClass != null) CharactersClass.text = CharactersDatas.Type.ToString();
        if (CharactersLevel != null) CharactersLevel.text = CharactersDatas.Level.ToString();
        if (CharactersHP != null) CharactersHP.text = CharactersDatas.HP.ToString();
        if (CharactersMP != null) CharactersMP.text = CharactersDatas.MP.ToString();
        if (CharactersHPMax != null) CharactersHPMax.text = CharactersDatas.MaxHP.ToString();
        if (CharactersMPMax != null) CharactersMPMax.text = CharactersDatas.MaxMP.ToString();
    }

    /// This procedure display the equipment abilities points
    public void LoadEquipementsAbilities()
	{
		if (AttackText != null) AttackText.text = CharactersDatas.GetAttack().ToString();
		if (MagicText != null) MagicText.text = CharactersDatas.GetMagic().ToString();
	    if(DefenseText != null) DefenseText.text = CharactersDatas.GetDefense().ToString();
        if(MagicDefenseText != null) MagicDefenseText.text = CharactersDatas.GetMagicDefense().ToString();
	}

    /// This procedure display the equiped items
    public void LoadEquipements()
	{
		if (CharactersDatas.RightHand != null) 
			if (RightHandText != null) RightHandText.text = CharactersDatas.RightHand.Name;
        else {
			if (RightHandImage != null) RightHandImage.sprite = null;
            if (RightHandText != null) RightHandText.text = string.Empty;
        }
        if (CharactersDatas.LeftHand != null)
            if (LeftHandText  != null) LeftHandText.text = CharactersDatas.LeftHand.Name;
        else {
			if(LeftHandImage != null) LeftHandImage.sprite = null;
            if (LeftHandText  != null) LeftHandText.text = string.Empty;
        }
        if (CharactersDatas.Head != null)
            if (HeadText  != null) HeadText.text = CharactersDatas.Head.Name;
        else {
			if(HeadImage != null) HeadImage.sprite = null;
            if (HeadText  != null) HeadText.text = string.Empty;
        }
        if (CharactersDatas.Body != null)
            if (BodyText != null) BodyText.text = CharactersDatas.Body.Name;
        else {
			if(BodyImage != null) BodyImage.sprite = null;
            if (BodyText != null) BodyText.text = string.Empty;
        }
    }

    /// This procedure display the equiped items
    public void CompareEquipementsAbilities(ItemsData itemsData)
	{
        var clonedSelectedCharacter =( CharactersData)( GameMenu.SelectedCharacter.Clone());

        switch (itemsData.EquipementType)
        {
            case EnumItemType.Head: 
                clonedSelectedCharacter.Head = itemsData; 
                break;
            case EnumItemType.Body : 
                clonedSelectedCharacter.Body = itemsData; 
                break;
            case EnumItemType.LeftHand: 
                clonedSelectedCharacter.LeftHand = itemsData; 
                break;
            case EnumItemType.RightHand: 
                clonedSelectedCharacter.RightHand = itemsData; 
                break;
            case EnumItemType.TwoHands: 
                clonedSelectedCharacter.RightHand = itemsData; clonedSelectedCharacter.LeftHand = null; 
                break;
        }
       
        if (AttackTextCompare != null) AttackTextCompare.text = clonedSelectedCharacter.GetAttack().ToString();
        if (MagicTextCompare != null) MagicTextCompare.text = clonedSelectedCharacter.GetMagic().ToString();
        if (DefenseTextCompare != null) DefenseTextCompare.text = clonedSelectedCharacter.GetDefense().ToString();
        if (MagicDefenseTextCompare != null) MagicDefenseTextCompare.text = clonedSelectedCharacter.GetMagicDefense().ToString();
    }

    /// Toggles the select character.
    public void ToggleSelectCharacter(GameObject gameObject)
	{
		Contract.Requires<UnassignedReferenceException> (gameObject != null);

		var toggle = gameObject.GetComponent<Toggle>();
		
		if (toggle.isOn) GameMenu.SelectedCharacter = CharactersDatas;
	}
}
