using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject Car;

	private void FixedUpdate()
	{
		transform.position = new Vector3(Car.transform.position.x + 5.0f,
			Car.transform.position.y + 1.0f, Car.transform.position.z);
	}
}
