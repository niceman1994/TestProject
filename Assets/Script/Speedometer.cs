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
    // TODO : 100�̸��� �ӵ����� �տ� 0�� �������� �����ؾ���
    void Update()
    {
        speedText.text = Mathf.Abs(Speed).ToString();
       //speedText.text = Speed.ToString();
    }
}
