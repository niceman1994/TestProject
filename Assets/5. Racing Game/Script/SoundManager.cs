using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : ManagerSingleton<SoundManager>
{
    public GameObject Pause;
    public Button Exitbutton;
    public Button Restartbutton;
    public AudioSource[] GameBGM;
    public AudioSource[] GameSound;

    void Start()
    {
        GameBGM[0].Play();
    }

    void Update()
    {
        if (!GameBGM[0].isPlaying && !GameBGM[1].isPlaying)
            GameBGM[1].Play();

        if (GameManager.Instance.IntroCanvas.activeInHierarchy == false)
        {
            foreach (AudioSource element in GameBGM)
                element.Stop();
        }

        setPause();
    }

    public void ClickExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            Pause.SetActive(true);
    }

    public void ClickRestart()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            Pause.SetActive(true);
    }

    void setPause()
    {
        if (GameManager.Instance.CountNum == 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Pause.activeInHierarchy == false)
                {
                    Pause.SetActive(true);
                    GameSound[0].Play();
                    Time.timeScale = 0;
                }
                else
                {
                    Pause.SetActive(false);
                    GameSound[0].Play();
                    Time.timeScale = 1;
                }
            }
        }
    }
}
