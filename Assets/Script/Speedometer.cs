using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField] private Text speedText;
    float Speed;

    void Start()
    {
        Speed = GameManager.Instance.Car.GetComponent<PlayerCar>().currentSpeed;
        speedText.text = "000";
    }
    // TODO : 100미만의 속도에선 앞에 0이 나오도록 수정해야함
    void Update()
    {
        speedText.text = Mathf.Abs(Speed).ToString();
       //speedText.text = Speed.ToString();
    }
}
