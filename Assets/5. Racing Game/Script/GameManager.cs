using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : ManagerSingleton<GameManager>
{
	public GameObject Car;
    public GameObject CountText;
    public GameObject IntroCanvas;
    public Transform UI;
    public float Speed;
    public float driftAngle;
	public float driftGauge;
    public TrailRenderer[] tireMarks;
    public Image BoostGauge;
	public GameObject[] BoostItem;
    public float BoosterTime;
    public bool useBooster;
    public float downForceValue;
    public bool StartRace;

    public int CountNum;
    public Text Count;
    public AudioSource CountSound;

    private void Start()
    {
        Application.targetFrameRate = 60;
        BoostItem[0].SetActive(false);
        BoostItem[1].SetActive(false);
        BoosterTime = 3.5f;
        downForceValue = 500.0f;
        useBooster = false;
        StartRace = false;
        CountNum = 3;
        tireMarks[0].emitting = false;
        tireMarks[1].emitting = false;
        StartCoroutine(countDown());
    }

    private void Update()
    {
        RaceStart();
        Speed = Mathf.Abs(Car.transform.GetComponent<PlayerCar>().getCurrentSpeed());

        if (tireMarks[0].emitting == true && tireMarks[1].emitting == true)
        {
            driftGauge = Time.deltaTime * 0.005f;
            BoostGauge.fillAmount += driftGauge;
        }
        else if (tireMarks[0].emitting == false && tireMarks[1].emitting == false)
            driftGauge = 0.0f;

        GaugeUp();
        UseBooster();
    }

    float driftangle()
    {
        driftAngle = Car.GetComponent<PlayerCar>().getSteerAngle();
        return driftAngle;
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

    void RaceStart()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            if (IntroCanvas.activeInHierarchy == true)
            {
                IntroCanvas.SetActive(false);
                UI.SetActive(true);
                StartRace = true;
            }
        }
    }

    public IEnumerator countDown()
    {
        while (true)
        {
            yield return null;

            if (StartRace == true)
            {
                CountSound.Play();
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = CountNum.ToString();
                yield return new WaitForSeconds(1.0f);

                CountNum -= 1;
                Count.text = "Start";
                yield return new WaitForSeconds(1.0f);
                Count.SetActive(false);

                if (CountNum == 0)
                {
                    CountNum = 0;
                    yield return null;
                    CountSound.Stop();
                    yield break;
                }
            }
        }
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
