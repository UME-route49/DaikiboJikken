using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldController : MonoBehaviour
{

    public float interval = 10;

    FadeOut fadeOut;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.FieldMusic();
        fadeOut = FindObjectOfType<FadeOut>();
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            interval = interval - Time.deltaTime;

            if (interval < 0)
            {
                interval = 10;
                playerController.canControl = false;
                playerController.GetComponent<Animator>().speed = 0;

                var go = GameObject.FindObjectOfType<SoundManager>();
                go.GetComponent<AudioSource>().Stop();
                SoundManager.EncountSound();
                fadeOut.StartCoroutine(fadeOut.Fadeout("SampleBattleScene", 2f));
            }
        }
    }
}
