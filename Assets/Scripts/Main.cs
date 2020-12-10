using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Security.Permissions;
using System.Reflection;
using System.IO;

[Serializable ()]
public class Main : MonoBehaviour
{
    //最初のシーン読み込み
    public string FirstSceneToLoad;
    //読み込まれたシーン
    public static string FirstSceneLoaded;

    //キャラクターリスト
    public static List<CharactersData> CharacterList = new List<CharactersData>();
    //装備リスト
    public static List<ItemsData> EquipmentList = new List<ItemsData>();
    //アイテムリスト
    public static List<ItemsData> ItemList = new List<ItemsData>();
    //遷移先のシーン
    public static string PreviousScene = string.Empty;
    //現在のシーン
    public static string CurrentScene = string.Empty;

    [NonSerialized]
    //プレイヤーのポジション
    public static Vector3 PlayerPosition = default(Vector3);
    //プレイヤーのxポジション
    public static int PlayerXPosition = default(int);
    //プレイヤーのyポジション
    public static int PlayerYPosition = default(int);
    //プレイヤーのzポジション
    public static int PlayerZPosition = default(int);
    //コントロールブロック
    public static bool ControlsBlocked = false;
    //シーンに入ったばっかりかどうか
    public static bool JustEnteredTheScreen = false;

    //このインスタンスがゲームの一時停止中かどうかを取得
    public static bool IsGamePaused
    {
        get { return Time.timeScale == 0; }
    }

    //ゲームのポーズ
    public static void PauseGame(bool pause)
    {
        if (pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    //Awake関数　初期値設定
    void Awake()
    {
        //最初にロードするシーンを確認
        FirstSceneLoaded = this.FirstSceneToLoad;

        //競合する他のインスタンスがあるかどうかの確認
        if (CharacterList != null || EquipmentList != null || ItemList != null)
        {
            //あったら他のインスタンスを消去
            Destroy(gameObject);
        }

        //初期値設定
        PreviousScene = string.Empty;
        CurrentScene = string.Empty;
        PlayerPosition = default(Vector3);
        PlayerXPosition = default(int);
        PlayerYPosition = default(int);
        PlayerZPosition = default(int);
        ControlsBlocked = false;
        JustEnteredTheScreen = false;

        //インスタンスの確保
        CharacterList = new List<CharactersData>();
        EquipmentList = new List<ItemsData>();
        ItemList = new List<ItemsData>();

        //ゲームデータの初期化
        Datas.PopulateDatas();

        //ニューゲームのためのデータの初期化（初期設定）
        InitializeDatas();

        //ロード中にデータを破壊しないようにする
        DontDestroyOnLoad(gameObject);

        //シーンをロード
        SceneManager.LoadScene(FirstSceneLoaded);
    }

    //プレイヤーのポジションを初期化
    public static void InitializePlayerPosition()
    {
        PlayerPosition = default(Vector3);
        PlayerXPosition = default(int);
        PlayerYPosition = default(int);
        PlayerZPosition = default(int);
    }

    /// バトルデータ（装備・魔法・アイテム）の初期化
    public void InitializeDatas()
    {
        CharacterList.Add(Datas.CharactersData[1]);
        CharacterList[0].Head = Datas.ItemsData[12];
        EquipmentList.Add(Datas.ItemsData[12]);
        CharacterList[0].Body = Datas.ItemsData[22];
        EquipmentList.Add(Datas.ItemsData[22]);
        CharacterList[0].RightHand = Datas.ItemsData[1];
        EquipmentList.Add(Datas.ItemsData[1]);
        CharacterList[0].LeftHand = Datas.ItemsData[17];
        EquipmentList.Add(Datas.ItemsData[17]);
        CharacterList[0].SpellsList.AddRange(Datas.SpellsData.Where(w => w.Value.AllowedCharacterType == EnumCharacterType.Warrior).Select(s => s.Value));

        //CharacterList.Add(Datas.CharactersData[2]);
        //CharacterList[1].Head = Datas.ItemsData[15];
        //EquipmentList.Add(Datas.ItemsData[15]);
        //CharacterList[1].Body = Datas.ItemsData[26];
        //EquipmentList.Add(Datas.ItemsData[26]);
        //CharacterList[1].RightHand = Datas.ItemsData[9];
        //EquipmentList.Add(Datas.ItemsData[9]);
        //CharacterList[1].SpellsList.AddRange(Datas.SpellsData.
        //    Where(w => w.Value.AllowedCharacterType == EnumCharacterType.Wizard).Select(s => s.Value));

        foreach (var item in EquipmentList)
        {
            item.isEquiped = true;
        }

        ItemList.Add(Datas.ItemsData[29]);
        ItemList.Add(Datas.ItemsData[30]);
        ItemList.Add(Datas.ItemsData[31]);
    }

    #region ISerializable implementation
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="context">The context.</param>
    public Main(SerializationInfo info, StreamingContext context)
    {
        CharacterList = (List<CharactersData>)info.GetValue("characterList", typeof(List<CharactersData>));
        EquipmentList = (List<ItemsData>)info.GetValue("equipmentList", typeof(List<ItemsData>));
        ItemList = (List<ItemsData>)info.GetValue("itemList", typeof(List<ItemsData>));
        PreviousScene = string.Empty;
        CurrentScene = (string)info.GetValue("currentScene", typeof(string));
        PlayerXPosition = (int)info.GetValue("playerXPosition", typeof(int));
        PlayerYPosition = (int)info.GetValue("playerYPosition", typeof(int));
        PlayerZPosition = (int)info.GetValue("playerZPosition", typeof(int));
        PlayerPosition = new Vector3(PlayerXPosition, PlayerYPosition, PlayerZPosition);
        ControlsBlocked = false;
        JustEnteredTheScreen = false;
    }
#pragma warning disable  // Type or member is obsolete
    /// <summary>
    /// Gets the object data.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="context">The context.</param>
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
#pragma warning restore  // Type or member is obsolete
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("characterList", CharacterList);
        info.AddValue("equipmentList", EquipmentList);
        info.AddValue("itemList", ItemList);
        info.AddValue("currentScene", CurrentScene);
        info.AddValue("playerXPosition", PlayerPosition.x);
        info.AddValue("playerYPosition", PlayerPosition.y);
        info.AddValue("playerZPosition", PlayerPosition.z);

    }

    /// <summary>
    /// Saves this instance.
    /// </summary>
    public static void Save()
    {

#pragma warning disable
        Main data = new Main();
#pragma warning restore 
        Stream stream = File.Open(Settings.SavedFileName, FileMode.Create);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Writing Information");
        bformatter.Serialize(stream, data);
        stream.Close();

    }

    /// <summary>
    /// Loads this instance.
    /// </summary>
    public static void Load()
    {


        //Main data = new Main ();
        Stream stream = File.Open(Settings.SavedFileName, FileMode.Open);
        BinaryFormatter bformatter = new BinaryFormatter();
        bformatter.Binder = new VersionDeserializationBinder();
        Debug.Log("Reading Data");
        bformatter.Deserialize(stream);
        stream.Close();

        if (!string.IsNullOrEmpty(Main.CurrentScene))
            SceneManager.LoadScene(Main.CurrentScene);
        else
            SceneManager.LoadScene(Settings.MainMenuScene);

    }


    /// <summary>
    /// Class VersionDeserializationBinder. This class cannot be inherited.
    /// </summary>
    public sealed class VersionDeserializationBinder : SerializationBinder
    {
        /// <summary>
        /// Binds to type.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>Type.</returns>
        public override Type BindToType(string assemblyName, string typeName)
        {
            if (!string.IsNullOrEmpty(assemblyName) && !string.IsNullOrEmpty(typeName))
            {
                Type typeToDeserialize = null;


                assemblyName = Assembly.GetExecutingAssembly().FullName;
                // The following line of code returns the type. 
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
                return typeToDeserialize;
            }
            return null;
        }

    }

    #endregion
}
