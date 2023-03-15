using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
	public GameObject Car;
	public GameObject CarView;
	public float Speed;

	private void Update()
	{
		CarView.transform.position = Car.transform.position;
		Speed = Mathf.Abs(Car.transform.GetComponent<PlayerCar>().currentSpeed);
	}
}
