using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : ManagerSingleton<SoundManager>
{
    public AudioSource introSound;

    void Start()
    {
        introSound.Play();
    }

    void Update()
    {
        if (GameManager.Instance.IntroCanvas.activeInHierarchy == false)
            introSound.Stop();
    }
}
