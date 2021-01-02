using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using DG.Tweening.Core;

public class BattleController : MonoBehaviour
{ 
    //現在の状態
    EnumBattleState currentState = EnumBattleState.Beginning;
    //バトルのアクション
    EnumBattleAction battlAction = EnumBattleAction.None;

    //可能な敵を取得
    public List<GameObject> PossibleEnnemies = new List<GameObject>();
    //生成する敵の数
    public int NumberOfEnemyToGenerate = 1;
	//キャラクターのパネルポジション
	private bool[] PanelPosition = new bool[8];
    //武器のエフェクト
    public GameObject WeaponParticleEffect;
    //魔法のエフェクト
    public GameObject MagicParticleEffect;
	//バトルUIのオブジェクト（プレハブ）
	public GameObject uiObject;
	//パネルのオブジェクト（プレハブ）
	public GameObject panelObject;
	//カメラのオブジェクト
	public GameObject mainCamera;

    //生成された敵のリスト
    List<GameObject> generatedEnemyList = new List<GameObject>();
    //生成されたキャラのリスト
    List<GameObject> instantiatedCharacterList = new List<GameObject>();
    //ターンの順序のリスト
    List<Tuple<EnumPlayerOrEnemy, GameObject>> turnByTurnSequenceList = new List<Tuple<EnumPlayerOrEnemy, GameObject>>();
    //順序の列挙
    List<Tuple<EnumPlayerOrEnemy, GameObject>>.Enumerator sequenceEnumerator;

    //選択された敵
    GameObject selectedEnemy;
    //選択されたプレイヤー
    GameObject selectedPlayer;
    //選択されたプレイヤーのデータ
    CharactersData selectedPlayerDatas;
    //バトルUIのオブジェクト（クローン）
    GameObject battleUi;
	//パネルのオブジェクト（クローン）
	GameObject panel;
	//DOTweenシーケンス
	Sequence sequence;
	//FadeOutクラスへの参照
	FadeOut fadeOut;


	int panelSelect = 0;

	/// <summary>Awakes this instance.main</summary>
	void Awake()
	{
		panel = Instantiate(panelObject);
		battleUi = Instantiate(uiObject);

		GenerateEnnemies();
		PositionPlayers();

		GenerateTurnByTurnSequence();
		sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
		NextBattleSequence();
	}

    private void Start()
    {
		sequence = DOTween.Sequence();
		SoundManager.BattleMusic();
		fadeOut = FindObjectOfType<FadeOut>();
		currentState = EnumBattleState.Beginning;
    }

    void Update()
	{
		if (sequence.active) return;
		else if (currentState == EnumBattleState.Beginning)
		{
			//var r = Random.Range(0, 6);
			currentState = EnumBattleState.PlayerTurn;
		}
		else if (currentState == EnumBattleState.EnemyTurn)
		{
			battleUi.SendMessage("LogText", GameTexts.EnemyTurn);

			var z = turnByTurnSequenceList.Where(w => w.First == EnumPlayerOrEnemy.Player);
			var playerTargetedByEnemy = z.ElementAt(UnityEngine.Random.Range(0, z.Count() - 1));
			var playerTargetedByEnemyDatas = GetCharacterDatas(playerTargetedByEnemy.Second.name);

			battlAction = EnumBattleAction.Weapon;

			EnemyAttack(playerTargetedByEnemy.Second, playerTargetedByEnemyDatas);
			NextBattleSequence();
		}
		else if (currentState == EnumBattleState.PlayerTurn)
		{
			battleUi.SendMessage("LogText", selectedPlayer.name + "の番です。");

			mainCamera.transform.position = selectedPlayer.transform.position + new Vector3(0, 2, 0)
				- selectedPlayer.transform.forward * 4;
			mainCamera.transform.LookAt(generatedEnemyList[0].transform);

			SoundManager.TurnSound();
			ShowMenu();
			currentState = EnumBattleState.None;
		}
		else if(currentState == EnumBattleState.SelectingPanel)
        {
			mainCamera.transform.position = new Vector3(0, 30, 0);
			mainCamera.transform.LookAt(generatedEnemyList[0].transform.position);

			panel.SetActive(true);
				
			var characterData = GetCharacterDatas(selectedPlayer.name);
            panel.transform.GetChild((int)characterData.Panel)
                .GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;

            if (Input.GetKeyDown(KeyCode.D))
            {
				panel.transform.GetChild((int)characterData.Panel + panelSelect)
					.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

				panelSelect++;
				if ((int)characterData.Panel + panelSelect > panel.transform.childCount - 1)
					panelSelect = -(int)characterData.Panel;
				panel.transform.GetChild((int)characterData.Panel + panelSelect)
					.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
			}
			else if (Input.GetKeyDown(KeyCode.A))
            {
				panel.transform.GetChild((int)characterData.Panel + panelSelect)
					.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;

				panelSelect--;
				if ((int)characterData.Panel + panelSelect < 0)
					panelSelect = panel.transform.childCount - (int)characterData.Panel - 1;
				panel.transform.GetChild((int)characterData.Panel + panelSelect)
					.GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
			}
			else if (Input.GetKeyDown(KeyCode.Space))
            {
				if (PanelPosition[(int)characterData.Panel + panelSelect]) return;

				panel.transform.GetChild((int)characterData.Panel + panelSelect)
					.GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
				PanelPosition[(int)characterData.Panel] = false;
				PanelPosition[(int)characterData.Panel + panelSelect] = true;
				characterData.Panel = (EnumPanelPosition)((int)characterData.Panel + panelSelect);
				panel.SetActive(false);
				PlayerAction();
			}
        }
		else if (currentState == EnumBattleState.PlayerWon)
		{

			battleUi.SendMessage("LogText", GameTexts.PlayerWon);
			battleUi.BroadcastMessage("HideActionMenu");
			int totalXP = 0;
			foreach (var x in generatedEnemyList)
				totalXP += x.GetComponent<EnemyCharacterDatas>().XP;

			foreach (var x in turnByTurnSequenceList)
			{
				var characterdatas = GetCharacterDatas(x.Second.name);
				characterdatas.XP += totalXP;
				var calculatedXP = Mathf.Floor(Mathf.Sqrt(625 + 100 * characterdatas.XP) - 25) / 50;
				characterdatas.Level = (int)calculatedXP;
			}
			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" + GameTexts.PlayerXP + totalXP;
			battleUi.BroadcastMessage("ShowDropMenu", textTodisplay);


			var go = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			go.GetComponent<AudioSource>().Stop();
			SoundManager.WinningMusic();

			currentState = EnumBattleState.EndBattle;
		}
		else if (currentState == EnumBattleState.EnemyWon)
		{
			battleUi.SendMessage("LogText", GameTexts.EnemyWon);
			battleUi.BroadcastMessage("HideActionMenu");

			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" + GameTexts.YouLost;
			battleUi.BroadcastMessage("ShowDropMenu", textTodisplay);

			var go = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			if (go) go.GetComponent<AudioSource>().Stop();
			SoundManager.GameOverMusic();

			currentState = EnumBattleState.EndBattle;
		}
		else if (currentState == EnumBattleState.EndBattle)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Main.ControlsBlocked = false;
				fadeOut.StartCoroutine(fadeOut.Fadeout("DemoField", 2f));
			}
		}
	}

    /// <summary>キャラクターの名前を取得</summary>
    public CharactersData GetCharacterDatas(string s )
	{ 
		return Main.CharacterList.Where (w => w.Name == s.Replace(Settings.CloneName,"" ) ).FirstOrDefault ();
	}

    /// <summary>Flips the specified to the left.</summary>
    void Flip(bool toTheLeft)
	{
		Vector3 theScale = transform.localScale;
		if (toTheLeft)theScale.x = -Mathf.Abs(theScale.x);
		else theScale.x = Mathf.Abs(theScale.x);
		transform.localScale = theScale;
	}

    /// <summary>敵の生成</summary>
    void GenerateEnnemies()
	{
		GameObject enemy = GameObject.Instantiate(PossibleEnnemies[0],
			new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		generatedEnemyList.Add(enemy);
	}

    /// <summary>プレイヤーのポジション</summary>
    void PositionPlayers()
	{
		try
		{
			int i = 0;
			foreach (var character in Main.CharacterList)
			{
				GameObject go =  GameObject.Instantiate(Resources.Load(Settings.PrefabsPath + character.Name)
					, panel.transform.GetChild(i).transform.position, Quaternion.identity) as GameObject;
				PanelPosition[i] = true;
				i++;
				go.transform.LookAt(generatedEnemyList[0].transform);

				var datas = GetCharacterDatas(go.name);
				datas.Panel = (EnumPanelPosition)i;

				instantiatedCharacterList.Add(go);

				battleUi.SendMessage("HpMpSet", character);
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

	public void Action(EnumBattleAction action)
    {
		battlAction = action;
		selectedEnemy = generatedEnemyList[0];
		battleUi.BroadcastMessage("HideActionMenu");
		if (battlAction == EnumBattleAction.MoveAttack)
			currentState = EnumBattleState.SelectingPanel;
		else 
			PlayerAction();
	}

    /// Shows the menu.
    public void ShowMenu()
	{	
		if (battleUi) {
			battleUi.BroadcastMessage("ShowActionMenu");	
			battleUi.BroadcastMessage ("Start");
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
					BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
					calculatedDamage = BattlePanels.selectedWeapon.Attack 
						+ selectedPlayerDatas.GetAttack () - enemyCharacterdatas.Defense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP = Mathf.Clamp(enemyCharacterdatas.HP 
						- calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);

					Sequence attack = DOTween.Sequence();
					selectedPlayer.transform.LookAt(selectedEnemy.transform.position);
					animator.SetBool("Run", true);
					sequence = attack.Append(selectedPlayer.transform.DOLocalMove(selectedEnemy.transform.position 
						- selectedPlayer.transform.forward * 2, 0.8f)).SetEase(Ease.InSine)
						.AppendCallback(() => animator.SetBool("Run", false))
						.AppendCallback(() => animator.SetTrigger("Attack"));
					//.AppendCallback(() => uiGameObject.BroadcastMessage("ShowPopup",
					//	new object[] { "-" + calculatedDamage.ToString(), selectedEnemy.transform.position }));
					sequence = attack.AppendInterval(1.0f)
						.AppendCallback(() => StartCoroutine(KillCharacter(selectedEnemy, enemyCharacterdatas.HP, 3f)));
					sequence = attack.Append(selectedPlayer.transform
						.DOLocalMove(selectedPlayer.transform.position, 0.5f))
						.Join(selectedPlayer.transform.DOJump(selectedPlayer.transform.position, 3, 1, 0.8f));
					sequence = attack.AppendInterval(1f);

					Destroy( Instantiate (WeaponParticleEffect, selectedEnemy.transform.localPosition, Quaternion.identity),1.5f);
					break;
				case EnumBattleAction.MoveAttack:
					BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
					calculatedDamage = BattlePanels.selectedWeapon.Attack
						+ selectedPlayerDatas.GetAttack() - enemyCharacterdatas.Defense;
					calculatedDamage = Mathf.Clamp(calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP = Mathf.Clamp(enemyCharacterdatas.HP
						- calculatedDamage, 0, enemyCharacterdatas.HP - calculatedDamage);

					selectedPlayer.transform.LookAt(selectedEnemy.transform.position);
					Sequence moveAttack = DOTween.Sequence();
					animator.SetBool("Run", true);
					sequence = moveAttack.Append(selectedPlayer.transform.DOLocalMove(selectedEnemy.transform.position
						- selectedPlayer.transform.forward * 2, 0.8f)).SetEase(Ease.Linear)
						.AppendCallback(() => animator.SetTrigger("Attack"));
					//.AppendCallback(() => uiGameObject.BroadcastMessage("ShowPopup",
					//	new object[] { "-" + calculatedDamage.ToString(), selectedEnemy.transform.position }));
					sequence = moveAttack.AppendInterval(1f)
						.AppendCallback(() => StartCoroutine(KillCharacter(selectedEnemy, enemyCharacterdatas.HP, 3f)))
						.AppendCallback(() => selectedPlayer.transform.LookAt(panel.transform.GetChild((int)selectedPlayerDatas.Panel).transform.position));
					sequence = moveAttack.Append(selectedPlayer.transform
						.DOLocalMove(panel.transform.GetChild((int)selectedPlayerDatas.Panel).transform.position, 0.8f))
						.AppendCallback(() => animator.SetBool("Run", false));
					sequence = moveAttack.Append(selectedPlayer.transform.DOLookAt(selectedEnemy.transform.position, 0.2f));
					sequence = moveAttack.AppendInterval(1f);

					Destroy(Instantiate(WeaponParticleEffect, selectedEnemy.transform.localPosition, Quaternion.identity), 1.5f);
					break;
				case EnumBattleAction.Magic:
					calculatedDamage = BattlePanels.selectedSpell.Attack + selectedPlayerDatas.GetMagic () - enemyCharacterdatas.MagicDefense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP =Mathf.Clamp ( enemyCharacterdatas.HP - calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);
					selectedPlayerDatas.MP = Mathf.Clamp ( selectedPlayerDatas.MP - BattlePanels.selectedSpell.ManaAmount, 0 
						,selectedPlayerDatas.MP - BattlePanels.selectedSpell.ManaAmount);
					battleUi.SendMessage("HpMpSet", selectedPlayerDatas);
					
					var ennemyEffect = Resources.Load<GameObject>(Settings.PrefabsPath + BattlePanels.selectedSpell.ParticleEffect);
					Destroy(Instantiate(ennemyEffect, selectedEnemy.transform.localPosition, Quaternion.identity), 0.5f);

					var playerEffect = Resources.Load<GameObject>(Settings.PrefabsPath + Settings.MagicAuraEffect);
					Destroy( Instantiate (playerEffect, selectedPlayer.transform.localPosition, Quaternion.identity),0.4f);
					SoundManager.StaticPlayOneShot(BattlePanels.selectedSpell.SoundEffect, Vector3.zero);
					break;
				case EnumBattleAction.Item:
					//calculatedDamage = BattlePanels.SelectedItem.Attack - enemyCharacterdatas.MagicDefense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					enemyCharacterdatas.HP = Mathf.Clamp ( enemyCharacterdatas.HP - calculatedDamage, 0 , enemyCharacterdatas.HP - calculatedDamage);
					Destroy( Instantiate (MagicParticleEffect, selectedEnemy.transform.localPosition, Quaternion.identity),1.7f);
					SoundManager.ItemSound();
					break;
				default:
					break;
			}
		}
		selectedEnemy = null;
		NextBattleSequence();
	}

    /// Enemies the attack.
    public void EnemyAttack(GameObject playerToAttack, CharactersData playerToAttackDatas )
	{
		var go = sequenceEnumerator.Current.Second;
		mainCamera.transform.position = go.transform.position
			+ playerToAttack.transform.right * 5 + new Vector3(0, 5, 0)
			- selectedPlayer.transform.forward * 5;
		mainCamera.transform.LookAt(playerToAttack.transform);

		var enemyCharacterdatas = go.GetComponent<EnemyCharacterDatas> ();
		var animator = go.GetComponent<Animator>();
		int calculatedDamage = 0;

		if (enemyCharacterdatas != null && selectedPlayerDatas != null) {
			switch (battlAction) {
				case EnumBattleAction.Weapon:
					calculatedDamage = enemyCharacterdatas.Attack - playerToAttackDatas.Defense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage) * -1;
					playerToAttackDatas.HP = Mathf.Clamp (playerToAttackDatas.HP + calculatedDamage 
						, 0 ,playerToAttackDatas.HP + calculatedDamage);

					animator.SetTrigger("Attack");
					Sequence attack = DOTween.Sequence();
					sequence = attack.Append(go.transform.DOLookAt(playerToAttack.transform.position, 0.2f));
					sequence = attack.Append(go.transform.DOLocalMove(playerToAttack.transform.position
						+ playerToAttack.transform.forward * 4, 1f).SetEase(Ease.OutCirc))
						.AppendCallback(() => battleUi.SendMessage("HpMpSet", playerToAttackDatas))
						.AppendCallback(() => battleUi.SendMessage("ShowPopup",
							new object[] {calculatedDamage.ToString(), playerToAttack.transform.position }));
					sequence = attack.AppendInterval(2f)
						.AppendCallback(() => StartCoroutine(KillCharacter(playerToAttack, playerToAttackDatas.HP, 2f)));
					sequence = attack.Append(go.transform.DOLocalMove(go.transform.position, 1).SetEase(Ease.OutCirc));
					
					Destroy(Instantiate(WeaponParticleEffect, playerToAttack.transform.localPosition, Quaternion.identity),1.5f);
					break;
				default:
					calculatedDamage = enemyCharacterdatas.Attack - playerToAttackDatas.Defense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					playerToAttackDatas.HP = Mathf.Clamp (playerToAttackDatas.HP - calculatedDamage , 0 ,playerToAttackDatas.HP - calculatedDamage);
					battleUi.SendMessage("HpMpSet", playerToAttackDatas);
					Destroy( Instantiate (WeaponParticleEffect, playerToAttack.transform.localPosition, Quaternion.identity),1.5f);
					break;
			}
		}
		//if (playerToAttackDatas.HP <= 0) KillCharacter (playerToAttack);
		selectedEnemy = null;
	}

	IEnumerator KillCharacter(GameObject go, int HP, float time)
	{
		if (HP <= 0) go.GetComponent<Animator>().SetTrigger("Death");

		yield return new WaitForSeconds(time);

		if (HP <= 0)
		{
			//go.SetActive(false);
			turnByTurnSequenceList.RemoveAll(r => r.Second.GetInstanceID() == go.GetInstanceID());
			var id = sequenceEnumerator.Current.Second.GetInstanceID();
			sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
			sequenceEnumerator.MoveNext();
			while (sequenceEnumerator.Current.Second.GetInstanceID() != id) sequenceEnumerator.MoveNext();
		}
	}
}