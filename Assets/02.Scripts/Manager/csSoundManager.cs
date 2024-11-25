using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSoundManager : MonoBehaviour
{
    public static csSoundManager instance;

    AudioSource myAudio;
    public AudioClip fireExtinguisherSound;
    public AudioClip explosionSound;
    public AudioClip sparkSound;
    public AudioClip successSound;
    public AudioClip failSound;

    void Awake()
    {
        if(csSoundManager.instance == null)
        {
            csSoundManager.instance = this;
        }
    }

    void Start()
    {
        myAudio = gameObject.GetComponent<AudioSource>();
    }

    public void PlayfireExtinguisherSound()
    {
        myAudio.PlayOneShot(fireExtinguisherSound);
    }

    public void PlayExplosionSound()
    {
        myAudio.PlayOneShot(explosionSound);
    }

    public void PlaySparkSound()
    {
        myAudio.PlayOneShot(sparkSound);
    }

    public void PlaySuccessSound()
    {
        myAudio.PlayOneShot(successSound);
    }

    public void PlayFailSound()
    {
        myAudio.PlayOneShot(failSound);
    }
}
