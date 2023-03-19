using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField] private int CountNum;
    [SerializeField] private Text Count;
    [SerializeField] private Transform MoveLocation;
    [SerializeField] private AudioSource CountSound;
    [SerializeField] private bool RaceStart;

    private void Start()
    {
        RaceStart = false;
        CountNum = 3;
        StartCoroutine(countDown());
    }

    IEnumerator countDown()
    {
        yield return null;

        CountSound.Play();
        Count.text = CountNum.ToString();
        yield return new WaitForSeconds(1.0f);

        CountNum -= 1;
        Count.text = CountNum.ToString();
        yield return new WaitForSeconds(1.0f);

        CountNum -= 1;
        Count.text = CountNum.ToString();
        yield return new WaitForSeconds(1.0f);

        Count.text = "Start";
        RaceStart = true;
        yield return new WaitForSeconds(1.0f);
        Count.SetActive(false);
    }

    public bool GetStart()
    {
        bool _Start = RaceStart;
        return _Start;
    }
}
