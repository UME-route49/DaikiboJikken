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

    //敵を取得
    public List<GameObject> PossibleEnnemies = new List<GameObject>();
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

	//キャラクターのパネルポジション
	private bool[] PanelPosition = new bool[8];
	int panelSelect = 0;

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

    void Start()
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

			//NextBattleSequence();
		}
		else if (currentState == EnumBattleState.PlayerTurn)
		{
            battleUi.SendMessage("LogText", selectedPlayer.name + "の番です。");
			SoundManager.TurnSound();
			battleUi.SendMessage("ShowActionMenu");
			battleUi.BroadcastMessage("Start");

			selectedPlayer.transform.LookAt(generatedEnemyList[0].transform.position);
			mainCamera.transform.position = selectedPlayer.transform.position + new Vector3(0, 2, 0)
				- selectedPlayer.transform.forward * 4;
			mainCamera.transform.LookAt(generatedEnemyList[0].transform);

			currentState = EnumBattleState.None;
		}
		else if(currentState == EnumBattleState.SelectingPanel)
        {
			mainCamera.transform.position = new Vector3(0, 30, 0);
			mainCamera.transform.LookAt(generatedEnemyList[0].transform.position);

			panel.SetActive(true);
				
			var characterData = GetCharacterDatas(selectedPlayer.name);

			if(panelSelect == 0)
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
				panelSelect = 0;
				panel.SetActive(false);
				PlayerAction();
			}
        }
		else if (currentState == EnumBattleState.PlayerWon)
		{
			battleUi.SendMessage("LogText", GameTexts.PlayerWon);
			battleUi.SendMessage("HideActionMenu");
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
			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" + GameTexts.PlayerXP + totalXP + "\n\nSPACEでリザルトを終える。";
			battleUi.SendMessage("ShowDropMenu", textTodisplay);

			var soundManager = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			soundManager.GetComponent<AudioSource>().Stop();
			SoundManager.WinningMusic();

			currentState = EnumBattleState.EndBattle;
		}
		else if (currentState == EnumBattleState.EnemyWon)
		{
			battleUi.SendMessage("LogText", GameTexts.EnemyWon);
			battleUi.SendMessage("HideActionMenu");

			var textTodisplay = GameTexts.EndOfTheBattle + "\n\n" + GameTexts.YouLost + "\n\nSPACEでリザルトを終える。";
			battleUi.BroadcastMessage("ShowDropMenu", textTodisplay);

			var soundManager = GameObject.FindGameObjectsWithTag(Settings.Music).FirstOrDefault();
			soundManager.GetComponent<AudioSource>().Stop();
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

    /// <summary>敵の生成</summary>
    void GenerateEnnemies()
	{
		GameObject enemy = GameObject.Instantiate(PossibleEnnemies[0],
			new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		var enemyData = enemy.GetComponent<EnemyCharacterDatas>();
		enemyData.Foward = EnumPanelPosition.UP;
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
				GameObject go =  Instantiate(Resources.Load<GameObject>(Settings.PrefabsPath + character.Name)
					, panel.transform.GetChild(i).transform.position, Quaternion.identity);
				PanelPosition[i] = true;
				go.transform.LookAt(generatedEnemyList[0].transform);

				var datas = GetCharacterDatas(go.name);
				datas.Panel = (EnumPanelPosition)i;

				instantiatedCharacterList.Add(go);

				battleUi.SendMessage("HpMpSet", character);

				i++;
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
		var x = turnByTurnSequenceList.Where(w => w.First == EnumPlayerOrEnemy.Enemy).Count();
		var y = turnByTurnSequenceList.Where(w => w.First == EnumPlayerOrEnemy.Player).Count();
		if (x <= 0)
		{
			currentState = EnumBattleState.PlayerWon;
			return;
		}
		else if (y <= 0)
		{
			currentState = EnumBattleState.EnemyWon;
			return;
		}

		if (sequenceEnumerator.MoveNext ()) 
		{
			if (sequenceEnumerator.Current.First == EnumPlayerOrEnemy.Player) {

				currentState = EnumBattleState.PlayerTurn;

				selectedPlayer = sequenceEnumerator.Current.Second;
				selectedPlayerDatas = GetCharacterDatas(selectedPlayer.name);
				BattlePanels.selectedCharacter = selectedPlayerDatas;
			} 
			else if (sequenceEnumerator.Current.First == EnumPlayerOrEnemy.Enemy) {
				currentState = EnumBattleState.EnemyTurn;
			}
		} 
		else 
		{
			sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
			NextBattleSequence();
		}
	}

	public void Action(EnumBattleAction action)
    {
		battlAction = action;
		selectedEnemy = generatedEnemyList[0];
		battleUi.SendMessage("HideActionMenu");
		if (battlAction == EnumBattleAction.MoveAttack)
			currentState = EnumBattleState.SelectingPanel;
		else PlayerAction();
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
		var enemy = selectedEnemy;
		var enemyData = enemy.GetComponent<EnemyCharacterDatas> ();
		var animator = selectedPlayer.GetComponent<Animator>();
		var damage = 0;
		var popPos = RectTransformUtility.WorldToScreenPoint(mainCamera.GetComponent<Camera>()
			, enemy.transform.position + new Vector3(1, 2, 1));

		if (enemyData != null && selectedPlayerDatas != null) {
			switch (battlAction) {
				case EnumBattleAction.Weapon:
					BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
					var weapon = BattlePanels.selectedWeapon;

					damage = weapon.Attack + selectedPlayerDatas.GetAttack () - enemyData.Defense; 
					damage = Mathf.Clamp (damage, 0, damage);
					enemyData.HP = Mathf.Clamp(enemyData.HP - damage, 0 , enemyData.HP - damage);

					selectedPlayer.transform.LookAt(enemy.transform.position);
					animator.SetBool("Run", true);

					Sequence attack = DOTween.Sequence();
					sequence = attack.Append(selectedPlayer.transform.DOLocalMove(enemy.transform.position
						- selectedPlayer.transform.forward * 3, 0.7f)).SetEase(Ease.InSine)
						.AppendCallback(() => animator.SetBool("Run", false))
						.AppendCallback(() => animator.SetTrigger("Attack"));
					sequence = attack.AppendInterval(0.8f)
						.AppendCallback(() => battleUi.SendMessage("ShowPopup", new object[] {damage.ToString(), popPos}))
						.AppendCallback(() => KillCharacter(enemy, enemyData.HP));
					sequence = attack.Append(selectedPlayer.transform
						.DOLocalMove(selectedPlayer.transform.position, 0.4f))
						.Join(selectedPlayer.transform.DOJump(selectedPlayer.transform.position, 3, 1, 0.5f));
					sequence = attack.AppendInterval(1f);
					break;

				case EnumBattleAction.MoveAttack:
					BattlePanels.selectedWeapon = BattlePanels.selectedCharacter.RightHand;
					weapon = BattlePanels.selectedWeapon;

					damage = weapon.Attack + selectedPlayerDatas.GetAttack() - enemyData.Defense;
					damage = Mathf.Clamp(damage, 0, damage);
					enemyData.HP = Mathf.Clamp(enemyData.HP
						- damage, 0, enemyData.HP - damage);

					selectedPlayer.transform.LookAt(enemy.transform.position);
					animator.SetBool("Run", true);

					Sequence moveAttack = DOTween.Sequence();
					sequence = moveAttack.Append(selectedPlayer.transform.DOLocalMove(enemy.transform.position
						- selectedPlayer.transform.forward * 3, 0.7f)).SetEase(Ease.Linear)
						.AppendCallback(() => animator.SetTrigger("Attack"));
					sequence = moveAttack.AppendInterval(0.8f)
						.AppendCallback(() => battleUi.SendMessage("ShowPopup", new object[] { damage.ToString(), popPos }))
						.AppendCallback(() => KillCharacter(enemy, enemyData.HP))
						.AppendCallback(() => selectedPlayer.transform.LookAt(panel.transform.GetChild((int)selectedPlayerDatas.Panel).transform.position));
					sequence = moveAttack.Append(selectedPlayer.transform
						.DOLocalMove(panel.transform.GetChild((int)selectedPlayerDatas.Panel).transform.position, 0.7f))
						.AppendCallback(() => animator.SetBool("Run", false));
					sequence = moveAttack.Append(selectedPlayer.transform.DOLookAt(enemy.transform.position, 0.2f));
					sequence = moveAttack.AppendInterval(1f);
					break;

				case EnumBattleAction.Magic:
					var spell = BattlePanels.selectedSpell;
					var playerParticle = Resources.Load<GameObject>(Settings.PrefabsPath + Settings.MagicAuraEffect);
					var enemyParticle = Resources.Load<GameObject>(Settings.PrefabsPath + spell.Particle);
					var enemyParticleTime = enemyParticle.GetComponent<ParticleSystem>().time;

					damage = spell.Attack + selectedPlayerDatas.GetMagic () - enemyData.MagicDefense; 
					damage = Mathf.Clamp (damage, 0, damage);
					enemyData.HP = Mathf.Clamp ( enemyData.HP - damage, 0 , enemyData.HP - damage);
					selectedPlayerDatas.MP = Mathf.Clamp ( selectedPlayerDatas.MP - spell.ManaAmount, 0 
						,selectedPlayerDatas.MP - spell.ManaAmount);
					battleUi.SendMessage("HpMpSet", selectedPlayerDatas);

					Sequence magic = DOTween.Sequence();
					mainCamera.transform.position = selectedPlayer.transform.position
						+ selectedPlayer.transform.forward * 5 + new Vector3(0, 2, 0);
					mainCamera.transform.LookAt(selectedPlayer.transform.position + new Vector3(0, 1, 0));
					animator.SetTrigger("Magic");
					StartCoroutine(ParticleInstantiate(playerParticle, selectedPlayer, 1.5f));
					SoundManager.MagicSound();

					sequence = magic.AppendInterval(3f)
						.AppendCallback(() => mainCamera.transform.position = enemy.transform.position
							+ enemy.transform.forward * 10 + new Vector3(0, 3, 0))
						.AppendCallback(() => mainCamera.transform.LookAt(enemy.transform.position + new Vector3(0, 2, 0)))
						.AppendCallback(() => StartCoroutine(ParticleInstantiate(enemyParticle, enemy, 5f)))
						.AppendCallback(() => SoundManager.HoatSound())
						.AppendCallback(() => battleUi.SendMessage("ShowPopup", new object[] { damage.ToString(), popPos }))
						.AppendCallback(() => KillCharacter(enemy, enemyData.HP));
					//.AppendCallback(() => selectedEnemy.GetComponent<Animator>().SetTrigger("Hit"));
					sequence = magic.AppendInterval(enemyParticleTime+2f);
					break;
				case EnumBattleAction.Item:
					var item = BattlePanels.selectedItem;
					var itemParticle = Resources.Load(Settings.PrefabsPath + item.Particle) as GameObject;

					if (item.itemType == EnumItemType.Damage)
					{
						enemyData.HP = Mathf.Clamp(enemyData.HP - item.HpDamege
							, 0, enemyData.HP - item.HpDamege);
						enemyData.MP = Mathf.Clamp(enemyData.MP - item.MpDamege
							, 0, enemyData.MP - item.MpDamege);
						mainCamera.transform.position = enemy.transform.transform.position
							+ enemy.transform.forward * 5;
						mainCamera.transform.LookAt(enemy.transform.position);
						StartCoroutine(ParticleInstantiate(itemParticle, enemy, 3f));
					}
					else if(item.itemType == EnumItemType.Heal)
					{
						selectedPlayerDatas.HP = Mathf.Clamp(selectedPlayerDatas.HP + item.HpDamege,
							selectedPlayerDatas.HP + item.HpDamege, selectedPlayerDatas.MaxHP);
						selectedPlayerDatas.MP = Mathf.Clamp(selectedPlayerDatas.MP + item.MpDamege,
							selectedPlayerDatas.MP + item.MpDamege, selectedPlayerDatas.MaxMP);
						battleUi.SendMessage("HpMpSet", selectedPlayerDatas);
						mainCamera.transform.position = selectedPlayer.transform.transform.position
							+ selectedPlayer.transform.forward * 5;
						mainCamera.transform.LookAt(selectedPlayer.transform.position);
						StartCoroutine(ParticleInstantiate(itemParticle, selectedPlayer, 3f));
						SoundManager.ItemSound();
					}

					Sequence useItem = DOTween.Sequence();
					sequence = useItem.AppendInterval(3f);
					break;
				default:
					break;
			}
		}
		selectedEnemy = null;
		//NextBattleSequence();
	}

	/// Enemies the attack.
	public void EnemyAttack(GameObject target, CharactersData targetData)
	{
		var enemy = sequenceEnumerator.Current.Second;
		mainCamera.transform.position = target.transform.position
			+ target.transform.right * 5 + new Vector3(0, 5, 0) + target.transform.forward * 5;
		mainCamera.transform.LookAt(target.transform);

		var enemyData = enemy.GetComponent<EnemyCharacterDatas> ();
		var animator = enemy.GetComponent<Animator>();
		int calculatedDamage = 0;
		var popPos = RectTransformUtility.WorldToScreenPoint(mainCamera.GetComponent<Camera>()
			, target.transform.position + new Vector3(1, 2, 1));

		if (enemyData != null && selectedPlayerDatas != null) {
			switch (battlAction) {
				case EnumBattleAction.Weapon:
					var attaceParticle = Resources.Load<GameObject>(Settings.PrefabsPath + "Sword");

					calculatedDamage = enemyData.Attack - targetData.Defense; 
					calculatedDamage = Mathf.Clamp (calculatedDamage, 0, calculatedDamage);
					targetData.HP = Mathf.Clamp (targetData.HP - calculatedDamage 
						, 0 ,targetData.HP - calculatedDamage);

					animator.SetTrigger("Attack");
					enemy.transform.LookAt(target.transform.position);
					enemyData.Foward = targetData.Panel;

					Sequence attack = DOTween.Sequence();
					sequence = attack.Append(enemy.transform.DOLocalMove(target.transform.position
						+ target.transform.forward * 4, 1f).SetEase(Ease.OutCirc))
						.AppendCallback(() => battleUi.SendMessage("HpMpSet", targetData));
					sequence = attack.AppendInterval(1f)
						.AppendCallback(() => battleUi.SendMessage("ShowPopup", new object[] { calculatedDamage.ToString(), popPos }))
						.AppendCallback(() => KillCharacter(target, targetData.HP));
					sequence = attack.Append(enemy.transform.DOLocalMove(enemy.transform.position, 1).SetEase(Ease.OutCirc));
					break;
				default:
					break;
			}
		}
		selectedEnemy = null;
	}

	void KillCharacter(GameObject target, int HP)
	{
		Debug.Log(HP);
		if (HP <= 0)
		{
			target.GetComponent<Animator>().SetTrigger("Death");
			turnByTurnSequenceList.RemoveAll(r => r.Second.GetInstanceID() == target.GetInstanceID());
			var id = sequenceEnumerator.Current.Second.GetInstanceID();
			sequenceEnumerator = turnByTurnSequenceList.GetEnumerator();
			sequenceEnumerator.MoveNext();
			while (sequenceEnumerator.Current.Second.GetInstanceID() != id) sequenceEnumerator.MoveNext();
		}
		NextBattleSequence();
	}

	IEnumerator ParticleInstantiate(GameObject particle, GameObject target, float time)
    {
		GameObject go = Instantiate(particle, target.transform.position, target.transform.rotation);
		go.transform.localScale = target.transform.localScale;

		yield return new WaitForSeconds(time);

		Destroy(go);
    }
}