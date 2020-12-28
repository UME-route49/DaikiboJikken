using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldController : MonoBehaviour
{

    private float interval = 10;
    FadeOut fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.FieldMusic();
        fadeOut = FindObjectOfType<FadeOut>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal")!=0 || Input.GetAxisRaw("Vertical") != 0)
        {
            interval = interval - Time.deltaTime;

            if (interval < 0) fadeOut.StartCoroutine("Fadeout", "SampleBattleScene");
        }
    }
}
