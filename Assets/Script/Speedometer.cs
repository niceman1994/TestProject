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

    void Update()
    {
        speedText.text = Speed.ToString();
    }
}
