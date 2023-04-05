using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSound : MonoBehaviour
{
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioSource driftSound;
    [SerializeField] private AudioSource boosterSound;
    [SerializeField] private ParticleSystem[] Nitros;

    void Update()
    {
        Driving();
        Booster();
        PauseSound();
    }

    void Driving()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            DrivingSound();
        else
            DrivingSound();

        foreach (TrailRenderer element in GameManager.Instance.tireMarks)
        {
            if (element.emitting == true)
            {
                if (engineSound.isPlaying == true && driftSound.isPlaying == false)
                {
                    engineSound.Stop();
                    driftSound.Play();
                }
            }
            else
            {
                if (engineSound.isPlaying == false && driftSound.isPlaying == true)
                {
                    driftSound.Stop();
                    engineSound.Play();
                }
                else if (engineSound.isPlaying == false && GameManager.Instance.Speed > 0)
                    engineSound.Play();
                else if (engineSound.isPlaying == true && GameManager.Instance.Speed == 0)
                    engineSound.Stop();
            }
        }
    }

    void DrivingSound()
    {
        if (GameManager.Instance.Speed <= 50.0f && GameManager.Instance.Speed >= 0.0f)
            engineSound.volume = GameManager.Instance.Speed * 0.01f;
    }

    void Booster()
    {
        foreach (ParticleSystem element in Nitros)
        {
            if (GameManager.Instance.useBooster == true)
                element.Play();
            else
                element.Stop();
        }

        if (GameManager.Instance.BoosterTime >= 1.0f && GameManager.Instance.useBooster == true)
        {
            if (boosterSound.isPlaying == false)
                boosterSound.Play();
        }
    }

    void PauseSound()
    {
        if (Time.timeScale == 0)
        {
            engineSound.Pause();
            driftSound.Pause();
            boosterSound.Pause();
        }
        else
        {
            engineSound.UnPause();
            driftSound.UnPause();
            boosterSound.UnPause();
        }
    }
}
