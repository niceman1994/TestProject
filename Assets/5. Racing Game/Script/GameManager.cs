using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ManagerSingleton<GameManager>
{
	public GameObject Car;
    public GameObject CountText;
    public GameObject IntroCanvas;
    public float Speed;
	public float driftGauge;
	public TrailRenderer[] tireMarks;
	public Image BoostGauge;
	public GameObject[] BoostItem;
    public float BoosterTime;
    public bool useBooster;
    public float downForceValue;
    public float Angle;
    public bool StartRace;

    private void Start()
    {
        Application.targetFrameRate = 60;
        BoostItem[0].SetActive(false);
        BoostItem[1].SetActive(false);
        BoosterTime = 3.5f;
        downForceValue = 500.0f;
        Angle = Car.GetComponent<PlayerCar>().getSteerAngle();
        useBooster = false;
    }

    private void Update()
    {
        Speed = Mathf.Abs(Car.transform.GetComponent<PlayerCar>().getCurrentSpeed());
        StartRace = CountText.GetComponent<CountDown>().GetStart();

        if (tireMarks[0].emitting == true && tireMarks[1].emitting == true)
        {
            driftGauge += Time.deltaTime * Mathf.Abs(Input.GetAxis("Horizontal")) * 0.0001f * Speed;
            BoostGauge.fillAmount += driftGauge;
        }
        else if (tireMarks[0].emitting == false && tireMarks[1].emitting == false)
            driftGauge = 0.0f;

        GaugeUp();
        UseBooster();
    }

    public void TrailStartEmitter()
	{
        foreach (TrailRenderer trail in tireMarks)
            trail.emitting = true;
	}

	public void TrailStopEmitter()
	{
        foreach (TrailRenderer trail in tireMarks)
            trail.emitting = false;
    }

    void GaugeUp()
    {
        if (BoostGauge.fillAmount == 1.0f &&
            tireMarks[0].emitting == false && tireMarks[1].emitting == false)
        {
            if (BoostItem[0].activeInHierarchy == false)
            {
                BoostItem[0].SetActive(true);
                BoostGauge.fillAmount = 0.0f;
            }
            else if (BoostItem[0].activeInHierarchy == true &&
                BoostItem[1].activeInHierarchy == false)
            {
               BoostItem[1].SetActive(true);
               BoostGauge.fillAmount = 0.0f;
            }
            else if (BoostItem[0].activeInHierarchy == true &&
                BoostItem[1].activeInHierarchy == true)
                BoostGauge.fillAmount = 0.0f;
        }
    }

    void UseBooster()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && useBooster == false)
        {
            if (BoostItem[0].activeInHierarchy == true &&
                BoostItem[1].activeInHierarchy == false)
            {
                useBooster = true;
                BoostItem[0].SetActive(false);
            }
            else if (BoostItem[0].activeInHierarchy == true &&
                BoostItem[1].activeInHierarchy == true)
            {
                useBooster = true;
                BoostItem[1].SetActive(false);
            }
        }

        if (useBooster == true)
            BoosterTime = BoosterTime - Time.deltaTime >= 0.0f ? BoosterTime - Time.deltaTime : 0.0f;

        if (BoosterTime == 0.0f)
        {
            useBooster = false;
            BoosterTime = 3.5f;
        }
    }
}
