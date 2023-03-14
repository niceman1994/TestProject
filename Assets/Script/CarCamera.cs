using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
	[SerializeField] private Vector3 Direction;

	private void Start()
	{
		Direction = transform.position - GameManager.Instance.Car.transform.position;
	}

	private void FixedUpdate()
	{
		CameraPos();
	}
	// TODO : 각도 추후 수정
	void CameraPos()
	{
		transform.position = GameManager.Instance.Car.transform.position + Direction;

		if (GameManager.Instance.Car.transform.rotation.y >= GameManager.Instance.Car.transform.rotation.y - 75.0f &&
			GameManager.Instance.Car.transform.rotation.y <= GameManager.Instance.Car.transform.rotation.y + 75.0f)
			transform.LookAt(GameManager.Instance.Car.transform.position, Vector3.up);
	}
}
