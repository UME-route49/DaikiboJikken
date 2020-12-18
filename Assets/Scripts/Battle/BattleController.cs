using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;
using DG.Tweening.Core;

public class BattleController : MonoBehaviour
{
    /// <summary>キャラの状態を取得</summary>
    /// <value>キャラの状態</value>
    EnumCharacterState CharacterState { get; set; }
    /// <summary>味方のキャラを取得</summary>
    /// <value>味方のキャラを取得</value>
    EnumSide CharacterSide { get; set; }

    /// <summary>可能な敵を取得</summary>
    public List<GameObject> PossibleEnnemies = new List<GameObject>();
    /// <summary>生成する敵の数</summary>
    public int NumberOfEnemyToGenerate = 1;
    /// <summary>キャラ同士の間隔</summary>
    public int SpaceBetweenCharacters = 5;
    /// <summary>敵と味方の間隔</summary>
    public int SpaceBetweenCharacterAndEnemy = 3;
    /// <summary>プレイヤーのｘ座標</summary>
    public int PlayersXPosition = -17;
    /// <summary>敵のｘ座標</summary>
    public int EnnemyXPosition = 17;
    /// <summary>プレイヤーのｙ座標</summary>
    public int PlayersYPosition = 16;
    /// <summary>敵のｙ座標</summary>
    public int EnnemyYPosition = 16;

    /// <summary>The selector</summary>
    public GameObject Selector;
    /// <summary>The target selector</summary>
    public GameObject TargetSelector;
    /// <summary>武器のエフェクト</summary>
    public GameObject WeaponParticleEffect;
    /// <summary>魔法のエフェクト</summary>
    public GameObject MagicParticleEffect;
    /// <summary>現在の状態</summary>
    private EnumBattleState currentState = EnumBattleState.Beginning;
    /// <summary>バトルのアクション</summary>
    private EnumBattleAction battlAction = EnumBattleAction.None;
    /// <summary>生成された敵のリスト</summary>
    private List<GameObject> generatedEnemyList = new List<GameObject>();
    /// <summary>生成されたキャラのリスト</summary>
    private List<GameObject> instantiatedCharacterList = new List<GameObject>();
    /// <summary>ターンの順序のリスト</summary>
    private List<Tuple<EnumPlayerOrEnemy, GameObject>> turnByTurnSequenceList = new List<Tuple<EnumPlayerOrEnemy, GameObject>>();
    /// <summary>順序の列挙</summary>
    List<Tuple<EnumPlayerOrEnemy, GameObject>>.Enumerator sequenceEnumerator;

    /// <summary>生成されたセレクター</summary>
    private GameObject instantiatedSelector;
    /// <summary>生成されたターゲットセレクター</summary>
    private GameObject instantiatedTargetSelector;
    /// <summary>選択された敵</summary>
    private GameObject selectedEnemy;
    /// <summary>選択されたプレイヤー</summary>
    private GameObject selectedPlayer;
    /// <summary>選択されたプレイヤーのデータ</summary>
    private CharactersData selectedPlayerDatas;
    /// <summary>UIのオブジェクト</summary>
    private GameObject uiGameObject;

	/// <summary>バトルUIのオブジェクト</summary>
	public GameObject uiBttle;
	/// コマンドUIの取得
	private BattlePanels battlePanels;
	/// 操作回数
	private int counter = 0;

	///カメラのオブジェクト
	public GameObject mainCamera;

	private Sequence sequence; 

	/// <summary>Awakes this instance.main</summary>
	void Awake()
	{
        CharacterState = EnumCharacterState.Idle;
		CharacterSide = EnumSide.Down;
		instantiatedSelector = GameObject.Instantiate(Selector);
		instantiatedTargetSelector = GameObject.Instantiate(TargetSelector);
		var o = Instantiate(uiBttle);
		var canvas = o.GetComponentsInChildren<Canvas>();
		foreach (Canvas canva in canvas) canva.worldCamera = Camera.main;
		GenerateEnnemies();
		PositionPlayers();
		GenerateTurnByTurnSequence();
		sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
		NextBattleSequence();
		uiGameObject  = GameObject.FindGameObjectsWithTag(Settings.UI).FirstOrDefault();
	}

    private void Start()
    {
		sequence = DOTween.Sequence();
		battlePanels = uiBttle.GetComponent<BattlePanels>();
    }

    /// <summary>This is the main loop and where the system detect the presed keys and send them to the controller.</summary>
    void Update()
	{
		if (sequence.active) return;
		else if (currentState == EnumBattleState.SelectingTarget) {
			selectedEnemy = generatedEnemyList[0];
			//PositionTargetSelector(selectedEnemy);
		}
		else if (currentState == EnumBattleState.EnemyTurn)
		{
			Log (GameTexts.EnemyTurn);
			var z = turnByTurnSequenceList.Where(w=> w.First == EnumPlayerOrEnemy.Player);
			var playerTargetedByEnemy = z.ElementAt(UnityEngine.Random.Range(0, z.Count() - 1));
			var playerTargetedByEnemyDatas =  GetCharacterDatas (playerTargetedByEnemy.Second.name);

			PositionTargetSelector(playerTargetedByEnemy.Second);
			EnemyAttack(playerTargetedByEnemy.Second, playerTargetedByEnemyDatas);
			NextBattleSequence ();                                                             
		}
		else if (currentState == EnumBattleState.PlayerTurn)
		{
			Log (GameTexts.PlayerTurn);

			mainCamera.transform.position = selectedPlayer.transform.position + new Vector3(0, 2, 0)
					- selectedPlayer.transform.forward * 4;
			mainCamera.transform.LookAt(generatedEnemyList[0].transform);

			HideTargetSelector();
			ShowMenu();
			currentState = EnumBattleState.None;
		}
		else if (currentState == EnumBattleState.PlayerWon)
		{
			Log (GameTexts.PlayerWon);
			HideTargetSelector();
			HideMenu ();
			int totalXP = 0;
			foreach (var x in generatedEnemyList) {
				totalXP += x.GetComponent<EnemyCharacterDatas> ().XP;
			}

			foreach (var x in turnByTurnSequenceList){
				var characterdatas = GetCharacterDatas (x.Second.name);
				characterdatas.XP += totalXP;
				var calculatedXP = Math.Floor ( Math.Sqrt(625+100* characterdatas.XP)-25)/ 50;
				characterdatas.Level =(int) calculatedXP;
			}
			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" + GameTexts.PlayerXP + totalXP;
			ShowDropMenu(textTodisplay);
			currentState = EnumBattleState.EndBattle;
			var go = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			if (go) go.GetComponent<AudioSource>().Stop(); 
			SoundManager.WinningMusic();         
		}
		else if (currentState == EnumBattleState.EnemyWon)
		{   
			Log (GameTexts.EnemyWon);
			HideTargetSelector();
			HideMenu ();

			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" +GameTexts.YouLost ;
			ShowDropMenu(textTodisplay);

			currentState = EnumBattleState.None;
			var go = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			if (go) go.GetComponent<AudioSource>().Stop();
			SoundManager.GameOverMusic();
        }
	}

    /// <summary>キャラクターの名前を取得</summary>
    public CharactersData GetCharacterDatas(string s )
	{ 
		return Main.CharacterList.Where (w => w.Name == s.Replace(Settings.CloneName,"" ) ).FirstOrDefault ();
	}


    /// <summary>Changes the state of the enum character</summary>
    public void ChangeEnumCharacterState(EnumCharacterState state)
	{
		CharacterState = state;
		SendMessage("Animate", string.Format("{0}{1}", CharacterState, CharacterSide));
	}

    /// <summary>Changes the enum character side.</summary>
    public void ChangeEnumCharacterSide(EnumSide state)
	{
		CharacterSide = state;
		SendMessage("Animate", string.Format("{0}{1}", CharacterState, CharacterSide));
	}

    /// <summary>Flips the specified to the left.</summary>
    void Flip(bool toTheLeft)
	{
		Vector3 theScale = transform.localScale;
		if (toTheLeft)
			theScale.x = -Mathf.Abs(theScale.x);
		else
			theScale.x = Mathf.Abs(theScale.x);
		transform.localScale = theScale;
	}

    /// <summary>敵の生成</summary>
    void GenerateEnnemies()
	{
		int y = EnnemyYPosition;
		int calculatedPosition = y;
		for (int i = 0; i <= NumberOfEnemyToGenerate - 1; i++)
		{
			y--;
			calculatedPosition = calculatedPosition - SpaceBetweenCharacters;
			GameObject go = GameObject.Instantiate(PossibleEnnemies[0],
						 new Vector3(EnnemyXPosition, 0, calculatedPosition), Quaternion.identity) as GameObject;
			generatedEnemyList.Add(go);
		}
	}

    /// <summary>プレイヤーのポジション</summary>
    void PositionPlayers()
	{
		try
		{
			int y = PlayersYPosition;
			int calculatedPosition = y;
			foreach (var character in Main.CharacterList)
			{
				y--;
				calculatedPosition = calculatedPosition - SpaceBetweenCharacters;

				GameObject go =  GameObject.Instantiate(Resources.Load(Settings.PrefabsPath + character.Name)
										, new Vector3(PlayersXPosition, 0, calculatedPosition), Quaternion.identity) as GameObject;
				go.transform.LookAt(generatedEnemyList[0].transform);
				var datas = GetCharacterDatas(go.name);

				instantiatedCharacterList.Add(go);

				//battlePanels.BroadcastMessage ("SetHPValue",datas.MaxHP <= 0 ? 0 : datas.HP*100/datas.MaxHP);
				//battlePanels.BroadcastMessage ("SetMPValue",datas.MaxMP <= 0 ? 0 : datas.MP*100/datas.MaxMP);
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}

	}

    /// <summary>ターンの順序の生成</summary>
    void GenerateTurnByTurnSequence()
	{
		var x = 0;
		var y = 0;
		var z = 0;
		var indexInRange = true;
		while (indexInRange)
		{
			if (instantiatedCharacterList.Count - 1 < y && generatedEnemyList.Count - 1 < z) { 
				indexInRange = false;
                break;
            }

            if (x % 2 == 0 && instantiatedCharacterList.Count - 1 >= y) { 
				turnByTurnSequenceList.Add(new Tuple<EnumPlayerOrEnemy, GameObject>(EnumPlayerOrEnemy.Player, instantiatedCharacterList[y]));
                y++;
            }
            else if (x % 2 != 0 && generatedEnemyList.Count - 1 >= z) { 
				turnByTurnSequenceList.Add(new Tuple<EnumPlayerOrEnemy, GameObject>(EnumPlayerOrEnemy.Enemy, generatedEnemyList[z]));
                z++;
            }
            x++;
		}
	}

    /// <summary>バトルの次の順序</summary>
    public void NextBattleSequence()
	{
		var x = turnByTurnSequenceList.Where (w => w.First == EnumPlayerOrEnemy.Enemy).Count ();
		var y = turnByTurnSequenceList.Where (w => w.First == EnumPlayerOrEnemy.Player).Count ();
		if(x<=0) {
			currentState=EnumBattleState.PlayerWon;
            return;
        }
        else if(y<=0){ 
			currentState=EnumBattleState.EnemyWon;
            return;
        }

        if (sequenceEnumerator.MoveNext ()) {
			//PositionSelector (sequenceEnumerator.Current.Second);
			if (sequenceEnumerator.Current.First == EnumPlayerOrEnemy.Player) {

				currentState = EnumBattleState.PlayerTurn;

				selectedPlayer = sequenceEnumerator.Current.Second;
				selectedPlayerDatas =GetCharacterDatas(selectedPlayer.name);
				BattlePanels.selectedCharacter = selectedPlayerDatas;
			} 
			else if (sequenceEnumerator.Current.First == EnumPlayerOrEnemy.Enemy) {
				currentState = EnumBattleState.EnemyTurn;
			}

		} else {
			sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
			NextBattleSequence();
		}
	}

    /// <summary>Positions the selector.</summary>
    public void PositionSelector(GameObject go)
	{
		instantiatedSelector.transform.position = go.transform.position + new Vector3(-4, 0, 0);
	}

	public void Action(EnumBattleAction battleAction)
    {
		currentState = EnumBattleState.SelectingTarget;
		battlAction = battleAction;
		SelectTheFirstEnemy();
		HideMenu();
		HideTargetSelector();
		PlayerAction();
	}

    /// Passes the action.
    public void PassAction()
	{
		battlAction = EnumBattleAction.Pass;
		//battlePanels.BroadcastMessage ("SetHPValue",selectedPlayerDatas.MaxHP <= 0 ? 0 : selectedPlayerDatas.HP*100/selectedPlayerDatas.MaxHP);
		//battlePanels.BroadcastMessage ("SetMPValue",selectedPlayerDatas.MaxMP <= 0 ? 0 : selectedPlayerDatas.MP*100/selectedPlayerDatas.MaxMP);
		NextBattleSequence ();
		HideMenu();
	}

    /// Selects the first enemy.
    public void SelectTheFirstEnemy()
	{
		selectedEnemy = generatedEnemyList.Where(w=>w.activeSelf).FirstOrDefault();
		if (selectedEnemy != null) PositionTargetSelector(selectedEnemy);
	}

    /// Positions the target selector.
    public void PositionTargetSelector(GameObject target)
	{
		instantiatedTargetSelector.SetActive(true);
		instantiatedTargetSelector.transform.position = target.transform.position + new Vector3(-4, 0, 0);
	}

    /// Hides the target selector.
    public void HideTargetSelector()
	{
		instantiatedTargetSelector.SetActive(false);
	}

    /// Hides the menu.
    public void HideMenu()
	{
		if (uiGameObject ) uiGameObject.BroadcastMessage("HideActionMenu");	
	}

    /// Shows the menu.
    public void ShowMenu()
	{	
		if (uiGameObject) {
			uiGameObject.BroadcastMessage("ShowActionMenu");	
			uiGameObject.BroadcastMessage ("Start");
		}
	}

	/// Ends the battle.
	public void EndBattle()
	{
		Main.ControlsBlocked = false;
        SceneManager.LoadScene (Settings.MainMenuScene);
	}

    /// Players the action.
    public void PlayerAction()
	{
		var enemyCharacterdatas = selectedEnemy.GetComponent<EnemyCharacterDatas> ();
		var animator = selectedPlayer.GetComponent<Animator>();
		int calculatedDamage = 0;
		if (enemyCharacterdatas != null && selectedPlayerDatas != null) {
			switch (battlAction) {
				case EnumBattleAction.Weapon:
					Sequence actions = DOTween.Sequence();
					animator.SetBool("Run", true);
					sequence = actions.Append(selectedPlayer.transform.DOLocalMove(selectedEnemy.transform.position
						- new Vector3(SpaceBetweenCharacterAndEnemy, 0, 0), 1.5f))//.SetEase(Ease.OutQuad)
						.AppendCallback(() => animator.SetBool("Run", false))
						.AppendCallback(() => animator.SetTrigger("Attack"));
					sequence = actions.AppendInterval(0.5f);
					sequence = actions.Append(selectedPlayer.transform
						.DOLocalMove(selectedPlayer.transform.position, 0.8f))
						.Join(selectedPlayer.transform.DOJump(selectedPlayer.transform.position, 3, 1, 0.8f));
					sequence = actions.AppendInterval(1f);

					BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
					calculatedDamage = BattlePanels.selectedWeapon.Attack 
						+ selectedPlayerDatas.GetAttack () - enemyCharacterdatas.Defense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP = Mathf.Clamp(enemyCharacterdatas.HP 
						- calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);
					ShowPopup ("-"+calculatedDamage.ToString (), selectedEnemy.transform.position);
					//selectedEnemy.BroadcastMessage ("SetHPValue",enemyCharacterdatas.MaxHP<=0?0 :  enemyCharacterdatas.HP*100/enemyCharacterdatas.MaxHP);
					Destroy( Instantiate (WeaponParticleEffect, selectedEnemy.transform.localPosition, Quaternion.identity),1.5f);
					//selectedPlayer.SendMessage("Animate",EnumBattleState.Attack.ToString());
					//selectedEnemy.SendMessage("Animate",EnumBattleState.Hit.ToString());
					break;
				case EnumBattleAction.Magic:
					calculatedDamage = BattlePanels.selectedSpell.Attack + selectedPlayerDatas.GetMagic () - enemyCharacterdatas.MagicDefense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP =Mathf.Clamp ( enemyCharacterdatas.HP - calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);
					selectedPlayerDatas.MP = Mathf.Clamp ( selectedPlayerDatas.MP - BattlePanels.selectedSpell.ManaAmount, 0 ,selectedPlayerDatas.MP - BattlePanels.selectedSpell.ManaAmount);
					ShowPopup (calculatedDamage.ToString (), selectedEnemy.transform.localPosition);
					ShowPopup ("-"+calculatedDamage.ToString (), selectedEnemy.transform.position);
					//selectedEnemy.BroadcastMessage ("SetHPValue",enemyCharacterdatas.MaxHP<=0?0 :  enemyCharacterdatas.HP*100/enemyCharacterdatas.MaxHP);
					//selectedPlayer.BroadcastMessage ("SetMPValue",selectedPlayerDatas.MaxMP <= 0 ? 0 : selectedPlayerDatas.MP*100/selectedPlayerDatas.MaxMP);

					var ennemyEffect = Resources.Load<GameObject>(Settings.PrefabsPath + BattlePanels.selectedSpell.ParticleEffect);
					Destroy(Instantiate(ennemyEffect, selectedEnemy.transform.localPosition, Quaternion.identity), 0.5f);

					var playerEffect = Resources.Load<GameObject>(Settings.PrefabsPath + Settings.MagicAuraEffect);
					Destroy( Instantiate (playerEffect, selectedPlayer.transform.localPosition, Quaternion.identity),0.4f);
					SoundManager.StaticPlayOneShot(BattlePanels.selectedSpell.SoundEffect, Vector3.zero);
					//selectedPlayer.SendMessage("Animate",EnumBattleState.Magic.ToString());
					//selectedEnemy.SendMessage("Animate",EnumBattleState.Hit.ToString());
					break;
				case EnumBattleAction.Item:
					//calculatedDamage = BattlePanels.SelectedItem.Attack - enemyCharacterdatas.MagicDefense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP = Mathf.Clamp ( enemyCharacterdatas.HP - calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);
					ShowPopup ("-"+calculatedDamage.ToString (), selectedEnemy.transform.position);
					//selectedEnemy.BroadcastMessage ("SetHPValue",enemyCharacterdatas.HP*100/enemyCharacterdatas.MaxHP);
					Destroy( Instantiate (MagicParticleEffect, selectedEnemy.transform.localPosition, Quaternion.identity),1.7f);
					SoundManager.ItemSound();
					//selectedPlayer.SendMessage("Animate",EnumBattleState.Magic.ToString());
					//selectedEnemy.SendMessage("Animate",EnumBattleState.Hit.ToString());
					break;
				default:
					break;
			}
		}

		if (enemyCharacterdatas.HP <= 0) KillCharacter (selectedEnemy);
		//selectedPlayer.SendMessage ("ChangeEnumCharacterState", battlection);
		selectedEnemy = null;

		NextBattleSequence();
	}

    /// Enemies the attack.
    public void EnemyAttack(GameObject playerToAttack, CharactersData playerToAttackDatas )
	{
		var go = sequenceEnumerator.Current.Second;
		mainCamera.transform.position = playerToAttack.transform.position + new Vector3(0, 5, 10)
					- selectedPlayer.transform.forward * 5;
		mainCamera.transform.LookAt(playerToAttack.transform);

		var enemyCharacterdatas = go.GetComponent<EnemyCharacterDatas> ();
		int calculatedDamage = 0;
		if (enemyCharacterdatas != null && selectedPlayerDatas != null) {
			switch (battlAction) {
			case EnumBattleAction.Weapon:
				go.GetComponent<Animator>().SetTrigger("Attack");
				calculatedDamage = enemyCharacterdatas.Attack - playerToAttackDatas.Defense; 
				calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
				playerToAttackDatas.HP = Mathf.Clamp (playerToAttackDatas.HP - calculatedDamage , 0 ,playerToAttackDatas.HP - calculatedDamage);
				ShowPopup ("-"+calculatedDamage.ToString (), playerToAttack.transform.position);
				//battlePanels.BroadcastMessage ("SetHPValue",playerToAttackDatas.MaxHP<=0 ?0 : playerToAttackDatas.HP*100/playerToAttackDatas.MaxHP);
				Destroy(Instantiate(WeaponParticleEffect, playerToAttack.transform.localPosition, Quaternion.identity),1.5f);
				//playerToAttack.SendMessage("Animate",EnumBattleState.Hit.ToString());
				break;
			default:
				calculatedDamage = enemyCharacterdatas.Attack - playerToAttackDatas.Defense; 
				calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
				playerToAttackDatas.HP = Mathf.Clamp (playerToAttackDatas.HP - calculatedDamage , 0 ,playerToAttackDatas.HP - calculatedDamage);
				ShowPopup ("-"+calculatedDamage.ToString (), playerToAttack.transform.position);
				//battlePanels.BroadcastMessage ("SetHPValue",playerToAttackDatas.MaxHP<=0 ?0 : playerToAttackDatas.HP*100/playerToAttackDatas.MaxHP);
				Destroy( Instantiate (WeaponParticleEffect, playerToAttack.transform.localPosition, Quaternion.identity),1.5f);
				SoundManager.WeaponSound();
				//go.SendMessage("Animate",EnumBattleState.Attack.ToString());
				//playerToAttack.SendMessage("Animate",EnumBattleState.Hit.ToString());
				
				break;
			}
		}

		Sequence actions = DOTween.Sequence();
		sequence = actions.Append(go.transform.DOLookAt(playerToAttack.transform.position, 0.2f));
		sequence = actions.Append(go.transform.DOLocalMove(playerToAttack.transform.position
			+ playerToAttack.transform.forward * 4, 1).SetEase(Ease.OutCirc));
		sequence = actions.AppendInterval(2f);
		sequence = actions.Append(go.transform.DOLocalMove(go.transform.position, 1).SetEase(Ease.OutCirc));

		if (playerToAttackDatas.HP <= 0) KillCharacter (playerToAttack);
		selectedEnemy = null;
	}

    /// Kills the character.
    public void KillCharacter(GameObject go)
	{
		go.SetActive (false);
		turnByTurnSequenceList.RemoveAll (r => r.Second.GetInstanceID () == go.GetInstanceID ());
		var id = sequenceEnumerator.Current.Second.GetInstanceID ();
		sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
		sequenceEnumerator.MoveNext();
		while (sequenceEnumerator.Current.Second.GetInstanceID() != id) sequenceEnumerator.MoveNext();
	}

    /// Logs the specified text.
    public void Log(string text)
	{
		if (uiGameObject) uiGameObject.BroadcastMessage ("LogText",text);
	}

    /// Shows the popup.
    public void ShowPopup(string text, Vector3 position)
	{
		if (uiGameObject)uiGameObject.BroadcastMessage ("ShowPopup", new object[]{ text, position });
	}

    /// Hides the popup.
    public void HidePopup()
	{
		if (uiGameObject) uiGameObject.BroadcastMessage ("HidePopup");
	}

    /// Shows the drop menu.
    public void ShowDropMenu(string text)
	{
		if (uiGameObject) uiGameObject.BroadcastMessage ("ShowDropMenu", text);
	}
}