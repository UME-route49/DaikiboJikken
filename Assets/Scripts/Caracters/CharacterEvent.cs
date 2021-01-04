using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEvent : MonoBehaviour
{ 
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayOneShotSE(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void PlayParticle(GameObject particle)
    {
        var go = Instantiate(particle, transform.position + transform.forward*3 + new Vector3(0, 1, 0), transform.rotation);
        
        Destroy(go, 2f);
    }
}
