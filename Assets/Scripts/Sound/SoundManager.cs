using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{

    /// This variable indicate if the music should restart from the begining even if it's the same as the previous scene
    public bool IgnorePreviousSceneMusic = false;
    /// The current singleton instance
    private static SoundManager _instance;

    /// The property of the current singleton instance
    public static SoundManager instance
    {
        get
        {
            return _instance;
        }
    }

    // Awake is called in the initialization 
    /// Awake this instance.
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            var audiosource = GetComponent<AudioSource>();
            var oldAudioSource = instance.GetComponent<AudioSource>();

            if (oldAudioSource.clip.name != audiosource.clip.name || IgnorePreviousSceneMusic)
            {
                Destroy(instance.gameObject);
                _instance = this;
            }
            if (this != _instance) Destroy(this.gameObject);
        }
    }


    // Update is called once per frame
    void Update() { }

    /// <summary>Plays the one shot.</summary>
    public void PlayOneShot(string soundName, float volume = -1)
    {
        var instantiatedSound = Resources.Load<AudioClip>(Settings.SoundPath + soundName);
        if (volume >= 0) instance.gameObject.GetComponent<AudioSource>().PlayOneShot(instantiatedSound, volume);
        else instance.gameObject.GetComponent<AudioSource>().PlayOneShot(instantiatedSound);
    }

    /// <summary>Plays the background music.</summary>
    public void PlayBackgroundMusic(string musicTitle, float volume = -1)
    {
        var instantiatedSound = Resources.Load<AudioClip>(Settings.SoundPath + musicTitle);
        if (volume >= 0) instance.gameObject.GetComponent<AudioSource>().volume = volume;
        instance.gameObject.GetComponent<AudioSource>().clip = instantiatedSound;
        instance.gameObject.GetComponent<AudioSource>().Play();
    }

    /// <summary>Statics the play one shot.</summary>
    public static void StaticPlayOneShot(string soundName, Vector3 position, float volume = -1)
    {
        var instantiatedSound = Resources.Load<AudioClip>(Settings.SoundPath + soundName);
        var lastTimeScale = Time.timeScale;
        Time.timeScale = 1f;
        if (volume >= 0) AudioSource.PlayClipAtPoint(instantiatedSound, position, volume);
        else AudioSource.PlayClipAtPoint(instantiatedSound, position);
        Time.timeScale = lastTimeScale;
    }

    /// <summary>Playing UI sound.</summary>
    public static void UISound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.ChatSound, Vector3.zero, volume);
    }

    /// <summary>Playing Chat sound.</summary>
    public static void ChatSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.ChatSound, Vector3.zero, volume);
    }

    public static void ItemSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.ItemSound, Vector3.zero, volume);
    }

    public static void GameOverMusic(float volume = 5)
    {
        SoundManager.StaticPlayOneShot(Settings.GameOverMusic, Vector3.zero, volume);
    }

    public static void WinningMusic(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.WinningMusic, Vector3.zero, volume);
    }

    public static void BattleMusic(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.BattleMusic, Vector3.zero, volume);
    }

    public static void FieldMusic(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.FIeldMusic, Vector3.zero, volume);
    }

    public static void TitleMusic(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.TitleMusic, Vector3.zero, volume);
    }

    public static void TurnSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.TurnSound, Vector3.zero, volume);
    }

    public static void StartSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.StartSound, Vector3.zero, volume);
    }

    public static void EncountSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.EncountSound, Vector3.zero, volume);
    }

    public static void MagicSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.MagicSound, Vector3.zero, volume);
    }

    public static void HoatSound(float volume = -1)
    {
        SoundManager.StaticPlayOneShot(Settings.HoatSound, Vector3.zero, volume);
    }
}