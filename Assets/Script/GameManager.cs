using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
	public GameObject Car;
	public GameObject CarView;

	private void Update()
	{
		CarView.transform.position = Car.transform.position;
	}
}
